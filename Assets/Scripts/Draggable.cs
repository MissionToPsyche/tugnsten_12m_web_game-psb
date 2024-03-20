using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public ImagerController imagerController;
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
        rectTransform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        snapToTarget.SnapIfInRange(); // snap to position if close enough
        imagerController.updateSnapPositions(gameObject); // update snap positions
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
        imagerController = GameObject.Find("ImagerController").GetComponent<ImagerController>();
    }

}
