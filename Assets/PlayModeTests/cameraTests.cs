using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class cameraTests
{
    // // A Test behaves as an ordinary method
    // // [Test]
    // // public void cameraTestsSimplePasses()
    // // {
    // //     // Use the Assert class to test conditions
    // // }

    // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator cameraPanRight()
    // {
    //     // Create a GameObject with the SlideCamera script
    //     GameObject cameraObject = new GameObject("TestCamera");
    //     SlideCamera slideCamera = cameraObject.AddComponent<SlideCamera>();

    //     // Save the initial position
    //     Vector3 initialPosition = cameraObject.transform.position;

    //     // Call the changeIndex method with the right arrow key
    //     slideCamera.changeIndex(KeyCode.RightArrow);

    //     // Wait to allow the camera to move
    //     yield return new WaitForSeconds(5.0f);

    //     // Check if the camera has moved to the next position with an epsilon tolerance
    //     Assert.IsTrue(Vector3.Distance(slideCamera.positions[1], cameraObject.transform.position) < 0.01f);

    //     // Destroy the test object
    //     Object.Destroy(cameraObject);
    // }

    // [UnityTest]
    // public IEnumerator cameraPanLeft()
    // {
    //     // Create a GameObject with the SlideCamera script
    //     GameObject cameraObject = new GameObject("TestCamera");
    //     SlideCamera slideCamera = cameraObject.AddComponent<SlideCamera>();

    //     // Save the initial position
    //     Vector3 initialPosition = slideCamera.positions[1];

    //     // Call the changeIndex method with the right arrow key
    //     slideCamera.changeIndex(KeyCode.LeftArrow);

    //     // Wait to allow the camera to move
    //     yield return new WaitForSeconds(5.0f);

    //     // Check if the camera has moved to the next position with an epsilon tolerance
    //     Assert.IsTrue(Vector3.Distance(slideCamera.positions[0], cameraObject.transform.position) < 0.01f);

    //     // Destroy the test object
    //     Object.Destroy(cameraObject);
    // }

    // [UnityTest]
    // public IEnumerator cameraZoom()
    // {
    //     // Load the initial scene with the CameraZoom script on a button
    //     SceneManager.LoadScene("Test_Console");

    //     // Wait for one frame to ensure the scene is loaded
    //     yield return null;

    //     // Find the GameObject named "Zoom"
    //     GameObject zoomObject = GameObject.Find("Zoom");
    //     Assert.NotNull(zoomObject, "Zoom GameObject not found");

    //     // Get the CameraZoom component from the GameObject
    //     CameraZoom cameraZoom = zoomObject.GetComponent<CameraZoom>();
    //     Assert.NotNull(cameraZoom, "CameraZoom component not found on the Zoom GameObject");

    //     // Get the initial scene name
    //     string initialScene = SceneManager.GetActiveScene().name;

    //     // Start camera movement and zoom by interacting with the button
    //     cameraZoom.startCameraMove("Title");

    //     // Wait for the camera to finish moving and zooming
    //     yield return new WaitForSeconds(3.0f); // Adjust this time based on your expected duration

    //     // CAN'T DO BECAUSE SCENE CHANGES AND SO THE CAMERA IS DIFFERENT
    //     // // Assert that the camera has reached its destination
    //     // Assert.AreEqual(25.0f, cameraZoom.getOrthographicSize(), 0.01f); // Tolerance for float comparison
    //     // Assert.IsTrue(Vector3.Distance(new Vector3(cameraZoom.getPosition().x, 12.0f, cameraZoom.getPosition().z),
    //     //                 cameraZoom.getPosition()) < 0.5f);

    //     // Get the scene name after the expected change
    //     string newScene = SceneManager.GetActiveScene().name;

    //     // Assert that the scene has changed as expected
    //     Assert.AreNotEqual(initialScene, newScene);
    // }
    
}
