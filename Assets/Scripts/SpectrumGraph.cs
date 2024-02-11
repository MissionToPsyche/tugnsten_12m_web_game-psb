using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpectrumGraph : MonoBehaviour
{
    public List<Element> elements;
    public LineRenderer lr;

    public bool showPeaks = false;

    // Distance between calculated points
    public const float graphStep = 0.5f;
    private Vector3[] graphPoints;

    void OnValidate()
    {
        lr = GetComponent<LineRenderer>();

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
        int graphStart = EmissionSpectra.spectraRange.Item1;
        int graphEnd = EmissionSpectra.spectraRange.Item2;

        int numPoints = Mathf.RoundToInt(graphEnd - graphStart / graphStep);

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
        foreach (Element element in elements)
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

                    points[i] = new(x, y);
                    x += graphStep;
                }

                // Adds each element of points to each corresponding element in
                // combined points. 
                combinedPoints = combinedPoints.Zip(points, (a, b) => a + b).ToArray();
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
}
