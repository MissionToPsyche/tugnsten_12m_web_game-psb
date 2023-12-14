using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnapToTarget : MonoBehaviour
{
    private float snapRadius = 100.0f;

    public void SnapIfInRange()
    {
        Dictionary<string, Vector2>.ValueCollection snapPoints = GetComponent<ImageController>().getSnapPoints();

        foreach (Vector2 snapPoint in snapPoints)
        {
            if(Mathf.Abs(Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, snapPoint)) < snapRadius)
            {
                GetComponent<RectTransform>().anchoredPosition = snapPoint;
            }
        }
    }

}
