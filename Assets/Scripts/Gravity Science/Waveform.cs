using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RectTransform))]
public class Waveform : MonoBehaviour
{
    public LineRenderer lr;
    public RectTransform rt;

    public const int lowerWavelength = 100;
    public const int upperWavelength = 500;

    [Range(lowerWavelength, upperWavelength)]
    public float wavelength = lowerWavelength;

    // Pixel length of line
    public float length = 775f;

    public float amplitude = 100f;
    public float lineWidth = 0.1f;

    // Distance between calculated points
    public int graphStep = 2;
    private Vector3[] graphPoints;

    public float animationRate = 8f;
    // Amount the sin wave is shifted along the x-axis, which is used to make it
    // appear to move.
    private float phase = 0f;

    void Reset()
    {
        lr = GetComponent<LineRenderer>();
        rt = GetComponent<RectTransform>();

        // TODO: fix this resource path
        lr.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.useWorldSpace = false;
        lr.startWidth = lineWidth;

        // Moves the graph left half its length to center it in the parent transform.
        Vector2 newTransform = new(-length / 2, 0);
        rt.anchoredPosition = newTransform;
    }

    void Start()
    {
        phase = 0f;
    }

    public void SetWavelength(float sliderValue)
    {
        // Maps (-1, 1) to (0, 1);
        float percent = Mathf.Lerp(0, 1, Mathf.InverseLerp(-1, 1, sliderValue));

        wavelength = percent * (upperWavelength - lowerWavelength) + lowerWavelength;
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
            float y = amplitude * Mathf.Sin(x / wavelength * 2 * Mathf.PI + phase);
            points[i] = new Vector3(x, y, 0f);

            x += graphStep;
        }

        graphPoints = points;
    }

    public Color CalculateColor()
    {
        // Remaps the true wavelength onto more or less the visible spectrum.
        // Most of the red wavelengths are omitted to make the apparent spectrum
        // symmetrical. 
        float colorWavelength = Mathf.Lerp(380, 645, Mathf.InverseLerp(lowerWavelength, upperWavelength, wavelength));

        // Loosely based on logic from https://www.physics.sfasu.edu/astro/color/spectra.html

        float r, g, b;

        if (colorWavelength >= 380 && colorWavelength < 440)
        {
            r = -(colorWavelength - 440) / (440 - 380);
            g = 0.0f;
            b = 1.0f;
        }
        else if (colorWavelength >= 440 && colorWavelength < 490)
        {
            r = 0.0f;
            g = (colorWavelength - 440) / (490 - 440);
            b = 1.0f;
        }
        else if (colorWavelength >= 490 && colorWavelength < 510)
        {
            r = 0.0f;
            g = 1.0f;
            b = -(colorWavelength - 510) / (510 - 490);
        }
        else if (colorWavelength >= 510 && colorWavelength < 580)
        {
            r = (colorWavelength - 510) / (580 - 510);
            g = 1.0f;
            b = 0.0f;
        }
        else if (colorWavelength >= 580 && colorWavelength < 645)
        {
            r = 1.0f;
            g = -(colorWavelength - 645) / (645 - 580);
            b = 0.0f;
        }
        else if (colorWavelength >= 645 && colorWavelength <= 780)
        {
            r = 1.0f;
            g = 0.0f;
            b = 0.0f;
        }
        else
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b));
    }

    public void DrawGraph()
    {
        CalculatePoints();
        Color color = CalculateColor();

        lr.startColor = color;
        lr.endColor = color;

        lr.positionCount = graphPoints.Length;
        lr.SetPositions(graphPoints);

        AnimatePhase();
    }

    public void AnimatePhase()
    {
        // Quick way to slow the animation as wavelength grows. Without this
        // adjustment, small wavelengths appear slow and long wavelengths appear
        // fast, which communicates their relative energies opposite to reality. 
        float phaseOffset = 100f / wavelength;

        // Changes the phase over time to animate the wave
        phase += animationRate * phaseOffset * Time.deltaTime;

        // Resets phase invisibly after the graph reaches its start position to
        // prevent overflows.
        phase %= Mathf.PI * 2;
    }
}
