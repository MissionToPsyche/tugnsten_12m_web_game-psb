using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject image;
    private CanvasGroup canvasGroup; // canvasGroup attached to the same GameObject as this script
    private bool dragging = false; // flag to indicate dragging state
    public ImageGameHelper imageGameHelper;
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
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
        dragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<GameObject>();
        canvasGroup = GetComponent<CanvasGroup>();
        imageGameHelper = FindAnyObjectByType<ImageGameHelper>();

        if (imageGameHelper == null)
        {
            Debug.LogError("ImageGameHelper not found in the scene.");
        }
    }

    void Update()
    {
        
        if (dragging && imageGameHelper != null)
        {
            // find the nearest piece to this gameObject
            GameObject nearest = imageGameHelper.FindNearestPiece(image);

            if (nearest != null)
            {
                // Call the method to set the snap position relative to the nearest piece
                imageGameHelper.SetTargetPosition(image, nearest);
            }
        }
    }

}
