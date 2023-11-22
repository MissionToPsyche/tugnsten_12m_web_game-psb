using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    private string sceneName;
    private bool zoom;
    private float finalZoom = 25.0f; // how far to zoom (smaller = closer)
    private float zoomSpeed = 30.5f;
    private bool move;
    private float moveSpeed = 25.0f;
    private Vector3 destPosition;

    [SerializeField] private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        // want y to reach 12 but set y dest to 30 b/c camera stops before reaching it
        destPosition = new Vector3(cam.transform.position.x, 30f, cam.transform.position.z); 
    }

    // Update is called once per frame
    void Update()
    {
        if(cam.orthographicSize <= finalZoom && zoom) 
        {
            zoom = false;
        }
        // never reached b/c camera stops moving toward the destination I don't know why
        if(Vector3.Distance(cam.transform.position, destPosition) <= 0.1f && move)
        {
            move = false;
        }
        if(cam.transform.position.y > 12.0f)
        {
            SceneManager.LoadScene(sceneName);
        }

    }

    void ZoomCamera()
    {
        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, finalZoom, Time.deltaTime * zoomSpeed);
    }

    void MoveCamera()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, destPosition, Time.deltaTime * moveSpeed);
    }

    IEnumerator MoveAndZoom()
    {
        while (zoom || move)
        {
            if (zoom) ZoomCamera();
            if (move) MoveCamera();

            yield return null;
        }
    }

    public void startCameraMove(string sceneName)
    {
        zoom = true;
        move = true;
        this.sceneName = sceneName;
        StartCoroutine(MoveAndZoom());
    }

}