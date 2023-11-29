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


    public void setSnapOffsets(List<Vector3> offsets)
    {
        foreach (Vector3 offset in offsets)
        {
            snapOffsets.Add(offset);
        }
    }

    public void setSnapPoints(List<GameObject> images)
    {
        foreach (GameObject img in images)
        {
            if(img != gameObject)
            {
                snapPoints.Add(img.name, calcSnapPoint(img.transform.position, snapOffsets[img.name]));
            }
        }
    }


    public Vector3 calcSnapPoint(Vector3 position, Vector3 offset)
    {
        return new Vector3(position.x - offset.x, position.y - offset.y, position.z - offset.z);
    }


    public void updateSnapPoint(string name, Vector3 position)
    {
        snapPositions[name] = calcSnapPoint(position, snapOffsets[name]);
    }
}
