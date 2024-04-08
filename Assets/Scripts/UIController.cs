using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

abstract public class UIController : MonoBehaviour
{
    public GameObject continueButton;
    // TODO: Menu button
    // TODO: Help button
    // TODO: Minigame name text
    // TODO: Timer display
    public GameScreenUI screenUI;
    private bool isSubmitted = false;
    private string text;

    public GameController controller;
    public void SetController(GameController gameController)
    {
        controller = gameController;
    }

    public void setIsSubmitted(bool sub)
    {
        isSubmitted = sub;
    }

    public void RightBtnListener()
    {
        if(!isSubmitted)
        {
            SubmitHandler();
        }
        else{
            ContinueHandler();
        }
    }

    private void SubmitHandler()
    {
        isSubmitted = true;
        SubmitClicked();
    }

    abstract public void SubmitClicked();

    private void ContinueHandler()
    {
        screenUI.continueButtonClicked();
    }

    public void ShowTime(float time)
    {
        // TODO: update time display
        TimeSpan formatTime = TimeSpan.FromSeconds(time);
        text = formatTime.Minutes.ToString("D2") + ":" + formatTime.Seconds.ToString("D2") + ":" + (formatTime.Milliseconds/100).ToString("D1");
        screenUI.setTimerText(text);
    }

    public void ShowInformation()
    {
        // TODO: show context and tutorial window, called by controller on scene change
    }

    public void ShowMenu()
    {
        // TODO: show settings menu
    }
    
    abstract public void ShowScore(int score, string grade);

    abstract public void ResetUI();
}