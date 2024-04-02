using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public ImagerGameController imagerGameController;
    private SnapToTarget snapToTarget;

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling(); // bring image to top
        canvasGroup.blocksRaycasts = false; // make sure image doesn't interact with anything else
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move image with mouse (as a child of a canvas)
        Vector3 newPosition = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out newPosition);
        
        // Define drag boundaries 
        float minX = -7.5f; 
        float maxX = 7.5f;  
        float minY = -2f; 
        float maxY = 3f;  

        // Clamp newPosition to stay within the defined boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        
        rectTransform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        snapToTarget.SnapIfInRange(); // snap to position if close enough
        imagerGameController.updateSnapPositions(gameObject); // update snap positions
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f; // set opacity
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f; // reset opacity
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        snapToTarget = GetComponent<SnapToTarget>();
        imagerGameController = GameObject.Find("Game Controller").GetComponent<ImagerGameController>();
    }

}