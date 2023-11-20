using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Orbit : MonoBehaviour
{
    public const int maxCalcSteps = 10000;
    public const int ellipsePoints = 200;
    public Vector3[] orbitLinePositions;
    public LineRenderer lr;
    public PointMass parent;
    public GameObject periapsisMarker;
    public GameObject apoapsisMarker;
    public Vector3 apoapsisPosition;
    public Vector3 periapsisPosition;
    public float apoapsisDistance;
    public float periapsisDistance;
    public float rotation;

    public void OnValidate()
    {
        lr = GetComponent<LineRenderer>();

        // Constrains the apoapsis to be greater than the periapsis
        apoapsisDistance = Mathf.Max(apoapsisDistance, periapsisDistance);
        
        CalcOrbitFixed();
        DrawOrbit();
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

    // Uses periapsis altitude, apoapsis altitude, and rotation to calculate
    // orbit path.
    public void CalcOrbitFixed()
    {
        float semiMajorAxis = (apoapsisDistance + periapsisDistance) / 2;
        // Focus / semi-major axis
        float eccentricity = (semiMajorAxis - periapsisDistance) / semiMajorAxis;
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity);
        
        Vector3[] points = new Vector3[ellipsePoints];
        float angleStep = 360f / ellipsePoints;
        
        // Calculates the points along the ellipse
        for (int i = 0; i < ellipsePoints; i++)
        {
            float angle = i * angleStep;
            float x = semiMajorAxis * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = semiMinorAxis * Mathf.Sin(angle * Mathf.Deg2Rad);

            points[i] = new Vector3(x, y, 0);
        }

        // Rotates each point around the origin
        for (int i = 0; i < ellipsePoints; i++) {
            points[i] = Quaternion.Euler(0,0, rotation) * points[i];
        }

        // Orbit initially has the planet at its center, so its periapsis and
        // apoapsis distance is equal to its semi-major axis. It needs to be
        // shifted along its semi-major axis by the difference between the
        // current position (semi-major axis) and the desired periapsis height. 

        float shiftDist = semiMajorAxis - periapsisDistance;
        // Vector pointing from periapsis to apoapsis
        Vector3 axisDir = (apoapsisPosition - periapsisPosition).normalized;
        Vector3 shiftVector = axisDir * shiftDist;

        for (int i = 0; i < ellipsePoints; i++) {
            points[i] += shiftVector;
        }

        orbitLinePositions = points;

        // Calculates apoapsis and periapsis
        periapsisPosition = BinarySearchMin(points);
        apoapsisPosition = BinarySearchMax(points);
    }

    public void CalcOrbitFromOrbiter(Vector3 position, Vector3 velocity)
    {
        int usedSteps = maxCalcSteps;

        // This "virtual" orbiter will step through the real one's orbit without
        // actually moving the real one. 
        VirtualOrbiter virtualOrbiter = new(parent, velocity, position);

        Vector3[] points = new Vector3[maxCalcSteps];

        // Step the orbit forward until either it completes a full orbit or
        // reaches maxSteps many increments.
        for (int step = 0; step < maxCalcSteps; step++)
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

        orbitLinePositions = points;

        // Calculates apoapsis and periapsis
        periapsisPosition = BinarySearchMin(points);
        periapsisDistance = Vector3.Distance(periapsisPosition, parent.transform.position);
        apoapsisPosition = BinarySearchMax(points);
        apoapsisDistance = Vector3.Distance(apoapsisPosition, parent.transform.position);

        rotation = Vector3.Angle(periapsisPosition, Vector3.up);
    }

    public void DrawOrbit()
    {
        // Draws orbit line
        lr.positionCount = orbitLinePositions.Length;
        lr.SetPositions(orbitLinePositions);

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
