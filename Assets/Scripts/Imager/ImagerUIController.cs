using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagerUIController : UIController
{

    override public void SubmitClicked()
    {
        return;
    }

    override public void ShowScore(int score, string grade)
    {
        Debug.Log("Grade: " + grade + " (" + score + ")");
    }

    override public void ResetUI()
    {

    }
}
