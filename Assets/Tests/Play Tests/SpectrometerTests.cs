using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class SpectrometerTests
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Spectrometer_minigame");
    }

    public (SpectGameController, SpectUIController) GetControllers()
    {
        GameObject go = GameObject.Find("Game Controller"); 
        SpectGameController controller = go.GetComponent<SpectGameController>();
        SpectUIController ui = controller.ui;

        return (controller, ui);
    }

    [UnityTest]
    public IEnumerator ElementGeneration()
    {
        (SpectGameController controller, SpectUIController ui) = GetControllers();
        CollectionAssert.AreEquivalent(ui.referenceGraph.elements.Keys, ui.userGraph.elements.Keys);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ControlInitialization()
    {
        (SpectGameController controller, SpectUIController ui) = GetControllers();

        List<string> controlElements = new();

        foreach (SpectrumGraph graph in ui.controls)
        {
            controlElements.Add(graph.elements.Keys.ElementAt(0));
        }

        CollectionAssert.AreEquivalent(ui.userGraph.elements.Keys, controlElements);
        yield return null;
    }

    [UnityTest]
    public IEnumerator WinDetection()
    {
        (SpectGameController controller, SpectUIController ui) = GetControllers();
        
        foreach (Element element in ui.referenceGraph.elements.Values)
        {
            ui.userGraph.elements[element.name].quantity = element.quantity;
        }

        controller.CalcScore();

        Assert.AreEqual(10000, controller.score);

        yield return null;
    }
}