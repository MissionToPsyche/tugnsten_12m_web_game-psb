using System.Collections.Generic;
using UnityEngine;

public class ImageGameHelper : MonoBehaviour
{
    // [SerializeField] private Canvas canvas;
    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    private float searchRadius = 300.0f;
    // Call this method when you create each image slice
    
    public Vector2 GetOriginalPosition(Rect sliceRect, Texture2D originalImage)
    {
        float xPosition = sliceRect.x / originalImage.width;
        float yPosition = sliceRect.y / originalImage.height;
        return new Vector2(xPosition, yPosition);
    }
    public void AddOriginalPosition(GameObject slice, Rect sliceRect, Texture2D originalImage)
    {
        Vector2 originalPos = GetOriginalPosition(sliceRect, originalImage);
        originalPositions[slice] = originalPos;
    }

    public Vector2 GetRelativePosition(GameObject imageA, GameObject imageB)
    {
        if (originalPositions.ContainsKey(imageA) && originalPositions.ContainsKey(imageB))
        {
            Vector2 posA = originalPositions[imageA];
            Vector2 posB = originalPositions[imageB];
            return posA - posB;
        }
        return Vector2.zero;
    }

     public void SetTargetPosition(GameObject current, GameObject reference)
    {
        if (originalPositions.ContainsKey(current) && originalPositions.ContainsKey(reference))
        {
            Vector2 relativePosition = GetRelativePosition(current, reference);
            Vector2 targetPosition = (Vector2)reference.transform.position + relativePosition;

            SnapToTarget snapToTarget = current.GetComponent<SnapToTarget>();
            if (snapToTarget != null)
            {
                snapToTarget.SetSnapPosition(targetPosition);
            }
        }
    }
    public GameObject FindNearestPiece(GameObject current)
    {
        GameObject nearest = null;  // nearest image
        float minDistance = 0;

        foreach (KeyValuePair<GameObject, Vector2> entry in originalPositions)
        {
            GameObject piece = entry.Key;
            if (piece != current && piece.activeInHierarchy) // avoid comparing the current image to itself
            {
                float distance = Vector2.Distance(current.transform.position, piece.transform.position);
                // if distance is within the searchRadius, updates minDistance
                if (distance < minDistance && distance <= searchRadius)
                {
                    minDistance = distance;
                    nearest = piece;
                }
            }
        }

        return nearest;
    }

}
