using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;

public class SliceImage : MonoBehaviour
{
    private Texture2D originalImage;
    [SerializeField] Canvas canvas; // SerializeField makes this variable visible in unity editor but cannot be accessed by other scripts (unlike public variables)
    private string path;
    private byte[] bytes;
    private List<Vector2> starts;
    private List<GameObject> images { get; set; }
    private GameObject image;
    private ImageGameHelper imageGameHelper;

    // private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();


    public void slice()
    {
        // can probably take out (just here to reset for testing)
        foreach (GameObject obj in images)
        {
            Destroy(obj);
        }
        images.Clear();
        starts.Clear();


        Texture2D slicedTexture = createSectionOfOriginal();

        float slicedWidth = slicedTexture.width;
        float slicedHeight = slicedTexture.height;

        // image
        float imgSize = 0.5f;
        float imgWidth = slicedWidth * imgSize;
        float imgHeight = slicedHeight * imgSize;

        // difference
        float maxOverlap = 0.2f;
        float yDiff = imgHeight * maxOverlap;
        float xDiff = imgWidth * maxOverlap;
        Vector2 diff = new Vector2(xDiff, yDiff);

        // find a different img
        int tries = 10;
        float spaceX = slicedWidth / tries;
        float spaceY = slicedHeight / tries;
        float space = Mathf.Min(spaceX, spaceY);
        int gridSize = 6;
        List<int[]> cells = new List<int[]>();
        for (int i = 0; i < Mathf.CeilToInt(gridSize * imgSize); i++)
        {
            for (int j = 0; j < Mathf.CeilToInt(gridSize * imgSize); j++)
            {
                cells.Add(new int[] { i, j });
            }
        }

        int numImgs = 4;

        // creates images
        for (int imgNum = 0; imgNum < numImgs; imgNum++)
        {
            int ctr = 0;
            float imgXStart;
            float imgYStart;
            Vector2 start;
            int randomCell = Random.Range(0, cells.Count); // select a cell

            // tries to find images different enough
            do
            {
                ctr++;

                int[] cell = cells[randomCell];
                int cellX = cell[0];
                int cellY = cell[1];
                float cellWidth = slicedWidth / gridSize;
                float cellHeight = slicedHeight / gridSize;

                // get a start point in the cell
                float adjustedCellWidth = cellWidth / space;
                float adjustedCellHeight = cellHeight / space;
                float randX = Random.Range(0, (int)adjustedCellWidth) * space;
                float randY = Random.Range(0, (int)adjustedCellHeight) * space;
                imgXStart = cellX * cellWidth + randX;
                imgYStart = cellY * cellHeight + randY;
                start = new Vector2(imgXStart, imgYStart);

                // TO DO: write code to use most different image
                if (ctr > tries)
                {
                    break;
                }

            } while (!isStartDifferent(start, diff));

            starts.Add(start);
            cells.RemoveAt(randomCell);

            GameObject imgObject = createImageObject(imgWidth, imgHeight, start, slicedTexture, imgNum);

            // // calc original position
            // Rect sliceRect = new Rect(start.x, start.y, imgWidth, imgHeight);
            // Vector2 originalPos = GetOriginalPosition(sliceRect);
            // originalPositions.Add(imgObject, originalPos);

            // // Calculate the target snap position and set it
            // Vector2 targetSnapPosition = CalculateTargetSnapPosition(sliceRect);
            // SnapToTarget snapToTarget = imgObject.AddComponent<SnapToTarget>();
            // snapToTarget.SetTargetPosition();




            images.Add(imgObject);
        }
    }

    private bool isStartDifferent(Vector2 newStart, Vector2 diff)
    {
        foreach (Vector2 start in starts)
        {
            if (Mathf.Abs(start.x - newStart.x) < diff.x && Mathf.Abs(start.y - newStart.y) < diff.y)
            {
                return false;
            }
        }
        return true;
    }

    private Texture2D createSectionOfOriginal()
    {
        float originalWidth = originalImage.width;
        float originalHeight = originalImage.height;

        float sectionSize = 0.5f;
        // TO DO: separate x vs y edgeAdjustment for left side ?? 
        float edgeAdjustment = 1 / 3f; // left side
        float sectionWidth = originalWidth * sectionSize;
        float sectionHeight = originalHeight * sectionSize;
        // TO DO: multiple section width/height by a right edge adjustment ?? V
        float sectionXStart = Random.Range(originalWidth * edgeAdjustment, originalWidth - sectionWidth);
        float sectionYStart = Random.Range(originalHeight * edgeAdjustment, originalHeight - sectionHeight);

        Rect regionToCopy = new Rect(sectionXStart, sectionYStart, sectionWidth, sectionHeight);
        Texture2D slicedTexture = new Texture2D((int)regionToCopy.width, (int)regionToCopy.height); // Create a new Texture2D to store the sliced part
        // Get the pixels from the original texture within the specified region
        Color[] pixels = originalImage.GetPixels((int)regionToCopy.x, (int)regionToCopy.y, (int)regionToCopy.width, (int)regionToCopy.height);
        slicedTexture.SetPixels(pixels); // Set the pixels in the new texture
        slicedTexture.Apply(); // Apply changes to the new texture

        return slicedTexture;
    }

    public GameObject createImageObject(float imgWidth, float imgHeight, Vector2 start, Texture2D slicedTexture, int imgNum)
    {
        GameObject imgObject = new GameObject();
        imgObject.name = "img" + imgNum;

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(canvas.transform); // setting parent
        trans.localScale = Vector3.one;
        // TO DO: position still needs to be determined V
        trans.anchoredPosition = new Vector2((imgWidth + 20) * imgNum, (imgHeight + 20) * imgNum); // setting position
        trans.sizeDelta = new Vector2(imgWidth, imgHeight); // set the size

        // adding canvas group component
        CanvasGroup group = imgObject.AddComponent<CanvasGroup>();
        group.alpha = 1;
        group.blocksRaycasts = true;

        // adding image component
        Image image = imgObject.AddComponent<Image>();

        Vector2 imgSize = new Vector2(imgWidth, imgHeight);
        // sets the texture of the sprite to a section of the slicedImage and specifies the center of the new image
        image.sprite = Sprite.Create(slicedTexture, new Rect(start, imgSize), new Vector2(0.5f, 0.5f));

        imgObject.transform.SetParent(canvas.transform); // sets the parent

        // add script component
        imgObject.AddComponent<Draggable>();
        imgObject.AddComponent<SnapToTarget>();

        Rect sliceRect = new Rect(start.x, start.y, imgWidth, imgHeight);
        imageGameHelper.AddOriginalPosition(imgObject, sliceRect, originalImage);


        return imgObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        bytes = File.ReadAllBytes(path);
        originalImage = new Texture2D(1, 1); // size will be replaced by image size
        originalImage.LoadImage(bytes); // create a texture2d asset from the image


        starts = new List<Vector2>();
        images = new List<GameObject>();
        imageGameHelper = FindAnyObjectByType<ImageGameHelper>();
    }

    // Update is called once per frame
    void Update()
    {

    }


}
