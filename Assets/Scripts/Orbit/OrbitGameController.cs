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

    // Tracks whether the game has been won
    public bool won = false;

    public int missionOrbit = 0;

    override public void InitializeGame()
    {
        ui.SetController(this);
        ui.setIsSubmitted(false);
        SetRightBtn();

        ui.screenUI.getResetButton().clicked -= initializeGameAction;
        ui.screenUI.getResetButton().clicked += initializeGameAction;
        ui.screenUI.getOptionsButton().clicked -= stopGameAction;
        ui.screenUI.getOptionsButton().clicked += stopGameAction;
        ui.screenUI.getOptionsCloseButton().clicked -= startGameAction; 
        ui.screenUI.getOptionsCloseButton().clicked += startGameAction;
        ui.screenUI.getInfoButton().clicked -= stopGameAction; 
        ui.screenUI.getInfoButton().clicked += stopGameAction; 
        ui.screenUI.getInfoCloseButton().clicked -= startGameAction; 
        ui.screenUI.getInfoCloseButton().clicked += startGameAction; 

        // If mission orbit index is invalid, set it to -1 (random).
        if (missionOrbit < -1 || missionOrbit > 3)
        {
            missionOrbit = -1;
        }

        (Vector2 position, Vector2 velocity) = generator.GetInitialState(missionOrbit);
        spacecraft.ResetSpacecraft(position, velocity);

        (float periapsisDistance, float apoapsisDistance, float rotation) = generator.GetTargetOrbit(missionOrbit);
        ui.SetTargetOrbit(periapsisDistance, apoapsisDistance, rotation);

        won = false;
        winTimer = 0f;
        score = -1;

        ui.ResetUI();
        StopGame();
    }

    public override void StartGame()
    {
        gameRunning = true;
        spacecraft.controllable = true;
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
        spacecraft.controllable = false;
        spacecraft.active = false;
    }

    override public void FinishGame()
    {
        StopGame();
        ui.screenUI.getOptionsCloseButton().clicked -= startGameAction;  
        ui.screenUI.getInfoCloseButton().clicked -= startGameAction; 

        if (won)
        {
            ui.EnterWinState();
        }
        else
        {
            ui.EnterFailState();
        }

        ui.ShowScore(GetScore(), GetGrade());
        
        // Puts the random orbit score into index 0.
        if (missionOrbit > 0)
        {
            scorecard.OrbitScore[missionOrbit] = score;
        }
        else
        {
            scorecard.OrbitScore[0] = score;
        }

        ui.setIsSubmitted(true);
        ui.screenUI.getContinueButton().SetEnabled(true);
    }

    override public void CalcScore()
    {
        // Must sum to 1
        const float fuelWeight = 0.30f;
        const float rotationWeight = 0.30f;
        const float altWeight = 0.40f;

        /* ------------------------------ Fuel ------------------------------ */

        const float bestFuelUse = 7;
        const float worstFuelUse = 20;
        const float fuelRange = worstFuelUse - bestFuelUse;

        float fuelUsed = Mathf.Max(bestFuelUse, spacecraft.fuelUsed);;

        float fuelEfficiency = 1 - Mathf.Clamp01((fuelUsed - bestFuelUse) / fuelRange);

        /* ---------------------------- Rotation ---------------------------- */
        
        const float bestRotationDelta = 0.5f; // Degrees
        const float worstRotationDelta = 7;
        const float rotationDeltaRange = worstRotationDelta - bestRotationDelta;

        float rotationDelta = Mathf.Abs(ui.targetOrbit.rotation - spacecraft.orbit.rotation);
        rotationDelta = Mathf.Max(bestRotationDelta, rotationDelta);

        float rotationPrecision = 1 - Mathf.Clamp01((rotationDelta - bestRotationDelta) / rotationDeltaRange);

        /* ---------------------------- Altitude ---------------------------- */

        const float bestAltDelta = 0.05f;
        const float worstAltDelta = 0.3f;
        const float altRange = worstAltDelta - bestAltDelta;

        float periapsisDelta = Mathf.Abs(ui.targetOrbit.periapsisDistance - spacecraft.orbit.periapsisDistance);
        periapsisDelta = Mathf.Max(bestAltDelta, periapsisDelta);
        float apoapsisDelta = Mathf.Abs(ui.targetOrbit.apoapsisDistance - spacecraft.orbit.apoapsisDistance);
        apoapsisDelta = Mathf.Max(bestAltDelta, apoapsisDelta);

        float altitudeDelta = periapsisDelta * 0.5f + apoapsisDelta * 0.5f;

        float altPrecision = 1 - Mathf.Clamp01((altitudeDelta - bestAltDelta) / altRange);

        /* ------------------------------------------------------------------ */

        float overallPercent = fuelEfficiency * fuelWeight + rotationPrecision * rotationWeight + altPrecision * altWeight;

        score = Mathf.RoundToInt(maxScore * overallPercent);
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
        ui.screenUI.getContinueButton().clicked -= ui.rightBtnListenerAction; // Prevents multiple listeners
        ui.screenUI.getContinueButton().clicked += ui.rightBtnListenerAction;
    }
}
