using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    private string sceneName;
    private bool zoom;
    private float finalZoom = 1.35f; // how far to zoom (smaller = closer)
    private float zoomSpeed = 6f;
    private bool move;
    private float moveSpeed = 1f;
    private Vector3 destPosition;
    private TitleController titleController;
    private Vector3 slidePosition;

    [SerializeField] private Camera cam;

    public float getOrthographicSize()
    {
        return cam.orthographicSize;
    }

    public Vector3 getPosition()
    {
        return cam.transform.position;
    }

    public void setTitleController(TitleController titleController)
    {
        this.titleController = titleController;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // check if camera in correct position before changeing scenes
        if(cam.orthographicSize <= finalZoom && Vector3.Distance(cam.transform.position, destPosition) <= 0.02f)
        {
            move = false;
            zoom = false;
            SceneManager.LoadScene(sceneName);
        }

    }

    void ZoomCamera()
    {
        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, finalZoom, Time.deltaTime * zoomSpeed);
    }

    void MoveCamera()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, destPosition, Time.fixedDeltaTime * moveSpeed);
    }

    IEnumerator MoveAndZoom()
    {
        while (zoom || move)
        {
            if (zoom) ZoomCamera();
            if (move) MoveCamera();

            if (!zoom && !move) break;

            yield return null;
        }
        yield return null;
    }

    public void startCameraMove(string sceneName)
    {
        slidePosition = titleController.getPosition();
        destPosition = new Vector3(slidePosition.x, 0.45f, slidePosition.z);
        this.sceneName = sceneName;
        zoom = true;
        move = true;
        titleController.cameraZooming = true;
        StartCoroutine(MoveAndZoom());
    }

}