using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup; // canvasGroup attached to the same GameObject as this script

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;

        // Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector3 mousePosition = Input.mousePosition;

        // bringing the object to top layer of the screen 
        mousePosition.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;

        // Debug.Log("OnDrag");
        // Debug.Log("Object's position " + transform.position);
        // Debug.Log("mouse Pousition: " + Input.mousePosition);


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }

}
