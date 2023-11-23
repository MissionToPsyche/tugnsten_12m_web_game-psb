// Based on https://github.com/SebLague/Solar-System/tree/Episode_01
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : PointMass
{
    public PointMass parent;
    public Orbit orbit;
    public Vector3 initialVelocity;
    private Vector3 currentVelocity;

    public const float thrustRate = 0.15f;
    public const float rotationRate = 2f;

    public bool active = false; // Enables player control

    private void OnValidate()
    {
        currentVelocity = initialVelocity;
        Time.fixedDeltaTime = physicsTimeStep;
        orbit.parent = parent;
        orbit.isTargetOrbit = false;

        orbit.CalcOrbitFromOrbiter(transform.position, currentVelocity);
        orbit.DrawOrbit();
    }

    private void Start()
    {
        currentVelocity = initialVelocity;
    }

    public void FixedUpdate()
    {
        if (active)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                AlignPrograde();
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                AlignRetrograde();
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                AlignRadialOut();
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                AlignRadialIn();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                ApplyThrustForward();
            }
        }

        if (!orbit.hasCrashed && !orbit.hasEscaped)
        {
            UpdateVelocity();
            UpdatePosition();
            orbit.CalcOrbitFromOrbiter(transform.position, currentVelocity);
            orbit.DrawOrbit();
        }
    }

    private Vector3 GetNewAcceleration()
    {
        Vector3 dist = parent.transform.position - transform.position;
        float sqrDist = dist.sqrMagnitude;
        Vector3 dir = dist.normalized;
        // This is the force equation combined with the acceleration equation,
        // since there's a "* mass / mass" that cancels between the two.
        Vector3 acceleration = gravitationalConstant * parent.mass * dir / sqrDist;
        return acceleration;
    }

    private Vector3 GetNewVelocity()
    {
        return GetNewAcceleration() * physicsTimeStep;
    }

    private void UpdateVelocity()
    {
        currentVelocity += GetNewVelocity();
    }

    private Vector3 GetNewPosition()
    {
        return currentVelocity * physicsTimeStep;
    }

    private void UpdatePosition()
    {
        transform.position += GetNewPosition();
    }

    private void AlignPrograde()
    {
        float angle = Vector3.SignedAngle(Vector3.up, currentVelocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 2);
    }

    private void AlignRetrograde()
    {
        float angle = Vector3.SignedAngle(Vector3.up, -currentVelocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 2);
    }

    private void AlignRadialIn()
    {
        float angle = Vector3.SignedAngle(Vector3.right, currentVelocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 2);
    }

    private void AlignRadialOut()
    {
        float angle = Vector3.SignedAngle(Vector3.right, -currentVelocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 2);
    }

    private void ApplyThrustForward()
    {
        currentVelocity += thrustRate * Time.deltaTime * transform.up;
    }

    public void ResetSpacecraft()
    {
        orbit.ResetOrbit();
        // UpdateVelocity();
        // UpdatePosition();
        currentVelocity = initialVelocity;
        active = true;
    }
}
