using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UIController : MonoBehaviour
{
    public GameObject continueButton;
    // TODO: Menu button
    // TODO: Help button
    // TODO: Minigame name text
    // TODO: Timer display

    private void Start()
    {
        ResetUI();
    }

    public void ShowTime(float time)
    {
        // TODO: update time display
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