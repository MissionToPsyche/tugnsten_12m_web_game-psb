using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrbitUIController : UIController
{
    public TextMeshProUGUI headerText;
    // public GameObject skipButton;
    // public GameObject restartButton;
    public Orbit targetOrbit;

    public void ShowMsg(string msg)
    {
        headerText.text = msg;
    }

    override public void ResetUI()
    {
        ShowMsg("");
    }

    public void EnterFailState()
    {
        // restartButton.SetActive(true);
    }

    public void EnterWinState()
    {
        ShowMsg("Orbit Reached");
        // restartButton.SetActive(true);
        // continueButton.SetActive(true);
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
        screenUI.getContinueButton().text = "Continue";
        controller.FinishGame();
    }

    public void ShowFuel(float fuel)
    {
        string text = "Fuel Used: " + (fuel * 10).ToString("F1");
        screenUI.setTimerText(text);
    }
}
