using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectGameController : GameController
{
    public SpectUIController ui;
    public SpectDataGenerator generator;

    // Difference between user and reference quantities treated as perfect.
    public float idealDiff = 0.05f;
    // Time treated as perfect.
    public float idealTime = 5f;

    [Range(0, 1)]
    public float accuracyWeight = 0.5f;
    private float timeWeight;

    private void OnValidate()
    {
        timeWeight = 1 - accuracyWeight;
    }

    public override void InitializeGame()
    {
        ui.SetController(this);
        // ui.screenUI = GameObject.Find("UIDocument").GetComponent<GameScreenUI>();
        SetRightBtn();
        // Gets true and false elements from generator
        SortedDictionary<string, Element> selectedElements = generator.GetData();

        // Gives UI all elements
        ui.InitializeGraphs(selectedElements);

        // Prevents multiple listeners being added on reset. 
        if (score < 0)
        {
            ui.submitButton.onClick.AddListener(FinishGame);
        }

        score = -1;

        timer.resetTimer();
        ui.ResetUI();
        StartGame();
    }

    private void Update()
    {
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
        ui.ShowScore(GetScore(), GetGrade());
    }

    public override void CalcScore()
    {
        // Scores each element individually, then averages

        List<float> accuracyRatios = new();

        foreach (Element element in ui.userGraph.elements.Values)
        {
            float trueQuantity = ui.referenceGraph.elements[element.name].quantity;
            float quantityDiff = Mathf.Abs(element.quantity - trueQuantity);
            float diffRatio = idealDiff / quantityDiff;
            diffRatio = Mathf.Min(diffRatio, 1.0f);

            // If this is not the false element and the user said it was the
            // false element, no points.
            if (trueQuantity != 0 && element.quantity == 0)
            {
                diffRatio = 0;
            }

            accuracyRatios.Add(diffRatio);
        }
        
        // Percent of perfect due to accuracy
        float accuracyRatio = (float)accuracyRatios.Average();

        score = Mathf.RoundToInt(maxScore * accuracyRatio);
    }

    override public void SetRightBtn()
    {
        ui.screenUI.getContinueButton().text = "Submit";
        ui.screenUI.getContinueButton().clicked += ui.RightBtnListener;
    }
}
