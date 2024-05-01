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
    private SortedDictionary<string, SpectrumGraph> controlsDict;

    public SortedDictionary<string, Element> allElements;

    public override void ResetUI()
    {
        SetSlidersEnabled(true);

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
            controls[i].title.text = controls[i].elements.ElementAt(0).Value.name;

            // Converts the array of control graphs into a dict with element
            // names as keys.
            controlsDict.Add(controls[i].elements.ElementAt(0).Value.name, controls[i]);
        }

        // Zeroes the sliders
        foreach (Element element in allElements.Values)
        {
            controlsDict[element.name].slider.value = 0;
        }
    }

    public void InitializeGraphs(SortedDictionary<string, Element> elements)
    {
        // Deep copies the elements to the graphs
        referenceGraph.elements = new(elements.ToDictionary(p => p.Key, p => p.Value.Clone()));
        userGraph.elements = new(elements.ToDictionary(p => p.Key, p => p.Value.Clone()));

        allElements = elements;
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
        screenUI.ShowScorePanel(score, grade);
    }

    override public void SubmitClicked()
    {
        screenUI.GetContinueButton().text = "Continue";
        controller.FinishGame();
    }

    public void SetSlidersEnabled(bool enabled)
    {
        foreach (SpectrumGraph graph in controls)
        {
            graph.slider.enabled = enabled;
        }
    }
}
