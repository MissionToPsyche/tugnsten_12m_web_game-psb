using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GravityTests
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Science_minigame");
    }

    public (GravitySciController, GravitySciUIController) GetControllers()
    {
        GameObject go = GameObject.Find("Game Controller"); 
        GravitySciController controller = go.GetComponent<GravitySciController>();
        GravitySciUIController ui = controller.ui;

        return (controller, ui);
    }

    [Test]
    public void ColorCalculation()
    {
        GameObject go = new();
        Waveform wave = go.AddComponent<Waveform>();
        wave.wavelength = 200;

        Color waveColor = wave.CalculateColor();

        Assert.AreEqual(new Color(0, 0.125f, 1), waveColor);
    }

    [UnityTest]
    public IEnumerator DistortionGeneration()
    {
        (GravitySciController controller, GravitySciUIController ui) = GetControllers();

        Assert.AreEqual(controller.generator.numPositions, controller.orbit.distortions.Count);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ScoreCalculation()
    {
        (GravitySciController controller, GravitySciUIController ui) = GetControllers();

        foreach (Distortion distortion in controller.orbit.distortions)
        {
            distortion.intensity = distortion.trueIntensity;
        }

        controller.CalcScore();

        Assert.AreEqual(10000, controller.score);

        yield return null;
    }
}
