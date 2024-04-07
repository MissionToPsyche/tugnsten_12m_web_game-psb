using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    private ChangeScene sceneChanger;
    private TitleController titleController;
    private Button button;
    private CameraZoom cameraZoom;


    public void changeScene()
    {
        Debug.Log("change to: " + titleController.getScene());
        cameraZoom.startCameraMove(titleController.getScene());
        // sceneChanger.NextScene(titleController.getNextScene());
    }


    // Start is called before the first frame update
    void Start()
    {
        titleController = GameObject.Find("TitleController").GetComponent<TitleController>();
        // sceneChanger = GameObject.Find("ChangeScene").GetComponent<ChangeScene>();
        cameraZoom = Camera.main.GetComponent<CameraZoom>();
        button = GetComponent<Button>();
        button.onClick.AddListener(changeScene);
    }
}
