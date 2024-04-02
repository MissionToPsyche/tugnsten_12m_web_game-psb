using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagerUIController : UIController
{

    override public void SubmitClicked()
    {
        screenUI.getContinueButton().SetEnabled(true);
    }

    override public void ShowScore(int score, string grade)
    {

    }

    override public void ResetUI()
    {

    }
}
