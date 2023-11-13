using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Orbit : MonoBehaviour
{
    public PointMass parent;
    public LineRenderer lr;
    public Vector3 apoapsisPosition;
    public Vector3 periapsisPosition;
    public float apoapsisDistance;
    public float periapsisDistance;

    // TODO: Add constructor with peri/apo and rotation

    public void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Finds the maximum point by magnitude in an array known to be unimodal
    private Vector3 BinarySearchMax(Vector3[] points) {
        int left = 0;
        int right = points.Length - 1;

        while (left < right) {
            int mid = left + (right - left) / 2;

            if(points[mid].magnitude < points[mid + 1].magnitude) {
                left = mid + 1;
            } else {
                right = mid;
            }
        }

        return points[left];
    }

    // Finds the minimum point by magnitude in an array known to be unimodal
    private Vector3 BinarySearchMin(Vector3[] points) {
        int left = 0;
        int right = points.Length - 1;

        while (left < right) {
            int mid = left + (right - left) / 2;

            if(points[mid].magnitude > points[mid + 1].magnitude) {
                left = mid + 1;
            } else {
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
            if ((points[0] - points[step]).magnitude < .01 && step > 100)
            {
                usedSteps = step;
                break;
            }
        }

        // Draws orbit line
        lr.positionCount = usedSteps;
        lr.SetPositions(points);

        // Calculates apo/peri
        // NOTE: This assumes that the parent will always be at (0, 0, 0) for optimization
        periapsisPosition = BinarySearchMin(points);
        periapsisDistance = periapsisPosition.magnitude;
        apoapsisPosition = BinarySearchMax(points);
        apoapsisDistance = apoapsisPosition.magnitude;

        // Draws apo/peri

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
