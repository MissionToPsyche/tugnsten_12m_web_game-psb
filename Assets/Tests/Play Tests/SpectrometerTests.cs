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

    // Start a new scene and test components within it
    // [UnityTest]
    // public IEnumerator StartSceneAndTestComponents()
    // {
    //     // Load a new scene
    //     yield return LoadSceneMode("MyScene", LoadSceneMode.Single);

    //     // Get references to components in the scene
    //     GameObject player = GameObject.Find("Player");
    //     PlayerController playerController = player.GetComponent<PlayerController>();
    //     EnemySpawner enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();

    //     // Test component methods and behavior
    //     playerController.Move(Vector3.right);
    //     yield return new WaitForSeconds(1f); // Wait for player to move
    //     Assert.AreEqual(player.transform.position, Vector3.right); // Assert player moved correctly

    //     enemySpawner.SpawnEnemy();
    //     yield return new WaitForSeconds(1f); // Wait for enemy to spawn
    //     Assert.IsNotNull(GameObject.Find("Enemy")); // Assert enemy was spawned

    //     // Additional tests...
    // }

    // // Test a specific component in the current scene
    // [UnityTest]
    // public IEnumerator TestSpecificComponent()
    // {
    //     // Get reference to the component
    //     HealthSystem healthSystem = GameObject.Find("Player").GetComponent<HealthSystem>();

    //     // Test component methods and behavior
    //     healthSystem.TakeDamage(20);
    //     Assert.AreEqual(healthSystem.CurrentHealth, 80); // Assert health was reduced correctly

    //     healthSystem.Heal(30);
    //     Assert.AreEqual(healthSystem.CurrentHealth, 100); // Assert health was healed correctly

    //     // Additional tests...

    //     yield return null;
    // }
}