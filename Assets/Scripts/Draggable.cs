using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject image; // Retrieving RectTransform properties of the image

    // private Canvas canvas {get; set;}

    private Canvas canvas;

    private SliceImage sliceImage;

    private CanvasGroup canvasGroup; // canvasGroup attached to the same GameObject as this script
    private RectTransform imageRect;

    // Start is called before the first frame update
    void Start()
    {
    //     GameObject canvasObject = GameObject.Find("Canvas");
    //     if(canvasObject != null)
    //     {
    //         canvas = GetComponent<Canvas>();
    //     }
        // image = GetComponent<GameObject>();
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
        // imageRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // imageRect.anchoredPosition = Input.mousePosition;

        transform.position +=(Vector3) eventData.delta / canvas.scaleFactor;
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

    public Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

     public void SetCanvas(Canvas newCanvas) {
        canvas = newCanvas;
    }

}
