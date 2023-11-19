using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCamera : MonoBehaviour
{
    public Vector3[] positions;
    private int currentIndex = 0;
    private float speed = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = positions[currentIndex];
        
        if(Input.GetKeyUp(KeyCode.RightArrow)) {
            if(currentIndex < positions.Length  -1)
            {
                currentIndex++;
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if(currentIndex > 0) 
            {
                currentIndex--;
            }
        }
        
        transform.position = Vector3.Lerp(transform.position, currentPos, speed*Time.deltaTime);
        
    }
}