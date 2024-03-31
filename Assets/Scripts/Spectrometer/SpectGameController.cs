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

    public override void InitializeGame()
    {
        // Gets true and false elements from generator
        SortedDictionary<string, Element> selectedElements = generator.GetData();

        // Gives UI all elements
        ui.InitializeGraphs(selectedElements);

        // Prevents multiple listeners being added on reset. 
        if(score < 0)
        {
            ui.submitButton.onClick.AddListener(FinishGame);
        }

        // TODO: start timer

        score = -1;

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
    }

    public override void StopGame()
    {
        gameRunning = false;
    }

    public override void FinishGame()
    {
        StopGame();
        ui.ShowScore(GetScore(), GetGrade());
    }

    public override void CalcScore()
    {
        // Scores each element individually, then averages. 

        List<int> scores = new();

        foreach (Element element in ui.userGraph.elements.Values)
        {
            float quantityDiff = Mathf.Abs(element.quantity - ui.referenceGraph.elements[element.name].quantity);
            float diffRatio = idealDiff / quantityDiff;
            diffRatio = Mathf.Min(diffRatio, 1.0f);

            scores.Add(Mathf.RoundToInt(diffRatio * maxScore));
        }

        score = Mathf.RoundToInt((float)scores.Average());
    }
}
