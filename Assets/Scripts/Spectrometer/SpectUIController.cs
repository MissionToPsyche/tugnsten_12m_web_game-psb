using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectUIController : UIController
{
    public override void ResetUI()
    {
        Debug.Log("Reset UI.");
        return;
    }

    public override void ShowScore(int score)
    {
        throw new System.NotImplementedException();
    }
}
