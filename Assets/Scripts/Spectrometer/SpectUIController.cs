using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpectUIController : UIController
{
    public SpectrumGraph referenceGraph;
    public SpectrumGraph userGraph;
    public SpectrumGraph[] controls = new SpectrumGraph[4];
    public SortedDictionary<string, SpectrumGraph> controlsDict;

    public Button submitButton;

    public SortedDictionary<string, Element> allElements;

    public override void ResetUI()
    {
        // Sets the element quantities in the user graph to 0
        foreach (Element element in userGraph.elements.Values)
        {
            element.quantity = 0f;
        }

        // Initializes control graphs
        controlsDict = new();
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
    }

    public void InitializeGraphs(SortedDictionary<string, Element> trueElements, SortedDictionary<string, Element> falseElements)
    {
        // This gross code seems to be the best way to merge two dicts
        SortedDictionary<string, Element>[] dictionaries = { trueElements, falseElements };
        allElements = new(dictionaries.SelectMany(d => d).ToDictionary(p => p.Key, p => p.Value.Clone()));

        // Deep copies the true elements to the reference graph
        referenceGraph.elements = new(trueElements.ToDictionary(p => p.Key, p => p.Value.Clone()));

        // Deep copies the true elements to the reference graph
        userGraph.elements = new(allElements.ToDictionary(p => p.Key, p => p.Value.Clone()));  
    }

    public void UpdateUserGraph()
    {
        foreach (Element element in allElements.Values)
        {
            float sliderValue = controlsDict[element.name].slider.value;
            userGraph.elements[element.name].quantity = sliderValue;
        }
    }

    public override void ShowScore(int score, string grade)
    {
        Debug.Log("Grade: " + grade + " (" + score + ")");
    }
}
