using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectGameController : GameController
{
    public SpectDataGenerator generator;
    public SpectrumGraph referenceGraph;
    public SpectrumGraph userGraph;
    public SpectrumGraph[] controls = new SpectrumGraph[4];

    public override void InitializeGame()
    {
        (SortedDictionary<string, Element> trueElements, SortedDictionary<string, Element> falseElements) = generator.GetData();
        
        // This gross code seems to be the best way to merge two dicts
        SortedDictionary<string, Element>[] dictionaries = { trueElements, falseElements };
        SortedDictionary<string, Element> allElements = new(dictionaries.SelectMany(d => d).ToDictionary(p => p.Key, p => p.Value.Clone()));

        // Deep copy
        referenceGraph.elements = new(trueElements.ToDictionary(p => p.Key, p => p.Value.Clone()));

        // Deep copy
        userGraph.elements = new(allElements.ToDictionary(p => p.Key, p => p.Value.Clone()));

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
