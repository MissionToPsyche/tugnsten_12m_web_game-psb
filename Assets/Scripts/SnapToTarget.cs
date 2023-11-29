using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnapToTarget : MonoBehaviour
{
    // public Vector2 targetPosition;
    // public float snapThreshold = 50.0f; // How close it needs to be to snap

    // public void OnDrop(PointerEventData eventData)
    // {
    //     RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();
       
    //     // if the object is still being drag (!= null)
    //     if (eventData != null)
    //     {   
    //         // if dropped is close enough to target then snaps the object 
    //         if (Vector2.Distance(droppedRect.anchoredPosition, targetPosition) < snapThreshold)
    //         {
    //             transform.position = targetPosition;
    //         }
    //     }
    // }

    // // set the target position from another script
    // public void SetSnapPosition(Vector2 newTargetPosition)
    // {
    //     targetPosition = newTargetPosition;
    // }





    private float snapRadius = 100.0f;
    

    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     SnapIfInRange();
    // }

    public void SnapIfInRange()
    {
        Debug.Log("enter snap");
        Dictionary<string, Vector3>.ValueCollection snapPoints = GetComponent<ImageController>().getSnapPoints();

        foreach (Vector3 snapPoint in snapPoints)
        {
            if(Mathf.Abs(Vector3.Distance(transform.position, snapPoint)) < snapRadius)
            {
                Debug.Log("snap");
                Debug.Log("current position: " + transform.position);
                Debug.Log("snapPoint: " + snapPoint);
                Debug.Log("radius: " + snapRadius);
                transform.position = snapPoint;
                Debug.Log("new position: " + transform.position);
            }
        }
    }

}
