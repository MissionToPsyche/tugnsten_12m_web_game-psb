using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DistortedOrbit : MonoBehaviour
{
    public LineRenderer lr;
    public float lineWidth = 0.06f;

    public int ellipsePoints = 400;
    public Vector3[] orbitLine;
    public Vector3[] distortedOrbitLine;

    public float apoapsisDistance = 2;
    public float periapsisDistance = 1;
    [Range(0, 360)]
    public float rotation;

    private List<Distortion> distortions;

    // TEMP
    [Range(-1f, 1f)]
    public float amp = 1;
    [Range(0, 1f)]
    public float pos = 0;

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

        DrawOrbit();
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawOrbit();

        // TEMP
        distortions = new()
        {
            new(0, 1)
        };

    }

    // Update is called once per frame
    void Update()
    {
        distortions[0].intensity = amp;
        distortions[0].position = pos;
        ApplyDistortions();
    }

    void ApplyDistortions()
    {
        Vector3[] tempPositions = (Vector3[])orbitLine.Clone();
        
        foreach (Distortion distortion in distortions)
        {
            int centerPoint = Mathf.RoundToInt(ellipsePoints * distortion.position);

            // Bitwise OR ensures that the number of points is odd so it can be centered properly.
            int numPointsAffected = Mathf.RoundToInt(ellipsePoints * distortion.size) | 1;

            int halfPointsAffected = numPointsAffected / 2;

            // Loops over numPointsAffected x values, with half the range below 0 and half above. 
            for (int x = -halfPointsAffected; x <= halfPointsAffected; x++)
            {
                // Implements Gaussian function Ae^(-x^2/2c^2) where A is the
                // intensity and c is the width. Intensity is reduced by an
                // order of magnitude to make the distortions a reasonable size.
                float y = distortion.intensity / 10 * Mathf.Exp(-Mathf.Pow(x, 2) / Mathf.Pow(2 * 10, 2));

                // The point being operated on in terms of the actual orbit.
                int currentOrbitPoint = centerPoint + x;
                
                // Wraps the point if it falls off the beginning or end of the array.
                currentOrbitPoint = (currentOrbitPoint + ellipsePoints) % ellipsePoints;

                // Direction vector pointing away from Psyche through the current point.
                Vector3 radialDirection = tempPositions[currentOrbitPoint].normalized;

                // Shifts the point outward by y.
                tempPositions[currentOrbitPoint] += y * radialDirection;
            }

        }

        lr.positionCount = tempPositions.Length;
        lr.SetPositions(tempPositions);
        distortedOrbitLine = (Vector3[])tempPositions.Clone();
    }

    // Uses periapsis altitude, apoapsis altitude, and rotation to calculate
    // orbit path.
    public void CalcOrbit()
    {
        float semiMajorAxis = (apoapsisDistance + periapsisDistance) / 2;
        float focus = semiMajorAxis - periapsisDistance; // Ellipse center to Psyche
        float eccentricity = focus / semiMajorAxis;
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity);

        Vector3[] points = new Vector3[ellipsePoints];
        float angleStep = 360f / (ellipsePoints - 1);

        // Calculates the points along the ellipse
        for (int i = 0; i < ellipsePoints - 1; i++)
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

        // Make the last point identical to the first point to close the shape. 
        points[ellipsePoints - 1] = points[0];

        orbitLine = points;
    }

    public void DrawOrbit()
    {
        CalcOrbit();

        lr.positionCount = orbitLine.Length;
        lr.SetPositions(orbitLine);
    }
}
