using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusGenerator : MonoBehaviour
{
    private int numPoints;
    private GameObject torus;

    void Start()
    {

    }

    public void drawTorus(int numEllipses, GameObject torus, int numPoints)
    {
        this.numPoints = numPoints;
        this.torus = torus;

        float ellipseFactor = 2f;
        float ellipseRatio = 2f;
        int reflection = 1;
        int ellipseNum = 1;

        // generates 2x numEllipses
        for(int i = 1; i <= numEllipses; i++)
        {
            float semiMajorAxis = (0.75f * (i) + Mathf.Pow(2, i)/(i+2))/ellipseFactor;
            float semiMinorAxis = (0.75f * 0.75f * (i) + Mathf.Pow(2, i)/(i+1))/ellipseFactor/ellipseRatio;

            createEllipse(ellipseNum, reflection, semiMajorAxis, semiMinorAxis);

            // flips across x axis and resets to generate second half of ellipses
            if(i == 5 && reflection == 1) 
            {
                i = 0;
                reflection = -1;
            }

            ellipseNum++;
        }

        setScaleAndRotation();
    }

    private void createEllipse(int ellipseNum, int reflection, float semiMajorAxis, float semiMinorAxis)
    {
        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;

        // generates points to form ellipse
        for(int i = 0; i < numPoints; i++)
        {
            float angle = i * angleStep;
            points[i] = new Vector3(semiMajorAxis * Mathf.Cos(angle * Mathf.Deg2Rad), reflection*(semiMinorAxis * Mathf.Sin(angle * Mathf.Deg2Rad) + semiMinorAxis), 0);
        }

        drawEllipse(ellipseNum, points);
    }

    private void drawEllipse(int ellipseNum, Vector3[] points)
    {
        // Create a new GameObject
        GameObject lineObject = new GameObject("Ellipse" + (ellipseNum-1));

        // set ellipse gameobject as a child of torus gameobject
        lineObject.transform.SetParent(torus.transform, false);
        lineObject.transform.localScale = Vector3.one;

        // Attach LineRenderer component to the new GameObject
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // makes line renderer scale/rotate/transform with parent (torus)
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
    }

    private void setScaleAndRotation()
    {
        Transform t = torus.transform;

        int zRotation = Random.Range(0, 360);
        float scaleFactor = Random.Range(0.3f, 1.1f); // keep torus in screen and bigger than Psyche
        Vector3 scale = new(scaleFactor, scaleFactor, scaleFactor);

        t.eulerAngles = new Vector3(0, 0, (float)zRotation);
        t.localScale = scale;
    }
}
