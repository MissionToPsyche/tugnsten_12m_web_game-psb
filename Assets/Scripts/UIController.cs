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

    // temporary
    // [SerializeField] private TMP_Text text;
    private string text;

    public void ShowTime(float time, GameScreenUI screenUI)
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