using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RailsOrbit : MonoBehaviour
{
    public LineRenderer lr;
    public float lineWidth = 0.06f;
    public int numOrbitPoints = 400;
    public Vector3[] orbitLine;

    public GameObject parent;

    public float apoapsisDistance = 2;
    public float periapsisDistance = 1;
    [Range(0, 360)]
    public float rotation;

    [Range(0, 360)]
    public float tilt;

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

        transform.position = parent.transform.position;

        DrawOrbit();
    }

    void Start()
    {
        lr.enabled = false;
    }

    // Uses periapsis altitude, apoapsis altitude, and rotation to calculate
    // orbit path.
    public void CalcOrbit()
    {
        float semiMajorAxis = (apoapsisDistance + periapsisDistance) / 2;
        float focus = semiMajorAxis - periapsisDistance; // Ellipse center to Psyche
        float eccentricity = focus / semiMajorAxis;
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity);

        Vector3[] points = new Vector3[numOrbitPoints];
        float angleStep = 360f / (numOrbitPoints - 1);

        // Calculates the points along the ellipse
        for (int i = 0; i < numOrbitPoints - 1; i++)
        {
            float angle = i * angleStep;
            float x = semiMajorAxis * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = semiMinorAxis * Mathf.Sin(angle * Mathf.Deg2Rad);

            // Orbit initially has Psyche at its center, so it needs to be
            // shifted along its semi-major axis by the focus distance.
            points[i] = new Vector3(x + focus, y, 0);

            // Rotates each point around origin, which rotates the whole ellipse.
            points[i] = Quaternion.Euler(tilt, 0, rotation) * points[i];
        }

        // Make the last point identical to the first point to close the shape. 
        points[numOrbitPoints - 1] = points[0];

        orbitLine = points;
    }

    public void DrawOrbit()
    {
        CalcOrbit();

        lr.positionCount = orbitLine.Length;
        lr.SetPositions(orbitLine);
    }
}
