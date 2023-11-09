using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageScript6 : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform image6;
    // Start is called before the first frame update
    void Start()
    {
        image6 = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        // image1.get
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData) {
        image6.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnPointerDown(PointerEventData eventData) {
    
    }


}
