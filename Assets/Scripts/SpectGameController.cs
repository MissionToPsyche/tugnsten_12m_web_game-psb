using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectGameController : GameController
{
    public SpectDataGenerator generator;
    public SpectrumGraph referenceGraph;
    public SpectrumGraph userGraph;
    // private SpectrumGraph[] controls;

    public override void InitializeGame()
    {
        (Dictionary<string, Element> trueElements, Dictionary<string, Element> falseElements) = generator.GetData();
        
        // This gross code seems to be the best way to merge two dicts
        Dictionary<string, Element>[] dictionaries = { trueElements, falseElements };
        Dictionary<string, Element> allElements = dictionaries.SelectMany(d => d).ToDictionary(p => p.Key, p => p.Value.Clone());

        // Deep copy
        referenceGraph.elements = trueElements.ToDictionary(p => p.Key, p => p.Value.Clone());

        // Deep copy
        userGraph.elements = allElements.ToDictionary(p => p.Key, p => p.Value.Clone());

        // Sets the element quantities in the user graph to 0
        foreach (Element element in userGraph.elements.Values)
        {
            element.quantity = 0f;
        }

        // draw control graphs

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
