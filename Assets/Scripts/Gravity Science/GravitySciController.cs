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

    public override void InitializeGame()
    {
        orbit.distortions = generator.GetDistortions(orbit.numOrbitPoints);
        ui.CreateSliders(orbit.distortions, orbit.orbitLine, orbit.transform.position);
        
        List<float> referenceWavelengths = new();
        foreach (Distortion distortion in orbit.distortions)
        {
            referenceWavelengths.Add(distortion.trueIntensity);
        }
        ui.referenceWavelengths = referenceWavelengths;

        ui.submitButton.onClick.AddListener(FinishGame);

        // TODO: start timer

        score = -1;

        ui.ResetUI();
        StartGame();
    }

    public override void StartGame()
    {
        gameRunning = true;
    }

    void Update()
    {
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
}
