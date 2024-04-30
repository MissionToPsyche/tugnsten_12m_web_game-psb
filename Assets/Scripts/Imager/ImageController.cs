using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    private Dictionary<string, Vector2> snapPoints = new Dictionary<string, Vector2>();
    private Dictionary<string, Vector2> snapOffsets = new Dictionary<string, Vector2>();


    public Dictionary<string, Vector2>.ValueCollection getSnapPoints()
    {
        return snapPoints.Values;
    }

    public void setSnapOffsets(Dictionary<string, Vector2> offsets)
    {
        foreach (var offset in offsets)
        {
            snapOffsets[offset.Key] = offset.Value;
        }
    }

    public void setSnapPoints(List<GameObject> images)
    {
        foreach (GameObject img in images)
        {
            if(img != gameObject)
            {
                snapPoints.Add(img.name, calcSnapPoint(img.GetComponent<RectTransform>().anchoredPosition, snapOffsets[img.name]));
            }
        }
    }

    public Vector2 calcSnapPoint(Vector2 position, Vector2 offset)
    {
        return position + offset;
    }

    // updates the snpa point of this image to the parameter image
    public void updateSnapPoint(string name, Vector2 position)
    {
        snapPoints[name] = calcSnapPoint(position, snapOffsets[name]);
    }
}
