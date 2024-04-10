using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    private string sceneName;
    private bool zoom;
    // private float finalZoom = 25.0f; // how far to zoom (smaller = closer)
    private float finalZoom = 4.0f; // how far to zoom (smaller = closer)
    // private float zoomSpeed = 30.5f;
    private float zoomSpeed = 6f;
    private bool move;
    // private float moveSpeed = 25.0f;
    private float moveSpeed = 20.0f;
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
        //titleController = GameObject.Find("TitleController").GetComponent<TitleController>();
        // want y to reach 12 but set y dest to 30 b/c camera stops before reaching it
        // destPosition = new Vector3(cam.transform.position.x, 30f, cam.transform.position.z); 
        // destPosition = new Vector3(cam.transform.position.x, 45.0f, cam.transform.position.z); 
    }

    // Update is called once per frame
    void Update()
    {
        // if(cam.orthographicSize <= finalZoom && zoom) 
        // {
        //     zoom = false;
        // }
        if(cam.fieldOfView <= finalZoom && zoom) 
        {
            zoom = false;
        }
        // never reached b/c camera stops moving toward the destination I don't know why
        if(Vector3.Distance(cam.transform.position, destPosition) <= 0.1f && move)
        {
            move = false;
        }
        // if(cam.transform.position.y > 12.0f && cam.orthographicSize == finalZoom)
        // {
        //     SceneManager.LoadScene(sceneName);
        // }
        // if(cam.transform.position.y >= 9.0f && cam.fieldOfView <= finalZoom)
        // {
        //     move = false;
        //     zoom = false;
        //     cam.transform.position = new Vector3(slidePosition.x, 9.0f, slidePosition.z);
        //     SceneManager.LoadScene(sceneName);
        // }
        if(cam.fieldOfView <= finalZoom)
        {
            move = false;
            zoom = false;
            cam.transform.position = new Vector3(slidePosition.x, 9.0f, slidePosition.z);
            SceneManager.LoadScene(sceneName);
        }

    }

    void ZoomCamera()
    {
        // cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, finalZoom, Time.deltaTime * zoomSpeed);
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, finalZoom, Time.fixedDeltaTime * zoomSpeed);
    }

    void MoveCamera()
    {
        Debug.Log("dest: " + destPosition);
        Debug.Log("pos1: " + cam.transform.position);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, destPosition, Time.fixedDeltaTime * moveSpeed);
        Debug.Log("pos2: " + cam.transform.position);
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
        Debug.Log("start zoom");
        slidePosition = titleController.getPosition();
        destPosition = new Vector3(slidePosition.x, 45.0f, slidePosition.z);
        Debug.Log("dest: " + destPosition);
        this.sceneName = sceneName;
        Debug.Log("name: " + this.sceneName);
        zoom = true;
        move = true;
        StartCoroutine(MoveAndZoom());
    }

}