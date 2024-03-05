using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectUIController : UIController
{
    public override void ShowMsg(string msg)
    {
        throw new System.NotImplementedException();
    }

    public override void ResetUI()
    {
        Debug.Log("Reset UI.");
        return;
    }

    public override void EnterFailState()
    {
        Debug.Log("UI entered fail state.");
        return;
    }

    public override void EnterWinState()
    {
        Debug.Log("UI entered win state.");
        return;
    }
}
