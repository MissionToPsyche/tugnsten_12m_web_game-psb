using System.Collections.Generic;
using UnityEngine;

public class ImageGameHelper : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    // a hash to store the img object and it's position relative to the original picture
    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    private float searchRadius = 400.0f;
    
    // store the original position of the sliced image 
    public void AddOriginalPosition(GameObject slice, Rect sliceRect, Texture2D originalImage)
    {
        Vector2 originalPos = GetOriginalPosition(sliceRect, originalImage);
        originalPositions[slice] = originalPos;

        Debug.Log("AddOriginalPosition: (" + originalPositions[slice] + ", " + originalPos + ")");
    }

    // find/calculate the original position of a sliced image based on the image itself before cut
     public Vector2 GetOriginalPosition(Rect sliceRect, Texture2D originalImage)
    {
        float xPosition = sliceRect.x / originalImage.width;
        float yPosition = sliceRect.y / originalImage.height;
        return new Vector2(xPosition, yPosition);

    }

    // find the distance between two images 
    public Vector2 GetRelativePosition(GameObject imageA, GameObject imageB)
    {
        if (originalPositions.ContainsKey(imageA) && originalPositions.ContainsKey(imageB))
        {
            Vector2 posA = originalPositions[imageA];
            Vector2 posB = originalPositions[imageB];
            
            Debug.Log("Relative distance: " + (posA - posB));

            return posA - posB;
        }
        
        Debug.Log("Could not find relative position ");
        return Vector2.zero;
    }

    // set the target snap position based on the relative position 
     public void SetTargetPosition(GameObject current, GameObject reference)
    {
        // look throught the hash map
        if (originalPositions.ContainsKey(current) && originalPositions.ContainsKey(reference))
        {
            // get the distance between the two img
            Vector2 relativePosition = GetRelativePosition(current, reference);
            // set a target position for snapping
            Vector2 targetPosition = (Vector2)reference.transform.position + relativePosition;

            SnapToTarget snapToTarget = current.GetComponent<SnapToTarget>();   // suppose to call the snaptarget component of the img obj instead
            if (snapToTarget != null)
            {
                snapToTarget.SetSnapPosition(targetPosition);
            }
            
            Debug.Log("Snap Target Position: " + targetPosition);
        }
    }

    // see if other images is near the current dragging img
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
