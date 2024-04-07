using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravitySciController : GameController
{
    public GravitySciUIController ui;
    public GravitySciGenerator generator;
    public DistortedOrbit orbit;
    public RailsSpacecraft spacecraft;

    // Difference between user and reference distortions treated as perfect.
    public float idealDiff = 0.05f;
    // Time treated as perfect.
    public float idealTime = 5f;

    [Range(0, 1)]
    public float accuracyWeight = 0.5f;
    private float timeWeight;

    public override void InitializeGame()
    {
        ui.SetController(this);
        SetRightBtn();

        orbit.distortions = generator.GetDistortions(orbit.numOrbitPoints);
        ui.CreateSliders(orbit.distortions, orbit.undistortedOrbitLine, orbit.transform.position);

        List<float> referenceWavelengths = new();
        foreach (Distortion distortion in orbit.distortions)
        {
            referenceWavelengths.Add(distortion.trueIntensity);
        }
        ui.referenceWavelengths = referenceWavelengths;

        // Prevents multiple listeners being added on reset. 
        if(score < 0)
        {
            ui.submitButton.onClick.AddListener(FinishGame);
        }

        score = -1;
        
        timer.resetTimer();
        ui.ResetUI();
        StartGame();
    }

    public override void StartGame()
    {
        gameRunning = true;
        timer.startTimer();
    }

    void Update()
    {
        ui.ShowTime(timer.getTime());
        if (gameRunning)
        {
            ui.UpdateGraphs();
            List<float> userDistortionIntensities = ui.GetSliderValues();

            for (int i = 0; i < userDistortionIntensities.Count; i++)
            {
                orbit.distortions[i].intensity = userDistortionIntensities[i];
            }

            ui.AnimateGraphs();

            orbit.ApplyDistortions();

            spacecraft.UpdatePosition();
        }
    }

    public override void StopGame()
    {
        gameRunning = false;
        timer.stopTimer();
    }

    public override void FinishGame()
    {
        StopGame();
        ui.ShowScore(GetScore(), GetGrade());
    }

    public override void CalcScore()
    {
        // Scores each distortion individually, then averages. 

        List<int> scores = new();

        foreach (Distortion distortion in orbit.distortions)
        {
            float distortionDiff = Mathf.Abs(distortion.intensity - distortion.trueIntensity);
            float diffRatio = idealDiff / distortionDiff;
            diffRatio = Mathf.Min(diffRatio, 1.0f);

            scores.Add(Mathf.RoundToInt(diffRatio * maxScore));
        }

        score = Mathf.RoundToInt((float)scores.Average());
    }

    override public void SetRightBtn()
    {
        ui.screenUI.getContinueButton().text = "Submit";
        ui.screenUI.getContinueButton().clicked -= ui.RightBtnListener; // Prevents multiple listeners
        ui.screenUI.getContinueButton().clicked += ui.RightBtnListener;
    }
}
