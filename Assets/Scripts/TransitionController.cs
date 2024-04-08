using UnityEngine;
using UnityEngine.UIElements;

public class TransitionController : MonoBehaviour
{
    private TitleController titleController;
    private CameraZoom cameraZoom;

    public void previousScene()
    {
        Debug.Log("change to: " + titleController.getScene());
        cameraZoom.startCameraMove(titleController.getScene());
    }
    public void nextScene()
    {
        Debug.Log("change to: " + titleController.getScene());
        cameraZoom.startCameraMove(titleController.getScene());
    }
}