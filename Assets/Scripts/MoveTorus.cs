using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTorus : MonoBehaviour
{
    float startDistance;
    Vector3 startScale;
    private Vector3 initialMousePosition;
    private Quaternion initialTorusRotation;

    void Update()
    {
        // detect when left mouse button initially pressed down
        if (Input.GetMouseButtonDown(0))
        {
            // initialize variables for scaling
            startDistance = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            startScale = transform.localScale;

            // record initial mouse position and torus rotation for rotation
            initialMousePosition = Input.mousePosition;
            initialTorusRotation = transform.rotation;
        }

        // detect left mouse button down
        if (Input.GetMouseButton(0))
        {
            RotateTorus();
            ScaleTorus();
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

            // Update initial mouse position for next frame
            initialMousePosition = Input.mousePosition;
        }
    }

    private void ScaleTorus()
    {
        // calculate scaling factor based on mouse movement
        float endDistance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        float scaleFactor = endDistance / startDistance;

        // apply scaling
        transform.localScale = startScale * scaleFactor;
    }
}
