using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private bool timerActive = false;
    private float currentTime = 0f;

    public void setTime(float time)
    {
        currentTime = time;
    }
    public float getTime()
    {
        return currentTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void startTimer()
    {
        timerActive = true;
    }
    
    public void stopTimer()
    {
        timerActive = false;
    }

    public void resetTimer()
    {
        currentTime = 0f;
    }
}
