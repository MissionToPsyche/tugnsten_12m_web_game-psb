using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class OrbitTests
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Orbit_subgame");
    }

    public (OrbitGameController, OrbitUIController) GetControllers()
    {
        GameObject go = GameObject.Find("Game Controller"); 
        OrbitGameController controller = go.GetComponent<OrbitGameController>();
        OrbitUIController ui = controller.ui;

        return (controller, ui);
    }

    [UnityTest]
    public IEnumerator WinDetection()
    {
        (OrbitGameController controller, OrbitUIController ui) = GetControllers();
        controller.StopGame();

        controller.spacecraft.transform.position = new(-1.94866f, 4.803424f);
        controller.spacecraft.velocity = new(-0.497148f, -0.3410891f);
        controller.spacecraft.orbit.CalcOrbitFromOrbiter(controller.spacecraft.transform.position, controller.spacecraft.velocity);
        
        ui.targetOrbit.apoapsisDistance = 5.278355f;
        ui.targetOrbit.periapsisDistance = 1.073405f;
        ui.targetOrbit.rotation = 106;

        controller.StartGame();
        
        controller.CheckWin();
        yield return new WaitForSeconds(3.5f);
        controller.CheckWin();

        Assert.IsTrue(controller.won);
    }

    [UnityTest]
    public IEnumerator EscapeDetection()
    {
        (OrbitGameController controller, OrbitUIController ui) = GetControllers();
        controller.StopGame();

        controller.spacecraft.transform.position = new(3, 0);
        controller.spacecraft.velocity = new(0, 10);
        controller.spacecraft.orbit.CalcOrbitFromOrbiter(controller.spacecraft.transform.position, controller.spacecraft.velocity);

        controller.StartGame();
        yield return null;
        
        Assert.IsTrue(controller.spacecraft.orbit.isEscaping);
    }

    [UnityTest]
    public IEnumerator DeorbitDetection()
    {
        (OrbitGameController controller, OrbitUIController ui) = GetControllers();
        controller.StopGame();

        controller.spacecraft.transform.position = new(3, 0);
        controller.spacecraft.velocity = new(0, 0);
        controller.spacecraft.orbit.CalcOrbitFromOrbiter(controller.spacecraft.transform.position, controller.spacecraft.velocity);

        controller.StartGame();
        yield return null;
        
        Assert.IsTrue(controller.spacecraft.orbit.isCrashing);
    }

    [UnityTest]
    public IEnumerator FailDetection()
    {
        (OrbitGameController controller, OrbitUIController ui) = GetControllers();
        controller.StopGame();

        controller.spacecraft.transform.position = new(2, 0);
        controller.spacecraft.velocity = new(-1, 0);
        controller.spacecraft.orbit.CalcOrbitFromOrbiter(controller.spacecraft.transform.position, controller.spacecraft.velocity);

        controller.StartGame();
        yield return new WaitForSeconds(2f);
        
        Assert.IsTrue(controller.spacecraft.orbit.hasCrashed);
    }

}
