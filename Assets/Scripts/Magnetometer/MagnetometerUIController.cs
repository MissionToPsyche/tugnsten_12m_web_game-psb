using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetometerUIController : UIController
{
    [SerializeField] private GameObject noFieldMsg;

    override public void ShowScore(int score, string grade)
    {
        screenUI.ShowScorePanel(score, grade);
    }

    override public void SubmitClicked()
    {
        screenUI.GetContinueButton().text = "Continue";
        controller.FinishGame();
    }

    override public void ResetUI()
    {
        HideNoFieldMsg();
    }

    public void ShowNoFieldMsg()
    {
        noFieldMsg.SetActive(true);
    }
    public void HideNoFieldMsg()
    {
        noFieldMsg.SetActive(false);
    }
}
