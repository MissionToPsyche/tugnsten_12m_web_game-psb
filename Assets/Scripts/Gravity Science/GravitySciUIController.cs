using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GravitySciUIController : UIController
{
    public Waveform referenceWave;
    public Waveform userWave;
    public List<float> referenceWavelengths;

    public Slider preciseSlider;

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
            sliders[i].gameObject.name = "Slider " + i;
            sliders[i].gameObject.tag = "destroyOnReset";

            // Adds an onClick event to change the active slider
            EventTrigger evTrig = sliders[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry clickEvent = new()
            {
                eventID = EventTriggerType.PointerDown
            };

            int sliderIndex = i; // This is necessary for the closure to capture i's value

            // Changes the active slider and syncs the precise slider to it
            // whenever one of the normal ones is clicked
            clickEvent.callback.AddListener((_) =>
            {
                SetActiveSlider(sliderIndex);
                SyncSliders(false);
            });

            evTrig.triggers.Add(clickEvent);

            // Updates the precise slider whenever one of the normal ones is changed
            sliders[i].onValueChanged.AddListener((_) =>
            {
                SyncSliders(false);
            });

            // Positions slider at center of the distortion, rotated so that top
            // is facing radial direction. The orbit's position is added to
            // align them to wherever the orbit is in the scene.
            sliders[i].transform.SetPositionAndRotation(
                orbitLine[distortions[i].centerPoint] + orbitPosition,
                Quaternion.AngleAxis(angle, Vector3.forward)
            );
        }

        // Allows the precise slider to preempt whichever normal slider is active. 
        preciseSlider.onValueChanged.AddListener((_) =>
        {
            SyncSliders(true);
        });
    }

    public void SetActiveSlider(int activeSlider)
    {
        this.activeSlider = activeSlider;
        // Changes the reference wave to the one for this slider's distortion
        referenceWave.SetWavelength(referenceWavelengths[this.activeSlider]);

        // TODO: Highlight active slider
        // for (int i = 0; i < sliders.Count; i++)
        // {
        //     ColorBlock colors = sliders[i].colors;
            
        //     if (i == this.activeSlider)
        //     {
        //         colors.normalColor = new Color(162, 55, 212);
        //         colors.selectedColor = new Color(162, 55, 212);
        //         colors.pressedColor = new Color(210, 126, 250);
        //     }
        //     else
        //     {
        //         colors.normalColor = Color.white;
        //         colors.selectedColor = new Color(245, 245, 245);
        //         colors.pressedColor = new Color(200, 200, 200);
        //     }

        //     sliders[i].colors = colors;
        // }
    }

    public void SyncSliders(bool preciseChanged)
    {
        // Changes the sync direction
        if (preciseChanged)
        {
            sliders[activeSlider].value = preciseSlider.value;
        }
        else
        {
            preciseSlider.value = sliders[activeSlider].value;
        }
    }

    public void UpdateGraphs()
    {
        userWave.SetWavelength(sliders[activeSlider].value);
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

        preciseSlider.value = 0;

        userWave.SetWavelength(0);
    }

    public override void ShowScore(int score, string grade)
    {
        screenUI.showScorePanel(score, grade);
    }

    override public void SubmitClicked()
    {
        screenUI.getContinueButton().text = "Continue";
        controller.FinishGame();
    }
}
