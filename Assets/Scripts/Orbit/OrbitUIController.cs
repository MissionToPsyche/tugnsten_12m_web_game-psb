using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrbitUIController : UIController
{
    public TextMeshProUGUI headerText;
    public GameObject skipButton;
    public GameObject restartButton;

    public void ShowMsg(string msg)
    {
        headerText.text = msg;
    }

    override public void ResetUI()
    {
        ShowMsg("");
        restartButton.SetActive(false);
        continueButton.SetActive(false);
    }

    public void EnterFailState()
    {
        restartButton.SetActive(true);
    }

    public void EnterWinState()
    {
        ShowMsg("Orbit Reached");
        restartButton.SetActive(true);
        continueButton.SetActive(true);
    }

    public override void ShowScore(int score, string grade)
    {
        throw new System.NotImplementedException();
    }
}