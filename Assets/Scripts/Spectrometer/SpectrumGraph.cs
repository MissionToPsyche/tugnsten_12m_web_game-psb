using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RectTransform))]
public class SpectrumGraph : MonoBehaviour
{
    public SortedDictionary<string, Element> elements;
    public LineRenderer lr;
    public RectTransform rt;
    public GraphFrame frame;
    public Slider slider = null;

    public bool showPeaks = false;
    public bool isControl = false;

    public float horizontalScale = 1f;
    public float verticalScale = 200f;
    public float lineWidth = 0.04f;

    // Amount of x room to add beyond the min/max peak position, so the bell
    // curve isn't truncated at the edge of the graph.
    public int graphEndPadding = 100;

    // Amount of space between the graph's line and the axes
    public int graphMargin = 20;

    // Amount of space between the slider and the graph axis
    public int sliderMargin = 100;

    // Number of tick marks on the axes
    public int numXTicks = 10;
    public int numYTicks = 4;
    // Change between each tick mark label
    public int tickStep = 1;
    // Pixel size of the tick marks
    public int tickSize = 15;

    // x values of the first and last points on the line
    private int graphStart;
    private int graphEnd;

    // Distance between calculated points
    public const float graphStep = 2f;
    private Vector3[] graphPoints;

    protected void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
        rt = GetComponent<RectTransform>();
        RectTransform frameRt = frame.GetComponent<RectTransform>();

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
        // Similar logic for moving it down by half its height. 

        float xCenter = (graphEnd - graphStart) / 2f + graphStart;
        
        // TODO: fix this
        float yCenter = (graphEnd - graphStart) / (numXTicks / (float)numYTicks) / 2f;
        
        Vector2 newTransform = new(-xCenter, -yCenter);

        rt.anchoredPosition = newTransform;
        frameRt.anchoredPosition = newTransform;

        DrawFrame();

        if (isControl)
        {
            slider.direction = Slider.Direction.BottomToTop;
            slider.GetComponent<RectTransform>().anchoredPosition = new(-(xCenter / 2f + graphStart + sliderMargin), 0);
        }
    }

    private void Update()
    {
        DrawFrame();
        DrawGraph();
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
                    // Implements Gaussian function Ae^(-(x - b)^2/2c^2) where A
                    // is the intensity, b is the location, and c is the width.
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

    public void DrawFrame()
    {
        // Pixel width of the graph's line
        int graphWidth = graphEnd - graphStart;

        int tickSeparation = graphWidth / numXTicks;
        
        // Bottom right of the graph
        Vector3 currentPoint = new(graphEnd, -graphMargin);
        
        // Points for the LineRenderer
        List<Vector3> points = new() {currentPoint};
        
        // Draws the x axis, ends with currentPoint at graph origin
        for (int i = 0; i < numXTicks; i++)
        {
            // Move down to point of tick
            currentPoint.y -= tickSize;
            points.Add(currentPoint);

            // TODO: Draw label

            // Move back to tick base
            currentPoint.y += tickSize;
            points.Add(currentPoint);

            // Move left one tick
            currentPoint.x -= tickSeparation;
            points.Add(currentPoint);
        }
        
        // Move from origin up to first tick base
        currentPoint.y += tickSeparation;
        points.Add(currentPoint);

        // Draws the y axis, ends with currentPoint at top left tick base
        for (int i = 0; i < numYTicks; i++)
        {
            // Move left to point of tick
            currentPoint.x -= tickSize;
            points.Add(currentPoint);

            // TODO: Draw label

            // Move back to tick base
            currentPoint.x += tickSize;
            points.Add(currentPoint);

            // Move up one tick
            currentPoint.y += tickSeparation;
            points.Add(currentPoint);
        }

        // Draws the point of the last y-axis tick
        currentPoint.x -= tickSize;
        points.Add(currentPoint);

        frame.lr.positionCount = points.Count;
        frame.lr.SetPositions(points.ToArray());

        // frame = new();

        // LineRenderer frameLine = frame.AddComponent<LineRenderer>();
        // TODO: fix this resource path
        // frameLine.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        // frameLine.useWorldSpace = false;
        // frameLine.startWidth = lineWidth;
        
        // frameLine.SetPositions(points.ToArray());   
    }
}
