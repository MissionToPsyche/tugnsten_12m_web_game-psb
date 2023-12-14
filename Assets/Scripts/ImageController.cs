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
                Debug.Log(gameObject.name + " to " + img.name);
                // Debug.Log("curent pos: " + gameObject.transform.position);
                Debug.Log("current rect pos: " + gameObject.GetComponent<RectTransform>().anchoredPosition);
                Debug.Log("offset: " + snapOffsets[img.name]);
                Debug.Log("snap point: " + snapPoints[img.name]);
                // Debug.Log("rect snap point: " + img.GetComponent<RectTransform>().anchoredPosition);
            }
        }
        Debug.Log("NEXT");
    }


    public Vector2 calcSnapPoint(Vector2 position, Vector2 offset)
    {
        // return new Vector3(position.x - offset.x, position.y - offset.y, position.z - offset.z);
        return position+offset;
        // return position;
    }


    public void updateSnapPoint(string name, Vector2 position)
    {
        snapPoints[name] = calcSnapPoint(position, snapOffsets[name]);
    }
}
