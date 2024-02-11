using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UIController : MonoBehaviour
{
    public GameObject continueButton;
    // TODO: Menu button

    private void Start()
    {
        ResetUI();
    }

    // Used to announce win/fail/etc.
    abstract public void ShowMsg(string msg);

    abstract public void EnterFailState();
    abstract public void EnterWinState();
    abstract public void ResetUI();
}
