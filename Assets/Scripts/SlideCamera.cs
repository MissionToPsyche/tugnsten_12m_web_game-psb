using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCamera : MonoBehaviour
{

    // private Vector3[] positions = new Vector3[] {
    //     new Vector3(0f, 0f, -10f),
    //     new Vector3(145f, 0f, -10f),
    //     new Vector3(293f, 0f, -10f),
    //     new Vector3(438f, 0f, -10f),
    // };
    private Vector3[] positions;

    private int currentIndex = 0;
    private float speed = 6.0f;

    private bool moving = false;

    private TitleController titleController;

    public int getCurrentIndex()
    {
        return currentIndex;
    }


    // Start is called before the first frame update
    void Start()
    {
        titleController = GameObject.Find("TitleController").GetComponent<TitleController>();
        positions = titleController.getAllPositions();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = positions[currentIndex];

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveNextPos();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movePrevPos();
        }

        // Check the distance between the current position and the target position
        float distance = Vector3.Distance(transform.position, currentPos);

        if (moving)
        {
            // If the distance is below a certain threshold, snap to the target position
            if (distance < 0.01f)
            {
                transform.position = currentPos;
                moving = false;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, currentPos, speed * Time.deltaTime);
            }
        }

        // transform.position = Vector3.Lerp(transform.position, currentPos, speed*Time.deltaTime);

    }
    public void movePrevPos()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            titleController.setMinigame(currentIndex);
            moving = true;
        }
    }

    public void moveNextPos()
    {
        if (currentIndex < positions.Length - 1)
        {
            currentIndex++;
            titleController.setMinigame(currentIndex);
            moving = true;
        }
    }

    // for testing
    public void changeIndex(KeyCode key)
    {
        if (key == KeyCode.RightArrow && currentIndex < positions.Length - 1)
        {
            currentIndex++;
        }
        else if (key == KeyCode.LeftArrow && currentIndex > 0)
        {
            currentIndex--;
        }
    }
}