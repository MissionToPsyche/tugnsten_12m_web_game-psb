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

    public const float thrustRate = 0.5f;

    public void Awake()
    {
        currentVelocity = initialVelocity;
        Time.fixedDeltaTime = physicsTimeStep;
        orbit.parent = parent;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentVelocity += thrustRate * Time.deltaTime * currentVelocity.normalized;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            currentVelocity += thrustRate * Time.deltaTime * -currentVelocity.normalized;
        }
    }

    public void FixedUpdate()
    {
        UpdateVelocity();
        UpdatePosition();
        orbit.DrawOrbit(transform.position, currentVelocity);
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
}
