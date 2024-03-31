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
        // Gets true and false elements from generator
        (SortedDictionary<string, Element> trueElements, SortedDictionary<string, Element> falseElements) = generator.GetData();
        
        // Gives UI all elements
        ui.InitializeGraphs(trueElements, falseElements);

        ui.submitButton.onClick.AddListener(FinishGame);

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
        throw new System.NotImplementedException();
    }
}
