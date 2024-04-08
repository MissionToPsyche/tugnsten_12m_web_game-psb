using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnapToTarget : MonoBehaviour
{
    private float snapRadius = 50.0f;

    public void SnapIfInRange()
    {
        Dictionary<string, Vector2>.ValueCollection snapPoints = GetComponent<ImageController>().getSnapPoints();

        foreach (Vector2 snapPoint in snapPoints)
        {
            if(Mathf.Abs(Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, snapPoint)) < snapRadius)
            {
                GetComponent<RectTransform>().anchoredPosition = snapPoint;
                AudioClip audioClip = (AudioClip)Resources.Load("Sounds/setting-pencil-down-87055");
                SoundManager.Instance.PlaySound(audioClip);
                break;
            }
        }
    }

}
