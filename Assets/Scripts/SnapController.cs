using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnapController : MonoBehaviour, IDropHandler
{
    // public List<Transform> snapPoints;
    // [SerializeField] private GameObject image;  // select the image object 
    // [SerializeField] private GameObject dragObject;  // select the image object 
    [SerializeField] private GameObject targetImage;  // select the image object 
    private RectTransform targetRect;
    // [SerializeField] public List<GameObject> targetImages;
    
    // Start is called before the first frame update
    void Start() 
    {
        // targetRect =  targetImage.GetComponent<RectTransform>();
        // canvasGroup = GetComponent<CanvasGroup>();
        
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
            // RectTransform targetRect = GetComponent<RectTranform>();
            // object being dragged aka img1
            // RectTransform droppedRect = eventData.pointerDrag.GetComponent<RectTransform>();

            // snap position is set to anchor position of the target image
            Vector2 snapPosition = targetImage.GetComponent<RectTransform>().anchoredPosition;
            Debug.Log("Image 2: " + snapPosition);

            Vector2 snapOffset = new Vector2(20f, 25f);

            Debug.Log("position + offset " + (snapPosition + snapOffset));

            // Set the position of the dropped object to the corner of the target object
            // droppedRect.anchoredPosition = snapPosition + snapOffset;
            // Debug.Log("Image 1: " + droppedRect.anchoredPosition);

            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = snapPosition + snapOffset;
        
        }
    }
}