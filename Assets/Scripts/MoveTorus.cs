using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTorus : MonoBehaviour
{
    float startDistance;
    Vector3 startScale;


    void Update()
    {
        // detect when left mouse button initially pressed down
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startDistance = Vector2.Distance(transform.position, worldPos);
            startScale = transform.localScale;
        }
        // detect left mouse button down
        if (Input.GetMouseButton(0))
        {
            rotate();
            resize();
        }
    }

    void rotate()
    {
        //which direction is up
        Vector3 upAxis = new Vector3(0, 0, -1);
        Vector3 mouseScreenPosition = Input.mousePosition;

        //set mouses z to your targets
        mouseScreenPosition.z = transform.position.z;
        Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        transform.LookAt(mouseWorldSpace, upAxis);

        //zero out all rotations except the axis I want
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
    }

    void resize()
	{
		Vector2 newWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		float endDistance = Vector2.Distance(newWorldPos, transform.position);
		float scaleFactor = endDistance / startDistance;

		transform.localScale = startScale * scaleFactor;
	}
}
