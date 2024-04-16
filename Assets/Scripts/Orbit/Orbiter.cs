// Based on https://github.com/SebLague/Solar-System/tree/Episode_01
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : PointMass
{
    public PointMass parent;
    public Orbit orbit;
    public ParticleSystem engine;
    // public Vector3 initialVelocity;
    public Vector3 velocity;

    public const float maxThrust = 0.15f;
    public const float reducedThrustPercent = 0.15f;
    private const float reducedThrust = maxThrust * reducedThrustPercent;
    public const float rotationRate = 200f;

    public bool toggleRotation = false;
    public int rotation = 1; // 1 = forward, 2 = backward, 3 = out, 4 = in
    public bool toggleThrustAmount = false;
    public bool reducedThrustEnabled = false;
    public bool controllable = false; // Enables player control
    public bool active = false; // Enables movement

    public float fuelUsed = 0;

    private void OnValidate()
    {
        // currentVelocity = initialVelocity;
        Time.fixedDeltaTime = physicsTimeStep;

        // Exit early if parent is not set
        if (parent == null)
        {
            return;
        }

        orbit.parent = parent;
        orbit.isTargetOrbit = false;

        orbit.CalcOrbitFromOrbiter(transform.position, velocity);
        orbit.DrawOrbit();
    }

    private void Start()
    {
        // currentVelocity = initialVelocity;
    }

    private void Update()
    {
        if (controllable)
        {
            /* ----------------------- Rotation logic ----------------------- */
            if (Input.GetKey(KeyCode.W))
            {
                AlignPrograde();
                rotation = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                AlignRetrograde();
                rotation = 2;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                AlignRadialOut();
                rotation = 3;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                AlignRadialIn();
                rotation = 4;
            }
            else if (toggleRotation)
            {
                // Holds the last pressed direction if enabled. This must only
                // happen if no button is pressed to prevent double input.
                switch (rotation)
                {
                    case 1:
                        AlignPrograde();
                        break;
                    case 2:
                        AlignRetrograde();
                        break;
                    case 3:
                        AlignRadialOut();
                        break;
                    case 4:
                        AlignRadialIn();
                        break;
                }
            }

            /* ------------------------ Thrust logic ------------------------ */
            // If toggle mode on
            if (toggleThrustAmount)
            {
                // Toggles reduced thrust with shift
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    reducedThrustEnabled = !reducedThrustEnabled;
                }
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                // Do reduced thrust if toggled on or shift held, otherwise do full thrust
                if (reducedThrustEnabled || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    ApplyThrustForward(reducedThrust);
                    SoundManager.Instance.playLightThrusterSound();
                }
                else
                {
                    ApplyThrustForward(maxThrust);
                    SoundManager.Instance.playThrusterSound();
                }

                // Turn engine particles on
                var emission = engine.emission;
                emission.enabled = true;
            } else {
                // Turn engine particles off
                var emission = engine.emission;
                emission.enabled = false;
                SoundManager.Instance.stopSound();
            }
        }
    }

    public void FixedUpdate()
    {
        if (!orbit.hasCrashed && !orbit.hasEscaped && active)
        {
            UpdatePosition();
            UpdateVelocity();
            orbit.CalcOrbitFromOrbiter(transform.position, velocity);
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

    public void UpdateVelocity()
    {
        velocity += GetNewVelocity();
    }

    private Vector3 GetNewPosition()
    {
        return velocity * physicsTimeStep;
    }

    public void UpdatePosition()
    {
        transform.position += GetNewPosition();
    }

    public void AlignPrograde()
    {
        float angle = Vector3.SignedAngle(Vector3.up, velocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);

        // Prevents going the long way around when rotating
        if (Quaternion.Angle(transform.rotation, target) > 180)
        {
            target *= Quaternion.Euler(0, 0, 180);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationRate * Time.deltaTime);
    }

    public void AlignRetrograde()
    {
        float angle = Vector3.SignedAngle(Vector3.up, -velocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);

        if (Quaternion.Angle(transform.rotation, target) > 180)
        {
            target *= Quaternion.Euler(0, 0, 180);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationRate * Time.deltaTime);
    }

    public void AlignRadialIn()
    {
        float angle = Vector3.SignedAngle(Vector3.right, velocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);

        if (Quaternion.Angle(transform.rotation, target) > 180)
        {
            target *= Quaternion.Euler(0, 0, 180);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationRate * Time.deltaTime);
    }

    public void AlignRadialOut()
    {
        float angle = Vector3.SignedAngle(Vector3.right, -velocity, Vector3.forward);
        Quaternion target = Quaternion.Euler(0, 0, angle);

        if (Quaternion.Angle(transform.rotation, target) > 180)
        {
            target *= Quaternion.Euler(0, 0, 180);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationRate * Time.deltaTime);
    }

    public void ApplyThrustForward(float thrustRate)
    {
        velocity += thrustRate * Time.deltaTime * transform.up;
        fuelUsed += thrustRate * Time.deltaTime;
    }

    public void ResetSpacecraft(Vector3 position, Vector3 velocity)
    {
        orbit.ResetOrbit();
        transform.position = position;
        this.velocity = velocity;
        fuelUsed = 0;

        controllable = true;
    }
}
