using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TorusGenerator : MonoBehaviour
{
    private int numPoints;
    private GameObject torusObject;
    private float rotationAngle = 0f;

    public Torus drawTorus(int numEllipses, int numPoints)
    {
        this.numPoints = numPoints;
        this.torusObject = new GameObject("MagneticTorus");
        Torus torus = new Torus();
        torus.torusObject = torusObject;

        List<float> magMoments = new List<float>();
        magMoments.Add(0f);
        magMoments.Add(2f * Mathf.Pow(10f, 14f));
        for (int i = 0; i < 8; i++)
        {
            float magMoment = Random.Range(2 * Mathf.Pow(10f, 7f), 2 * Mathf.Pow(10f, 14f));
            magMoments.Add(magMoment);
        }

        int index = Random.Range(0, magMoments.Count);
        float magMomentMagnitudeX = magMoments[index];
        index = Random.Range(0, magMoments.Count);
        float magMomentMagnitudeY = magMoments[index];
        Vector3 magneticMoment = new Vector3(magMomentMagnitudeX, magMomentMagnitudeY, 0);
        // Debug.Log("mag mom: " + magneticMoment);
        // magneticMoment = new Vector3(2 * Mathf.Pow(10f, 14f), 0, 0);
        // magneticMoment = new Vector3(8 * Mathf.Pow(10f, 22f), 0, 0);
        torus.magneticMoment = magneticMoment;

        // Calculate the angle between the magnetic moment and the position vector
        Vector3 cross = Vector3.Cross(Vector3.right.normalized, magneticMoment.normalized);
        rotationAngle = Mathf.Acos(Vector3.Dot(Vector3.right.normalized, magneticMoment.normalized)) * Mathf.Rad2Deg;
        // Adjust angle sign based on the direction of the cross product
        rotationAngle *= Mathf.Sign(Vector3.Dot(cross, Vector3.back));

        float ellipseFactor = 2.5f;
        float ellipseRatio = 2f;
        int reflection = 1;
        int ellipseNum = 1;

        // generates 2x numEllipses
        for (int i = 1; i <= numEllipses; i++)
        {
            float semiMajorAxis = (0.75f * (i) + Mathf.Pow(2, i) / (i + 2)) / ellipseFactor;
            float semiMinorAxis = (0.75f * 0.75f * (i) + Mathf.Pow(2, i) / (i + 1)) / ellipseFactor / ellipseRatio;
            // Debug.Log("major: " + semiMajorAxis);
            // Debug.Log("minor: " + semiMinorAxis);
            Vector3 ellipseCenter = new Vector3(0, semiMinorAxis, 0);

            Ellipse ellipse = new Ellipse(semiMajorAxis, semiMinorAxis, ellipseCenter);

            createEllipse(ellipseNum, reflection, ellipse);

            // flips across x axis and resets to generate second half of ellipses
            if (i == 5 && reflection == 1)
            {
                i = 0;
                reflection = -1;
            }

            torus.getEllipses().Add(ellipse);
            ellipseNum++;
        }

        setScaleAndRotation();
        return torus;
    }

    private void createEllipse(int ellipseNum, int reflection, Ellipse ellipse)
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;
        float semiMajorAxis = ellipse.semiMajorAxis;
        float semiMinorAxis = ellipse.semiMinorAxis;
        Vector3 ellipseCenter = ellipse.center;
        List<(int, Vector3)> usablePoints = new List<(int, Vector3)>();

        // generates points to form ellipse
        for (int i = 0; i < numPoints; i++)
        {
            float angle = (i * angleStep) + 270;
            angle *= Mathf.Deg2Rad;

            float x = semiMajorAxis * Mathf.Cos(angle);
            float y = reflection * (semiMinorAxis * Mathf.Sin(angle) + ellipseCenter.y);

            float rotatedX = (x * Mathf.Cos(rotationAngle)) - (y * Mathf.Sin(rotationAngle));
            float rotatedY = (x * Mathf.Sin(rotationAngle)) + (y * Mathf.Cos(rotationAngle));

            points[i] = new Vector3(rotatedX, rotatedY, 0);

            if(points[i].magnitude > 1)
            {
                usablePoints.Add((i, points[i]));
            }
        }

        ellipse.usablePoints = usablePoints;
        drawEllipse(ellipseNum, points, ellipse);
    }

    private void drawEllipse(int ellipseNum, Vector3[] points, Ellipse ellipse)
    {
        // Create a new GameObject
        GameObject lineObject = new GameObject("Ellipse" + (ellipseNum - 1));

        // set ellipse gameobject as a child of torus gameobject
        lineObject.transform.SetParent(torusObject.transform, false);
        lineObject.transform.localScale = Vector3.one;

        // Attach LineRenderer component to the new GameObject
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // makes line renderer scale/rotate/transform with parent (torusObject)
        lineRenderer.useWorldSpace = false;

        // Assign the points to the LineRenderer
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        // set material
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        // close loop
        lineRenderer.loop = true;

        // set color and width
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        ellipse.lineObject = lineObject;
    }

    private void setScaleAndRotation()
    {
        Transform t = torusObject.transform;

        int zRotation = Random.Range(0, 360);
        float scaleFactor = Random.Range(0.3f, 1.0f); // keep torus in screen and bigger than Psyche
        Vector3 scale = new(scaleFactor, scaleFactor, scaleFactor);

        // t.eulerAngles = new Vector3(0, 0, (float)zRotation);
        // t.localScale = scale;
    }
}
