using System.Collections.Generic;
using UnityEngine;

public class ImageGameHelper : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    // private Vector2 canvasOffset = new Vector2(10f, 10f); // Example offset, adjust as needed
    private float searchRadius = 40.0f; // Adjust as needed
    // Call this method when you create each image slice
    public void RegisterOriginalPosition(GameObject slice, Rect sliceRect, Texture2D originalImage)
    {
        Vector2 originalPos = GetOriginalPosition(sliceRect, originalImage);
        originalPositions[slice] = originalPos;
    }

    public Vector2 GetOriginalPosition(Rect sliceRect, Texture2D originalImage)
    {
        float xPosition = sliceRect.x / originalImage.width;
        float yPosition = sliceRect.y / originalImage.height;
        return new Vector2(xPosition, yPosition);
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

    public void SetSnapToTargetPosition(GameObject currentPiece, GameObject referencePiece)
    {
        Vector2 relativePosition = GetRelativePosition(currentPiece, referencePiece);

        // Assuming the referencePiece is already in its correct position
        Vector2 referencePosition = referencePiece.transform.position;
        Vector2 targetPosition = referencePosition + relativePosition;

        SnapToTarget snapToTarget = currentPiece.GetComponent<SnapToTarget>();
        if (snapToTarget != null)
        {
            snapToTarget.SetTargetPosition(targetPosition);
        }
    }
    // get snap position
    public Vector2 CalculateTargetSnapPosition(GameObject currentPiece, GameObject referencePiece)
    {
        if (originalPositions.ContainsKey(currentPiece) && originalPositions.ContainsKey(referencePiece))
        {
            // Get the original relative position
            Vector2 relativePosition = GetRelativePosition(currentPiece, referencePiece);

            // Get the current position of the reference piece
            Vector2 referencePieceCurrentPosition = referencePiece.transform.position;

            // Calculate the target snap position for the current piece
            Vector2 targetSnapPosition = referencePieceCurrentPosition + relativePosition;

            return targetSnapPosition;
        }

        return Vector2.zero;
    }

    public GameObject FindNearestPiece(GameObject currentPiece)
    {
        GameObject nearestPiece = null;
        float minDistance = float.MaxValue;

        foreach (KeyValuePair<GameObject, Vector2> entry in originalPositions)
        {
            GameObject piece = entry.Key;
            if (piece != currentPiece && piece.activeInHierarchy)
            {
                float distance = Vector2.Distance(currentPiece.transform.position, piece.transform.position);
                if (distance < minDistance && distance <= searchRadius)
                {
                    minDistance = distance;
                    nearestPiece = piece;
                }
            }
        }

        return nearestPiece;
    }

}
