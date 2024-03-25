using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GameController : MonoBehaviour
{
    public GameTimer timer;
    protected bool gameRunning = false;

    public int maxScore = 10000;
    public int score = -1;

    void Start()
    {
        InitializeGame();
    }

    abstract public void InitializeGame();
    abstract public void StartGame();
    abstract public void StopGame();
    abstract public void FinishGame();
    abstract public void CalcScore();
    
    public int GetScore()
    {
        if (score < 0)
        {
            CalcScore();
        }

        return score;
    }
    
    public char GetGrade()
    {
        if (score < 0)
        {
            CalcScore();
        }

        // TODO: grade logic
        // TODO: enum for letters

        return 'F';
    }
}
