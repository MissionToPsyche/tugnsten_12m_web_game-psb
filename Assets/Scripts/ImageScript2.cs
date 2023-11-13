using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageScript2 : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] 
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform image2;
    // Start is called before the first frame update
    void Start()
    {
        image2 = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        // image1.get
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        image2.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnPointerDown(PointerEventData eventData)
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
