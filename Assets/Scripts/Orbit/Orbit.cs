using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Orbit : MonoBehaviour
{
    public int maxCalcSteps = 10000;
    public int ellipsePoints = 200;
    public float lineWidth = 0.06f;
    public float crashThreshold = 0.75f;
    public float escapeThreshold = 10;
    public Vector3[] orbitLinePositions;
    public LineRenderer lr;
    public PointMass parent;
    public GameObject periapsisMarker;
    public GameObject apoapsisMarker;
    public GameObject warningMarker;
    public Vector3 apoapsisPosition;
    public Vector3 periapsisPosition;
    public float apoapsisDistance = 2;
    public float periapsisDistance = 1;
    [Range(0, 360)]
    public float rotation;
    public bool isTargetOrbit = true;

    public bool isCrashing = false;
    public bool isEscaping = false;
    public bool hasCrashed = false;
    public bool hasEscaped = false;

    void Reset()
    {
        lr = GetComponent<LineRenderer>();

        // TODO: fix this resource path
        lr.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.useWorldSpace = false;
        lr.startWidth = lineWidth;

        DrawOrbit();
    }

    private void OnValidate()
    {
        lr = GetComponent<LineRenderer>();

        // Constrains the apoapsis to be greater than the periapsis
        apoapsisDistance = Mathf.Max(apoapsisDistance, periapsisDistance);

        // Hack so the markers work on a circular orbit
        if (apoapsisDistance == periapsisDistance)
        {
            apoapsisDistance += 0.01f;
        }

        // Skips the setup if the parent is not set
        if (parent == null) {
            return;
        }

        if (isTargetOrbit)
        {
            CalcOrbitFixed();
            DrawOrbit();
        }
    }

    private void Start() {
        warningMarker.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Finds the maximum point by altitude in an array known to be unimodal
    public Vector3 BinarySearchMax(Vector3[] points)
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
    public Vector3 BinarySearchMin(Vector3[] points)
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
        float focus = semiMajorAxis - periapsisDistance; // Ellipse center to Psyche
        float eccentricity = focus / semiMajorAxis;
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity);

        Vector3[] points = new Vector3[ellipsePoints];
        float angleStep = 360f / ellipsePoints;

        // Calculates the points along the ellipse
        for (int i = 0; i < ellipsePoints; i++)
        {
            float angle = i * angleStep;
            float x = semiMajorAxis * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = semiMinorAxis * Mathf.Sin(angle * Mathf.Deg2Rad);

            // Orbit initially has Psyche at its center, so it needs to be
            // shifted along its semi-major axis by the focus distance.
            points[i] = new Vector3(x + focus, y, 0);

            // Rotates each point around origin, which rotates the whole ellipse.
            points[i] = Quaternion.Euler(0, 0, rotation) * points[i];
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
        points[0] = position;

        // Step the orbit forward until either it completes a full orbit or
        // reaches maxSteps many increments.
        for (int step = 1; step < maxCalcSteps; step++)
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

            // Check if crashing
            if (Vector3.Distance(new(0, 0, 0), points[step]) < crashThreshold)
            {
                isCrashing = true;
                periapsisMarker.GetComponent<SpriteRenderer>().enabled = false; // Hides the periapsis marker
                apoapsisMarker.GetComponent<SpriteRenderer>().enabled = false; // Hides the apoapsis marker

                // Shows and positions the warning marker
                warningMarker.GetComponent<SpriteRenderer>().enabled = true;
                // Adds an offset to avoid clipping into Psyche
                warningMarker.transform.position = points[step] + new Vector3(0, 0, -2f);

                // If the crash is happening very soon
                if (step < 2)
                {
                    hasCrashed = true;
                }

                usedSteps = step;
                break;
            }
            else
            {
                isCrashing = false;
                periapsisMarker.GetComponent<SpriteRenderer>().enabled = true;
                apoapsisMarker.GetComponent<SpriteRenderer>().enabled = true;
                warningMarker.GetComponent<SpriteRenderer>().enabled = false;
            }

            // Check if escaping
            if (Vector3.Distance(new(0, 0, 0), points[step]) > escapeThreshold)
            {
                isEscaping = true;
                periapsisMarker.GetComponent<SpriteRenderer>().enabled = false; // Hides the periapsis marker
                apoapsisMarker.GetComponent<SpriteRenderer>().enabled = false; // Hides the apoapsis marker

                // Shows and positions the warning marker
                warningMarker.GetComponent<SpriteRenderer>().enabled = true;
                // Adds an offset to avoid clipping into Psyche
                warningMarker.transform.position = points[step] + new Vector3(0, 0, -2f);

                // If the escape is happening very soon
                if (step < 2)
                {
                    hasEscaped = true;
                }

                usedSteps = step;
                break;
            }
            else
            {
                isEscaping = false;
                periapsisMarker.GetComponent<SpriteRenderer>().enabled = true;
                apoapsisMarker.GetComponent<SpriteRenderer>().enabled = true;
                warningMarker.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        Array.Resize(ref points, usedSteps);

        orbitLinePositions = points;

        // Calculates apoapsis and periapsis
        periapsisPosition = BinarySearchMin(points);
        periapsisDistance = Vector3.Distance(periapsisPosition, parent.transform.position);
        apoapsisPosition = BinarySearchMax(points);
        apoapsisDistance = Vector3.Distance(apoapsisPosition, parent.transform.position);

        rotation = Vector3.Angle(periapsisPosition, Vector3.left);
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
    public class VirtualOrbiter
    {
        private PointMass parent;
        public Vector3 velocity;
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

    public void ResetOrbit()
    {
        isCrashing = false;
        isEscaping = false;
        hasCrashed = false;
        hasEscaped = false;
    }
}
