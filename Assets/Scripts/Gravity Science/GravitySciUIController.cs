using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravitySciUIController : UIController
{
    public Waveform referenceWave;
    public Waveform userWave;
    public Slider sliderPrefab;
    public List<Slider> sliders;
    private List<float> lastValues;
    public int activeSlider = 0;

    public void CreateSliders(List<Distortion> distortions, Vector3[] orbitLine, Vector3 orbitPosition)
    {
        int numSliders = distortions.Count;

        if (numSliders < 1)
        {
            Debug.LogError("GravitySciUIController passed less than 1 distortion.");
            return;
        }

        // Sets up last values to match number of distortions
        lastValues = new();
        for (int i = 0; i < numSliders; i++)
        {
            lastValues.Add(0);
        }

        // Creates and positions sliders, one per distortion
        sliders = new();
        for (int i = 0; i < numSliders; i++)
        {
            // Downcast to Vector2 for performance, since z is always 0
            Vector2 lastPoint = orbitLine[distortions[i].lastPoint];
            Vector2 firstPoint = orbitLine[distortions[i].firstPoint];

            // Direction vector pointing from the first point of the distortion to the last point
            Vector2 tangent = (lastPoint - firstPoint).normalized;

            // Vector perpendicular to tangent pointing outward
            Vector2 radial = new(-tangent.y, tangent.x);

            float angle = Mathf.Atan2(radial.y, radial.x) * Mathf.Rad2Deg;

            // `transform` makes it a child of this script's object, the main UI canvas
            sliders.Add(Instantiate(sliderPrefab, transform));

            // Positions slider at center of the distortion, rotated so that top
            // is facing radial direction. The orbit's position is added to
            // align them to wherever the orbit is in the scene.
            sliders[i].transform.SetPositionAndRotation(
                orbitLine[distortions[i].centerPoint] + orbitPosition,
                Quaternion.AngleAxis(angle, Vector3.forward)
            );
        }
    }

    public void UpdateUserGraph()
    {
        // TODO: replace this with valueChanged listeners, or onClick if that's a thing
        for (int i = 0; i < sliders.Count; i++)
        {
            if (lastValues[i] != sliders[i].value)
            {
                activeSlider = i;
            }

            lastValues[i] = sliders[i].value;
        }

        userWave.SetWavelength(sliders[activeSlider].value);

        // TODO: Highlight active slider
    }

    public List<float> GetSliderValues()
    {
        List<float> values = new();

        foreach (Slider slider in sliders)
        {
            values.Add(slider.value);
        }

        return values;
    }

    public void AnimateGraphs()
    {
        referenceWave.DrawGraph();
        userWave.DrawGraph();
    }

    public override void ResetUI()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            sliders[i].value = 0;
            lastValues[i] = 0;
        }

        activeSlider = 0;

        userWave.SetWavelength(0);
    }

    public override void ShowScore(int score)
    {
        throw new System.NotImplementedException();
    }
}
