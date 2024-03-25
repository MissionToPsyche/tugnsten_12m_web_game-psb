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
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        ResetUI();
    }

    public void ShowTime(float time)
    {
        // TODO: update time display
        TimeSpan formatTime = TimeSpan.FromSeconds(time);
        text.text = formatTime.Minutes.ToString("D2") + ":" + formatTime.Seconds.ToString("D2") + ":" + (formatTime.Milliseconds/100).ToString("D1");
    }

    public void ShowInformation()
    {
        // TODO: show context and tutorial window, called by controller on scene change
    }

    public void ShowMenu()
    {
        // TODO: show settings menu
    }
    
    abstract public void ShowScore(int score);

    abstract public void ResetUI();
}