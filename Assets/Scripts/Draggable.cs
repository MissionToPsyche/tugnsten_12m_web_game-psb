using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // private GameObject image;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup; // canvasGroup attached to the same GameObject as this script
    private bool dragging = false; // flag to indicate dragging state
    public ImagerGameHelper imagerGameHelper;
    private SnapToTarget snapToTarget;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // dragging = true;
        // canvasGroup.alpha = .5f;
        // canvasGroup.blocksRaycasts = false;

        dragging = true;
        rectTransform .SetAsLastSibling();
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        // Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {

        // Vector3 mousePosition = Input.mousePosition;

        // bringing the object to top layer of the screen 
        // mousePosition.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // transform.position = worldPosition;


        // Debug.Log("OnDrag");
        // Debug.Log("Object's position " + transform.position);
        // Debug.Log("mouse Pousition: " + Input.mousePosition);

        // Move the object with the mouse movement
        Vector3 newPosition = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out newPosition);
        rectTransform.position = newPosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // Debug.Log("OnEndDrag");

        snapToTarget.SnapIfInRange();        
        // update snap positions
        imagerGameHelper.updateSnapPositions(gameObject);
    }

    // private List<GameObject> images = SliceImage.getImages();//////////

    // public void updateSnapPositions()
    // {
    //     foreach (GameObject img in images)
    //     {
    //         if(img != gameObject)
    //         {
    //             img.GetComponent<ImageController>.updateSnapPoint(gameObject.name, gameObject.transform.position);
    //         }
    //     }
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
        canvasGroup.alpha = 0.5f;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
        canvasGroup.alpha = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        // image = GetComponent<GameObject>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        snapToTarget = GetComponent<SnapToTarget>();
        // imageGameHelper = FindAnyObjectByType<ImageGameHelper>();
        imagerGameHelper = GameObject.Find("ImagerHelper").GetComponent<ImagerGameHelper>();

        if (imagerGameHelper == null)
        {
            Debug.LogError("ImageGameHelper not found in the scene.");
        }
    }

    void Update()
    {
        
        // if (dragging && imageGameHelper != null)
        // {
        //     // find the nearest piece to this gameObject
        //     GameObject nearest = imageGameHelper.FindNearestPiece(image);

        //     if (nearest != null)
        //     {
        //         // call the method to set the snap position relative to the nearest piece
        //         imageGameHelper.SetTargetPosition(image, nearest);
        //     }
        // }
    }

}
