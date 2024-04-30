using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class UIController : MonoBehaviour
{
    public GameObject continueButton;
    public GameScreenUI screenUI;
    private bool isSubmitted = false;
    private string text;
    public Action rightBtnListenerAction;
    public GameController controller;

    public UIController()
    {
        rightBtnListenerAction = () => { RightBtnListener(); };
    }
    
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
        screenUI.ContinueButtonClicked();
    }

    public void ShowTime(float time)
    {
        // TODO: update time display
        TimeSpan formatTime = TimeSpan.FromSeconds(time);
        text = formatTime.Minutes.ToString("D2") + ":" + formatTime.Seconds.ToString("D2") + ":" + (formatTime.Milliseconds/100).ToString("D1");
        screenUI.SetTimerText(text);
    }

    abstract public void ShowScore(int score, string grade);

    abstract public void ResetUI();
}