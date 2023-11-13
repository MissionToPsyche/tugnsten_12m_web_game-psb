using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnapController : MonoBehaviour, IDropHandler
{
    public List<Transform> snapPoints;
    [SerializeField] 
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform image;
    // Start is called before the first frame update
    void Start() 
    {
        image = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update() 
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        // if the object is still being drag (!= null)
        if (eventData.pointerDrag != null)
        {
            // img 2
            RectTransform targetRect = GetComponent<RectTransform>();
            // object being dragged aka img1
            RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();

            // snap position is set to anchor position of img2
            Vector2 snapPosition = targetRect.anchoredPosition;

            Vector2 snapOffset = new Vector2(20f, 25f);

            // Set the position of the dropped object to the corner of the target object
            droppedRect.anchoredPosition = snapPosition + snapOffset;
        }
    }
}