using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectGameController : GameController
{
    public SpectDataGenerator generator;
    public SpectrumGraph referenceGraph;
    public SpectrumGraph userGraph;
    // private SpectrumGraph[] controls;

    public override void InitializeGame()
    {
        (List<Element> trueElements, List<Element> falseElements) = generator.GetData();

        referenceGraph.elements = trueElements;

        // draw control graphs

        // draw user graph
        
        // start timer

        gameRunning = true;
    }

    public override void FinishGame()
    {
        Debug.Log("Game finished.");
        return;
    }

    public override int GetScore()
    {
        throw new System.NotImplementedException();
    }

    public override bool CheckWin()
    {
        return false;
    }
}
