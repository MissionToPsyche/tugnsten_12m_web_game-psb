using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrbitUIController : UIController
{
    public TextMeshProUGUI headerText;
    public Orbit targetOrbit;

    public bool orbitReached = false;

    public void ShowMsg(string msg)
    {
        headerText.text = msg;
    }

    override public void ResetUI()
    {
        ShowMsg("");
    }

    public override void ShowScore(int score, string grade)
    {
        screenUI.showScorePanel(score, grade);
    }

    public void SetTargetOrbit(float periapsisDistance, float apoapsisDistance, float rotation)
    {
        targetOrbit.periapsisDistance = periapsisDistance;
        targetOrbit.apoapsisDistance = apoapsisDistance;
        targetOrbit.rotation = rotation;
        targetOrbit.CalcOrbitFixed();
        targetOrbit.DrawOrbit();
    }

    override public void SubmitClicked()
    {
        controller.FinishGame();
        
        if (orbitReached)
        {
            screenUI.getContinueButton().text = "Continue";
        }
        else
        {
            screenUI.continueButtonClicked();
        }
    }

    public void ShowFuel(float fuel)
    {
        string text = "Fuel Used: " + (fuel * 10).ToString("F1");
        screenUI.setTimerText(text);
    }
}
