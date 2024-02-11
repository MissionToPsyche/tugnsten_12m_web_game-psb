using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GameController : MonoBehaviour
{
    protected bool gameRunning = false;
    public UIController ui;

    public int maxScore = 10000;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (gameRunning)
        {
            if (CheckWin()) {
                ui.EnterWinState();
            };
        }
        else
        {
            FinishGame();
        }
    }

    abstract public void InitializeGame();
    abstract public void FinishGame();
    abstract public int GetScore();
    abstract public bool CheckWin();
}
