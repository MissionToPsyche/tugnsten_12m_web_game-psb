using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas; // allow to assign a canvas in Unity
    [SerializeField] private GameObject image; // Retrieving RectTransform properties of the image
    private CanvasGroup canvasGroup; // canvasGroup attached to the same GameObject as this script
    private RectTransform imageRect;
    // private float snapPoint { get; set; }
    // private RectTransform image1;a
    // Start is called before the first frame update
    void Start()
    {
        imageRect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;

        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        imageRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // transform.position = Input.mousePosition;
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}
