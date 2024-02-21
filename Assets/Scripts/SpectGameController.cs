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
    private SortedDictionary<string, SpectrumGraph> controlsDict = new();
    private SortedDictionary<string, Element> allElements;

    public override void InitializeGame()
    {
        (SortedDictionary<string, Element> trueElements, SortedDictionary<string, Element> falseElements) = generator.GetData();
        
        // This gross code seems to be the best way to merge two dicts
        SortedDictionary<string, Element>[] dictionaries = { trueElements, falseElements };
        allElements = new(dictionaries.SelectMany(d => d).ToDictionary(p => p.Key, p => p.Value.Clone()));

        // Deep copy
        referenceGraph.elements = new(trueElements.ToDictionary(p => p.Key, p => p.Value.Clone()));

        // Deep copy
        userGraph.elements = new(allElements.ToDictionary(p => p.Key, p => p.Value.Clone()));

        // Sets the element quantities in the user graph to 0
        foreach (Element element in userGraph.elements.Values)
        {
            element.quantity = 0f;
        }
        
        // Initializes control graphs
        for (int i = 0; i < controls.Length; i++)
        {   
            controls[i].elements = new()
            {
                { allElements.ElementAt(i).Key, allElements.ElementAt(i).Value.Clone() }
            };

            controls[i].elements.ElementAt(0).Value.quantity = 1f;
            
            // Converts the array of control graphs into a dict with element
            // names as keys.
            controlsDict.Add(controls[i].elements.ElementAt(0).Value.name, controls[i]);
        }

        // TODO: start timer

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

    // Called by super's Update()
    public override bool CheckWin()
    {
        UpdateUserGraph();
        return false;
    }

    public void UpdateUserGraph()
    {
        foreach (Element element in allElements.Values)
        {
            // controlsDict[element.name]
        }
    }
}
