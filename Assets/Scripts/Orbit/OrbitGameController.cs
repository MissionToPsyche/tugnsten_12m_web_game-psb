using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGameController : GameController
{
    public OrbitDataGenerator generator;
    public OrbitUIController ui;

    public Orbiter spacecraft;

    public const float altitudeTolerance = 0.1f;
    public const float rotationTolerance = 4f;
    public const float winTimeRequired = 3f;
    private float winTimer = 0f;
    // TODO: tune this
    public float idealFuelUsage = 0.5f; // fuel use value for maximum possible score

    // Tracks whether the game has been won
    public bool won = false;

    public int missionOrbit = 0;

    override public void InitializeGame()
    {
        ui.SetController(this);
        SetRightBtn();

        (Vector2 position, Vector2 velocity) = generator.GetInitialState(missionOrbit);
        spacecraft.ResetSpacecraft(position, velocity);

        (float periapsisDistance, float apoapsisDistance, float rotation) = generator.GetTargetOrbit(missionOrbit);
        ui.SetTargetOrbit(periapsisDistance, apoapsisDistance, rotation);

        won = false;
        winTimer = 0f;
        score = -1;

        ui.ResetUI();
        StartGame();
    }

    public override void StartGame()
    {
        gameRunning = true;
        spacecraft.active = true;
    }

    void Update()
    {
        ui.ShowFuel(spacecraft.fuelUsed);

        if (gameRunning)
        {
            CheckWin();
        }
    }

    public override void StopGame()
    {
        gameRunning = false;
        spacecraft.active = false;
    }

    override public void FinishGame()
    {
        StopGame();

        if (won)
        {
            ui.EnterWinState();
        }
        else
        {
            ui.EnterFailState();
        }

        ui.ShowScore(GetScore(), GetGrade());
        ui.setIsSubmitted(true);
        ui.screenUI.getContinueButton().SetEnabled(true);
    }

    override public void CalcScore()
    {
        float fuelRatio = idealFuelUsage / spacecraft.fuelUsed;
        fuelRatio = Mathf.Min(fuelRatio, 1.0f);

        int score = Mathf.RoundToInt(maxScore * fuelRatio);

        this.score = score;
    }

    public void CheckWin()
    {
        // Checks if the current orbit's rotation and apsis distances are
        // within tolerance of the target orbit.
        bool winState = Mathf.Abs(spacecraft.orbit.rotation - ui.targetOrbit.rotation) < rotationTolerance
                        && Mathf.Abs(spacecraft.orbit.periapsisDistance - ui.targetOrbit.periapsisDistance) < altitudeTolerance
                        && Mathf.Abs(spacecraft.orbit.apoapsisDistance - ui.targetOrbit.apoapsisDistance) < altitudeTolerance;

        // If the game is in the win state, starts counting up to
        // winTimeRequired. If the game leaves the win state, the timer is
        // reset.
        if (winState)
        {
            winTimer += Time.deltaTime;

            float secondsRemaining = Mathf.Round(winTimeRequired - winTimer);
            ui.ShowMsg("Maintain Orbit..." + secondsRemaining);
        }
        else
        {
            winTimer = 0f;
            ui.ShowMsg("");
        }

        if (spacecraft.orbit.hasCrashed)
        {
            ui.ShowMsg("Spacecraft Deorbited!");
            FinishGame();
        }
        else if (spacecraft.orbit.hasEscaped)
        {
            ui.ShowMsg("Spacecraft Escaped Orbit!");
            FinishGame();
        }

        // If the game has remained in the win state for winTimeRequired,
        // the player has actually won.
        if (winTimer >= winTimeRequired)
        {
            won = true;
            FinishGame();
        }
    }

    override public void SetRightBtn()
    {
        ui.screenUI.getContinueButton().text = "Continue";
        ui.screenUI.getContinueButton().SetEnabled(false);
        ui.screenUI.getContinueButton().clicked -= ui.RightBtnListener; // Prevents multiple listeners
        ui.screenUI.getContinueButton().clicked += ui.RightBtnListener;
    }
}
