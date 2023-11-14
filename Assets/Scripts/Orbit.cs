using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Orbit : MonoBehaviour
{
    public LineRenderer lr;
    public GameObject periapsisMarker;
    public GameObject apoapsisMarker;
    public PointMass parent;
    public Vector3 apoapsisPosition;
    public Vector3 periapsisPosition;
    public float apoapsisDistance;
    public float periapsisDistance;

    // TODO: Add constructor with peri/apo and rotation

    public void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Finds the maximum point by altitude in an array known to be unimodal
    private Vector3 BinarySearchMax(Vector3[] points)
    {
        int left = 0;
        int right = points.Length - 1;

        while (left < right)
        {
            int mid = (left + right) / 2;

            if (Vector3.Distance(points[mid], parent.transform.position) < Vector3.Distance(points[mid + 1], parent.transform.position))
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }

        return points[left];
    }

    // Finds the minimum point by altitude in an array known to be unimodal
    private Vector3 BinarySearchMin(Vector3[] points)
    {
        int left = 0;
        int right = points.Length - 1;

        while (left < right)
        {
            int mid = (left + right) / 2;

            if (Vector3.Distance(points[mid], parent.transform.position) > Vector3.Distance(points[mid + 1], parent.transform.position))
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }

        return points[left];
    }

    public void DrawOrbit(Vector3 position, Vector3 velocity)
    {
        const int maxSteps = 10000;
        int usedSteps = maxSteps;

        // This "virtual" orbiter will step through the real one's orbit without
        // actually moving the real one. 
        VirtualOrbiter virtualOrbiter = new(parent, velocity, position);

        Vector3[] points = new Vector3[maxSteps];

        // Step the orbit forward until either it completes a full orbit or
        // reaches maxSteps many increments.
        for (int step = 0; step < maxSteps; step++)
        {
            virtualOrbiter.UpdateVelocity();
            virtualOrbiter.UpdatePosition();

            points[step] = virtualOrbiter.position;

            // Breaks the calculation early if the virtual orbiter has completed
            // a full orbit, which is detected by checking if the first position
            // calculated is very close to the current one.
            if (Vector3.Distance(points[0], points[step]) < .01 && step > 100)
            {
                usedSteps = step;
                break;
            }
        }

        Array.Resize(ref points, usedSteps);

        // Draws orbit line
        lr.positionCount = usedSteps;
        lr.SetPositions(points);

        // Calculates apoapsis and periapsis
        periapsisPosition = BinarySearchMin(points);
        periapsisDistance = Vector3.Distance(periapsisPosition, parent.transform.position);
        apoapsisPosition = BinarySearchMax(points);
        apoapsisDistance = Vector3.Distance(apoapsisPosition, parent.transform.position);

        // Small offset to avoid clipping into the orbit line
        Vector3 zOffset = new(0, 0, -0.001f);

        // Positions apoapsis and periapsis markers apo/peri
        apoapsisMarker.transform.position = apoapsisPosition + zOffset;
        periapsisMarker.transform.position = periapsisPosition + zOffset;
    }

    // This is essentially the Orbiter class but stripped down. We can't use the 
    // actual Orbiter class because we can't make new instances of Unity monobehaviors.
    private class VirtualOrbiter
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
            Vector3 acceleration = PointMass.gravitationalConstant * parent.mass * dir / sqrDist;
            velocity += PointMass.physicsTimeStep * acceleration;
        }

        public void UpdatePosition()
        {
            position += PointMass.physicsTimeStep * velocity;
        }
    }
}
