using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageScript2 : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
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
        if (eventData.pointerDrag != null)
        {
            RectTransform targetRect = GetComponent<RectTransform>();
            RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();

            // Calculate the position of the bottom left corner of the target object
            Vector2 cornerPosition = targetRect.anchoredPosition - (targetRect.sizeDelta / 2);

            // Set the position of the dropped object to the corner of the target object
            droppedRect.anchoredPosition = cornerPosition + (droppedRect.sizeDelta / 2);
        }
    }
}
