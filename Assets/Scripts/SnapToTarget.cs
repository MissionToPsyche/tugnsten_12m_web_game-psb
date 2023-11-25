using UnityEngine;
using UnityEngine.EventSystems;

public class SnapToTarget : MonoBehaviour, IDropHandler
{
    public Vector2 targetPosition;
    public float snapThreshold = 50.0f; // How close it needs to be to snap

    public void OnDrop(PointerEventData eventData)
    {
        // RectTransform rectTransform = GetComponent<RectTransform>();

         if (Vector2.Distance(transform.position, targetPosition) < snapThreshold)
        {
            transform.position = targetPosition; 
        }
    }

    // set the target position from another script
    public void SetTargetPosition(Vector2 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }
}
