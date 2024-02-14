using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RectTransform))]
public class SpectrumGraph : MonoBehaviour
{
    public Dictionary<string, Element> elements;
    public LineRenderer lr;
    public RectTransform rt;

    public bool showPeaks = false;

    public float horizontalScale = 1f;
    public float verticalScale = 100.0f;
    public float lineWidth = 0.04f;

    // Amount of x room to add beyond the min/max peak position, so the bell
    // curve isn't truncated at the edge of the graph.
    public int graphEndPadding = 100;

    // x values of the first and last points on the line
    public int graphStart;
    public int graphEnd;

    // Distance between calculated points
    public const float graphStep = 0.5f;
    private Vector3[] graphPoints;

    void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
        rt = GetComponent<RectTransform>();

        // TODO: fix this resource path
        lr.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.useWorldSpace = false;
        lr.startWidth = lineWidth;

        graphStart = EmissionSpectra.spectraRange.Item1 - graphEndPadding;
        graphEnd = EmissionSpectra.spectraRange.Item2 + graphEndPadding;

        // Moves the graph left half its width plus its start x value. Since the
        // LineRenderer draws points at their face coordinate values, and the
        // LineRender's own origin is at 0,0, this centers the line within its
        // parent transform.
        //
        // This also moves the line down by the maximum probable height, which
        // is two max strength peaks (1.0 each) at max quantity at the same x
        // value. That places the vertical center of the graph somewhere near
        // the vertical center of its parent transform. 
        rt.anchoredPosition = new(-((graphEnd - graphStart) / 2 + graphStart), -2 * verticalScale);

        DrawWindow();
    }

    private void Update()
    {
        DrawGraph();

        // If this graph only has one element, it's a control graph
        if (elements.Count == 1)
        {
            showPeaks = true;
            DrawPeaks();
        }
    }

    public void CalculatePoints()
    {
        int numPoints = Mathf.RoundToInt((graphEnd - graphStart) / graphStep);

        // The LineRenderer eventually needs a Vector3, even though this is
        // operating in two dimensions. 
        Vector3[] combinedPoints = new Vector3[numPoints];

        // Initializes combinedPoints to all (0, 0)s
        for (int i = 0; i < numPoints; i++)
        {
            combinedPoints[i] = new(0, 0);
        }

        // Calculates the graph points for each peak in each element using a
        // Gaussian function and combines them all into combinedPoints.
        foreach (Element element in elements.Values)
        {
            foreach (SpectrumPeak peak in element.peaks)
            {
                Vector3[] points = new Vector3[numPoints];

                // The x value is lifted out of the loop for performance, since
                // this avoids a (graphStep * i) calculation on each loop.
                float x = graphStart;

                for (int i = 0; i < numPoints; i++)
                {
                    // Implements Gaussian function Ae^(-(x - μ)^2/2σ^2) where A
                    // is the intensity, μ is the location, and σ is the width.
                    float y = peak.intensity * Mathf.Exp(-Mathf.Pow(x - peak.location, 2) / Mathf.Pow(2 * peak.width, 2));

                    // Scales according to quantity present
                    y *= element.quantity;

                    points[i] = new(x * horizontalScale, y * verticalScale);
                    x += graphStep;
                }

                // Adds the y values of each element in points to each
                // corresponding element in combined points. 
                for (int i = 0; i < numPoints; i++)
                {
                    Vector3 a = combinedPoints[i];
                    Vector3 b = points[i];

                    combinedPoints[i] = new Vector3(b.x, a.y + b.y, b.z);
                }
            }
        }

        graphPoints = combinedPoints;
    }

    public void DrawGraph()
    {
        CalculatePoints();

        lr.positionCount = graphPoints.Length;
        lr.SetPositions(graphPoints);
    }

    public void DrawPeaks()
    {
        // TODO
    }

    public void DrawWindow()
    {
        // TODO
    }
}
