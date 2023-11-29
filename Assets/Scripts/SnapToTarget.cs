using UnityEngine;
using UnityEngine.EventSystems;

public class SnapToTarget : MonoBehaviour, IDropHandler
{
    public Vector2 targetPosition;
    public float snapThreshold = 50.0f; // How close it needs to be to snap

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();
       
        // if the object is still being drag (!= null)
        if (eventData != null)
        {   
            // if dropped is close enough to target then snaps the object 
            if (Vector2.Distance(droppedRect.anchoredPosition, targetPosition) < snapThreshold)
            {
                transform.position = targetPosition;
            }
        }
    }

    // set the target position from another script
    public void SetSnapPosition(Vector2 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }





    private float snapRadius = 50.0f;
    

    public void OnPointerUp(PointerEventData eventData)
    {
        SnapIfInRange();
    }

    void SnapIfInRange()
    {
        Dictionary<string, Vector3>.ValueCollection snapPoints = GetComponent<ImageController>.getSnapPoints();

        foreach (var snapPoint in snapPoints)
        {
            if(Mathf.Abs(transform.position - snapPoint) < snapRadius)
            {
                transform.position = snapPoint.position;
            }
        }
    }

}
