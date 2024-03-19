using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Waveform : MonoBehaviour
{
    public LineRenderer lr;
    public Slider slider = null;

    public float wavelength = 300;

    // Pixel length of line
    public float length = 400f;

    public float verticalScale = 200f;
    public float lineWidth = 0.04f;

    // Distance between calculated points
    public const float graphStep = 2f;
    private Vector3[] graphPoints;

    public float animationRate = 0.2f;
    // Amount the sin wave is shifted along the x-axis, used to make it appear
    // to move.
    private float offset = 0f;

    protected void OnValidate()
    {
        lr = GetComponent<LineRenderer>();

        // TODO: fix this resource path
        lr.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.useWorldSpace = false;
        lr.startWidth = lineWidth;
    }

    void Start()
    {
        offset = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        DrawGraph();
        offset += animationRate * Time.deltaTime;

        // Reset invisibly after the graph reaches its start position.
        offset %= Mathf.PI * 2;
    }

    public void CalculatePoints()
    {
        int numPoints = Mathf.RoundToInt(length / graphStep);

        // The LineRenderer eventually needs a Vector3, even though this is
        // operating in two dimensions. 
        Vector3[] points = new Vector3[numPoints];

        // The x value is lifted out of the loop for performance, since
        // this avoids a (graphStep * i) calculation on each loop.
        float x = 0;

        for (int i = 0; i < numPoints; i++)
        {
            float y = Mathf.Sin(wavelength * x + offset) * verticalScale;

            points[i] = new(x, y);
            x += graphStep;
        }

        graphPoints = points;
    }

    public void DrawGraph()
    {
        CalculatePoints();

        lr.positionCount = graphPoints.Length;
        lr.SetPositions(graphPoints);
    }
}
