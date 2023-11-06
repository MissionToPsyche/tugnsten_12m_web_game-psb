// Based on https://github.com/SebLague/Solar-System/tree/Episode_01
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Orbiter : PointMass
{
    public PointMass parent;
    public LineRenderer lr;
    public Vector3 initialVelocity;
    public Vector3 currentVelocity;

    private const float thrustRate = 0.5f;

    public void Awake()
    {
        currentVelocity = initialVelocity;
        Time.fixedDeltaTime = physicsTimeStep;

        lr = GetComponent<LineRenderer>();
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
        DrawOrbit();
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

    void DrawOrbit()
    {
        const int maxSteps = 10000;
        int usedSteps = maxSteps;

        // Duplicates the orbiter
        VirtualOrbiter virtualOrbiter = new(parent, currentVelocity, transform.position);

        Vector3[] points = new Vector3[maxSteps];

        // Step the orbit forward
        for (int step = 0; step < maxSteps; step++)
        {
            virtualOrbiter.UpdateVelocity();
            virtualOrbiter.UpdatePosition();

            points[step] = virtualOrbiter.position;

            if ((points[0] - points[step]).magnitude < .1 && step > 100)
            {
                Debug.Log("breaking");
                usedSteps = step;
                break;
            }
        }

        lr.positionCount = usedSteps;
        lr.SetPositions(points);
    }

    class VirtualOrbiter
    {
        private PointMass parent;
        private Vector3 velocity;
        public Vector3 position;

        public VirtualOrbiter(PointMass p, Vector3 vel, Vector3 pos)
        {
            parent = p;
            velocity = vel;
            position = pos;
        }

        public void UpdateVelocity()
        {
            Vector3 dist = parent.transform.position - position;
            float sqrDist = dist.sqrMagnitude;
            Vector3 dir = dist.normalized;
            Vector3 acceleration = gravitationalConstant * parent.mass * dir / sqrDist;
            velocity += physicsTimeStep * acceleration;
        }

        public void UpdatePosition()
        {
            position += physicsTimeStep * velocity;
        }
    }
}
