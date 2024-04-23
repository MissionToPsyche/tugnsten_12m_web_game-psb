using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectGameController : GameController
{
    public SpectUIController ui;
    public SpectDataGenerator generator;

    public override void InitializeGame()
    {
        ui.SetController(this);
        ui.setIsSubmitted(false);
        SetRightBtn();

        ui.screenUI.GetResetButton().clicked -= initializeGameAction;
        ui.screenUI.GetResetButton().clicked += initializeGameAction;
        ui.screenUI.GetOptionsButton().clicked -= stopGameAction;
        ui.screenUI.GetOptionsButton().clicked += stopGameAction;
        ui.screenUI.getOptionsCloseButton().clicked -= startGameAction; 
        ui.screenUI.getOptionsCloseButton().clicked += startGameAction;
        ui.screenUI.getInfoButton().clicked -= stopGameAction; 
        ui.screenUI.getInfoButton().clicked += stopGameAction; 
        ui.screenUI.getInfoCloseButton().clicked -= startGameAction; 
        ui.screenUI.getInfoCloseButton().clicked += startGameAction; 

        // Gets true and false elements from generator
        SortedDictionary<string, Element> selectedElements = generator.GetData();

        // Gives UI all elements
        ui.InitializeGraphs(selectedElements);

        score = -1;

        timer.resetTimer();
        ui.ResetUI();
    }

    private void Update()
    {
        ui.ShowTime(timer.getTime());
        ui.UpdateUserGraph();
    }

    public override void StartGame()
    {
        gameRunning = true;
        timer.startTimer();
    }

    public override void StopGame()
    {
        gameRunning = false;
        timer.stopTimer();
    }

    public override void FinishGame()
    {
        StopGame();
        ui.screenUI.getOptionsCloseButton().clicked -= startGameAction;  
        ui.screenUI.getInfoCloseButton().clicked -= startGameAction; 
        ui.ShowScore(GetScore(), GetGrade());
        scorecard.SpectrometerScore = score;
    }

    public override void CalcScore()
    {
        // Must sum to 1
        const float precisionWeight = 0.8f;
        const float timeWeight = 0.2f;

        /* ---------------------------- Precision --------------------------- */

        const float bestDelta = 0.05f;
        const float worstDelta = 0.3f;
        const float deltaRange = worstDelta - bestDelta;

        // Scores each element individually, then averages.

        List<float> precisions = new();

        foreach (Element element in ui.userGraph.elements.Values)
        {
            float trueQuantity = ui.referenceGraph.elements[element.name].quantity;
            float delta = Mathf.Abs(element.quantity - trueQuantity);
            delta = Mathf.Max(delta, bestDelta);
            float deltaPercent = 1 - Mathf.Clamp01((delta - bestDelta) / deltaRange);

            // If this is not the false element and the user said it was the
            // false element, no points.
            if (trueQuantity != 0 && element.quantity == 0)
            {
                deltaPercent = 0;
            }

            precisions.Add(deltaPercent);
        }

        float precision = (float)precisions.Average();
        
        // Catches a minimum-effort submit (everything wrong but the false
        // element) and gives it an F.
        if (precision <= 0.25)
        {
            precision = 0;
        }

        /* ------------------------------ Time ------------------------------ */

        float timePercent = 1 - CalcTimePercent(timer.getTime(), 40, 10);

        /* ------------------------------------------------------------------ */

        float overallPercent = precision * precisionWeight + timePercent * timeWeight;

        score = Mathf.RoundToInt(maxScore * overallPercent);
    }

    override public void SetRightBtn()
    {
        ui.screenUI.GetContinueButton().text = "Submit";
        ui.screenUI.GetContinueButton().clicked -= ui.rightBtnListenerAction; // Prevents multiple listeners
        ui.screenUI.GetContinueButton().clicked += ui.rightBtnListenerAction;
    }
}
