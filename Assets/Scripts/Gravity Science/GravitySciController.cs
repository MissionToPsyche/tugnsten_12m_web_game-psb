using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySciController : GameController
{
    public GravitySciUIController ui;
    public GravitySciGenerator generator;
    public DistortedOrbit orbit;
    public RailsSpacecraft spacecraft;

    public override void InitializeGame()
    {
        orbit.distortions = generator.GetDistortions(orbit.numOrbitPoints);
        ui.CreateSliders(orbit.distortions, orbit.orbitLine, orbit.transform.position);
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
            ui.UpdateUserGraph();
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
        gameRunning = false;
        CalcScore();
    }

    public override void CalcScore()
    {
        throw new System.NotImplementedException();
    }
}
