using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class GameController : MonoBehaviour
{
    public GameTimer timer;
    public Scorecard scorecard;
    protected bool gameRunning = false;
    public int maxScore = 10000;
    public int score = -1;
    protected Action startGameAction;
    protected Action stopGameAction;
    protected Action initializeGameAction;

    public GameController()
    {
       startGameAction = () => { StartGame(); };
       stopGameAction = () => { StopGame(); };
       initializeGameAction = () => { InitializeGame(); StartGame(); };
    }

    void Start()
    {
        timer = GameObject.Find("GameTimer").GetComponent<GameTimer>();
        InitializeGame();
    }

    abstract public void InitializeGame();
    abstract public void StartGame();
    abstract public void StopGame();
    abstract public void FinishGame();
    abstract public void CalcScore();

    abstract public void SetRightBtn();

    public int GetScore()
    {
        if (score < 0)
        {
            CalcScore();
        }

        return score;
    }

    public string GetGrade()
    {
        if (score < 0)
        {
            CalcScore();
        }

        if (score >= 9600)
            return "A+";
        else if (score >= 9200)
            return "A";
        else if (score >= 8800)
            return "A-";
        else if (score >= 8400)
            return "B+";
        else if (score >= 8000)
            return "B";
        else if (score >= 7600)
            return "B-";
        else if (score >= 7200)
            return "C+";
        else if (score >= 6800)
            return "C";
        else if (score >= 6200)
            return "C-";
        else if (score >= 5500)
            return "D+";
        else if (score >= 4000)
            return "D";
        else if (score >= 2500)
            return "D-";
        else
            return "F";
    }

    public float CalcTimePercent(float time, float worstTime, float bestTime)
    {
        time = Mathf.Max(time, bestTime);
        
        float timeRange = worstTime - bestTime;

        return Mathf.Clamp01((time - bestTime) / timeRange);
    }
}
