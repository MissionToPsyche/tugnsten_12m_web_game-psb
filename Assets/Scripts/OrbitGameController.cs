using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGameController : GameController
{
    public Orbiter spacecraft;
    public Orbit targetOrbit;
    public OrbitDataGenerator generator;

    public const float altitudeTolerance = 0.1f;
    public const float rotationTolerance = 4f;
    public const float winTimeRequired = 3f;
    private float winTimer = 0f;
    // TODO: tune this
    public float idealFuelUsage = 0.5f; // fuel use value for maximum possible score

    override public void InitializeGame()
    {
        ui.ResetUI();
        spacecraft.ResetSpacecraft();

        (spacecraft.transform.position, spacecraft.initialVelocity) = generator.GetInitialState();

        winTimer = 0f;

        gameRunning = true;
    }

    override public void FinishGame()
    {
        spacecraft.active = false;
    }

    override public int GetScore()
    {
        float fuelRatio = idealFuelUsage / spacecraft.fuelUsed;

        fuelRatio = Mathf.Max(fuelRatio, 1.0f);

        int score = Mathf.RoundToInt(maxScore * fuelRatio);

        return score;
    }

    override public bool CheckWin()
    {
        // Checks if the current orbit's rotation and apsis distances are
        // within tolerance of the target orbit.
        bool winState = Mathf.Abs(spacecraft.orbit.rotation - targetOrbit.rotation) < rotationTolerance
                        && Mathf.Abs(spacecraft.orbit.periapsisDistance - targetOrbit.periapsisDistance) < altitudeTolerance
                        && Mathf.Abs(spacecraft.orbit.apoapsisDistance - targetOrbit.apoapsisDistance) < altitudeTolerance;

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
            gameRunning = false;
            ui.ShowMsg("Spacecraft Deorbited!");
            ui.EnterFailState();
        }
        else if (spacecraft.orbit.hasEscaped)
        {
            gameRunning = false;
            ui.ShowMsg("Spacecraft Escaped Orbit!");
            ui.EnterFailState();
        }

        // If the game has remained in the win state for winTimeRequired,
        // the player has actually won.
        if (winTimer >= winTimeRequired)
        {
            gameRunning = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetTargetOrbit(float periapsisDistance, float apoapsisDistance, float rotation)
    {
        targetOrbit.periapsisDistance = periapsisDistance;
        targetOrbit.apoapsisDistance = apoapsisDistance;
        targetOrbit.rotation = rotation;
    }
}
