using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveTorus : MonoBehaviour
{
    public float startDistance {get;set;}
    public Vector3 startScale {get;set;}
    public Vector3 initialMousePosition {get;set;}
    public Quaternion initialTorusRotation {get;set;}
    private GameScreenUI gui;
    private bool uiFlag = false;
    public GameObject northObject {get;set;}
    public GameObject southObject {get;set;}

    void Start()
    {
        // TODO: don't detect mouse click over UI
        gui = GameObject.Find("UIDocument").GetComponent<GameScreenUI>();
        gui.getBottomContainer().RegisterCallback<MouseDownEvent>(OnElementClicked);
        gui.getTopContainer().RegisterCallback<MouseDownEvent>(OnElementClicked);
        // gui.getBottomContainer().RegisterCallback<MouseUpEvent>(OnElementClicked);
        // gui.getTopContainer().RegisterCallback<MouseUpEvent>(OnElementClicked);

        northObject = GameObject.Find("North");
        southObject = GameObject.Find("South");
    }

    void Update()
    {
        // detect when left mouse button initially pressed down
        if (Input.GetMouseButtonDown(0) && !uiFlag)
        {
            // initialize variables for scaling
            startDistance = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            startScale = transform.localScale;

            // record initial mouse position and torus rotation for rotation
            initialMousePosition = Input.mousePosition;
            initialTorusRotation = transform.rotation;
        }

        // detect left mouse button down
        if (Input.GetMouseButton(0) && !uiFlag)
        {
            RotateTorus();
            ScaleTorus();
        }

        if (Input.GetMouseButtonUp(0))
        {
            uiFlag = false;
        }
    }

    private void RotateTorus()
    {
        // Calculate initial and current mouse positions in world space
        Vector3 initialMouseWorldPos = Camera.main.ScreenToWorldPoint(initialMousePosition);
        Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Ignore z-axis component to ensure rotation only around the z-axis
        initialMouseWorldPos.z = 0f;
        currentMouseWorldPos.z = 0f;

        // Check if mouse has moved
        if (initialMouseWorldPos != currentMouseWorldPos)
        {
            // Calculate rotation angle around the z-axis
            Vector3 initialDirection = (initialMouseWorldPos - transform.position).normalized;
            Vector3 currentDirection = (currentMouseWorldPos - transform.position).normalized;
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) - Mathf.Atan2(initialDirection.y, initialDirection.x);

            // Apply rotation around the z-axis
            transform.Rotate(Vector3.forward, angle * Mathf.Rad2Deg, Space.Self);

            southObject.transform.rotation = Quaternion.identity;
            northObject.transform.rotation = Quaternion.identity;

            // Update initial mouse position for next frame
            initialMousePosition = Input.mousePosition;
        }
    }

    private void ScaleTorus()
    {
        // calculate scaling factor based on mouse movement
        float endDistance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        float scaleFactor = endDistance / startDistance;

        // maximum scale limit
        float maxScale = 3.0f;

        // Calculate the new scale while applying the maximum scale limit
        Vector3 newScale = startScale * scaleFactor;
        newScale.x = Mathf.Min(newScale.x, maxScale);
        newScale.y = Mathf.Min(newScale.y, maxScale);
        newScale.z = Mathf.Min(newScale.z, maxScale);

        // apply scaling
        transform.localScale = newScale;
    }

    void OnElementClicked(EventBase evt)
    {
        uiFlag = true;
    }
}
