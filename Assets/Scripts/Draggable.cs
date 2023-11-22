using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // private Canvas canvas; // allow to assign a canvas in Unity
    private GameObject image; // Retrieving RectTransform properties of the image

    private Canvas canvas;
    // private GameObject canvasGUI;

    private SliceImage sliceImage;

    private CanvasGroup canvasGroup; // canvasGroup attached to the same GameObject as this script
    private RectTransform imageRect;

    // Start is called before the first frame update
    void Start()
    {
        // image = GetComponent<GameObject>();
        // canvas = GetCan<Canvas>();
        // imageRect = GetComponent<RectTransform>();
        // canvasGroup = GetComponent<CanvasGroup>();
        // sliceImage = new SliceImage();
        canvas = GetComponent<Canvas>();

        // canvasGUI = GameObject.Find("CanvasGUI");
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
        // var canvasScaler = GetComponent<CanvasScaler>();
        // if (canvasScaler)
        // {
        //     float scaleFactor = canvasScaler.scaleFactor;
        //     imageRect.anchoredPosition += eventData.delta / scaleFactor;
        // }
        // else
        // {
        //     imageRect.anchoredPosition += eventData.delta;
        // }

        // var canvas = GetComponent<Canvas>();
        imageRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
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
