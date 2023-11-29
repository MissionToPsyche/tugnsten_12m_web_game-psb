using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class TitleScreenTests : InputTestFixture
{
    Mouse mouse;
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Assets/Scenes/Title.unity");
        mouse = InputSystem.AddDevice<Mouse>();
    }
    public void ClickUI(GameObject ui)
    {
        //Gets the position of a specified UI element and simulates a click there
        Vector3 screenPos = Camera.main.WorldToScreenPoint(ui.transform.position);
        Debug.Log(ui.transform.position);
        Debug.Log(screenPos);
        Set(mouse.position, screenPos);
        Click(mouse.leftButton);
    }
    [UnityTest]
    public IEnumerator TestPlayButton()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Title"));

        GameObject playButton = GameObject.Find("Canvas/Play Button");
        ClickUI(playButton);
        yield return new WaitForSeconds(2f);

        sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Magnetometer_minigame"));
    }





    [UnityTest]
    public IEnumerator TestMinigameMenu()
    {
        GameObject miniSelectMenu = GameObject.Find("Minigame Select Menu");

        GameObject miniSelect = GameObject.Find("Canvas/Minigame Select Button");
        ClickUI(miniSelect);
        yield return new WaitForSeconds(2f);

        GameObject exit = GameObject.Find("Minigame Select Menu/Exit Minigame Select Button");
        ClickUI(exit);
        yield return new WaitForSeconds(2f);
    }
}
