using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    private Dictionary<string, Vector3> snapPoints = new Dictionary<string, Vector3>();
    private Dictionary<string, Vector3> snapOffsets = new Dictionary<string, Vector3>();

    public Dictionary<string, Vector3>.ValueCollection getSnapPoints()
    {
        return snapPoints.Values;
    }


    public void setSnapOffsets(Dictionary<string, Vector3> offsets)
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
                snapPoints.Add(img.name, calcSnapPoint(img.transform.position, snapOffsets[img.name]));
                Debug.Log(gameObject.name + " to " + img.name);
                Debug.Log("offset: " + snapOffsets[img.name]);
                Debug.Log("snap point: " + snapPoints[img.name]);
            }
        }
        Debug.Log("NEXT");
    }


    public Vector3 calcSnapPoint(Vector3 position, Vector3 offset)
    {
        // return new Vector3(position.x - offset.x, position.y - offset.y, position.z - offset.z);
        return position-offset;
    }


    public void updateSnapPoint(string name, Vector3 position)
    {
        snapPoints[name] = calcSnapPoint(position, snapOffsets[name]);
    }
}
