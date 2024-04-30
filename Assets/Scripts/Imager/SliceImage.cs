using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SliceImage : MonoBehaviour
{
    // SerializeField makes this variable visible in unity editor but cannot be accessed by other scripts (unlike public variables)
    [SerializeField] Canvas canvas;
    private Texture2D originalImage;
    private string path;
    private byte[] bytes;
    private List<Vector2> starts = new List<Vector2>();
    private List<GameObject> images = new List<GameObject>();
    private float displaySize = 300f;


    public void setOriginalImage(Texture2D tex)
    {
        originalImage = tex;
    }
    public Texture2D getOriginalImage()
    {
        return originalImage;
    }
    public void setCanvas(Canvas canvas)
    {
        this.canvas = canvas;
    }
    public Canvas getCanvas()
    {
        return canvas;
    }
    public void setStarts(List<Vector2> starts)
    {
        this.starts = starts;
    }
    public List<Vector2> getStarts()
    {
        return starts;
    }
    public void setImages(List<GameObject> imgs)
    {
        images = imgs;
    }
    public List<GameObject> getImages()
    {
        return images;
    }


    public void slice()
    {
        GetTexture();

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
        int numImgs = 6;
        float imgSize = 0.4f;
        float imgWidth = slicedWidth * imgSize;
        float imgHeight = slicedHeight * imgSize;

        // overlap
        float minOverlap = 0.8f;
        float yOverlap = imgHeight * minOverlap;
        float xOverlap = imgWidth * minOverlap;
        Vector2 overlap = new Vector2(xOverlap, yOverlap);

        // difference
        float maxOverlap = 0.3f;
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

                // TODO: write code to use most different image
                if (ctr > tries)
                {
                    break;
                }

            } while (!isStartDifferent(start, diff) && !isOverlap(start, overlap));

            starts.Add(start);
            cells.RemoveAt(randomCell);

            GameObject imgObject = createImageObject(imgWidth, imgHeight, start, slicedTexture, imgNum);

            images.Add(imgObject);
        }

        // add snap offsets to each image
        addSnapOffsets(imgWidth, imgHeight);
        setInitialSnapPositions();
    }

    public void GetTexture()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            // Access the Sprite from the SpriteRenderer
            Sprite sprite = spriteRenderer.sprite;

            // Access the Texture2D associated with the Sprite
            originalImage = sprite.texture;
        }
        else
        {
            Debug.LogError("SpriteRenderer or Sprite not found!");
        }
    }

    // checks if the small images are different enough
    public bool isStartDifferent(Vector2 newStart, Vector2 diff)
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

    // checks if the small images overlap enough
    public bool isOverlap(Vector2 newStart, Vector2 overlap)
    {
        foreach (Vector2 start in starts)
        {
            if (Mathf.Abs(start.x - newStart.x) < overlap.x && Mathf.Abs(start.y - newStart.y) < overlap.y)
            {
                return true;
            }
        }
        return false;
    }

    public Texture2D createSectionOfOriginal()
    {
        float originalWidth = originalImage.width;
        float originalHeight = originalImage.height;

        float sectionSize = 0.5f;
        // TODO: separate x vs y edgeAdjustment for left side ?? 
        float edgeAdjustment = 1 / 3f; // left side
        float sectionWidth = originalWidth * sectionSize;
        float sectionHeight = originalHeight * sectionSize;
        // TODO: multiple section width/height by a right edge adjustment ?? V
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
        int separation = 100;

        List<Vector2> initialPositions = new List<Vector2>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                initialPositions.Add(new Vector2(-600f + ((displaySize + separation) * j), 253f - ((displaySize + separation) * i)));
            }
        }

        GameObject imgObject = new GameObject();
        imgObject.name = "img" + imgNum;
        imgObject.tag = "destroyOnReset";

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(canvas.transform); // setting parent
        trans.localScale = Vector3.one;
        trans.pivot = new Vector2(0.5f, 0.5f);
        trans.anchoredPosition = new Vector2(initialPositions[imgNum].x, initialPositions[imgNum].y); // setting position
        trans.sizeDelta = new Vector2(imgWidth, imgHeight);

        // adding canvas group component
        CanvasGroup group = imgObject.AddComponent<CanvasGroup>();
        group.alpha = 1;
        group.blocksRaycasts = true;

        // adding image component
        Image image = imgObject.AddComponent<Image>();

        image.preserveAspect = true;
        image.rectTransform.pivot = new Vector2(0.5f, 0.5f);

        Vector2 imgSize = new Vector2(imgWidth, imgHeight);

        // sets the texture of the sprite to a section of the slicedImage and specifies the center of the new image
        image.sprite = Sprite.Create(slicedTexture, new Rect(start, imgSize), new Vector2(0.5f, 0.5f));

        imgObject.transform.SetParent(canvas.transform); // sets the parent


        imgObject.AddComponent<ImageController>();
        imgObject.AddComponent<Draggable>(); // adding script to drag image

        // adding script to snap image
        imgObject.AddComponent<SnapToTarget>(); // need to take this snapToTarget and setTargetPosition in imageGameHelper

        return imgObject;
    }

    // set the snap offsets for all images
    public void addSnapOffsets(float imgWidth, float imgHeight)
    {
        for (int i = 0; i < images.Count; i++)
        {
            Dictionary<string, Vector2> snapOffsets = new Dictionary<string, Vector2>();
            for (int j = 0; j < images.Count; j++)
            {
                if (images[j] != images[i])
                {
                    float widthOffset = starts[i].x - starts[j].x;
                    float heightOffset = starts[i].y - starts[j].y;
                    if (Mathf.Abs(widthOffset) < imgWidth && Mathf.Abs(heightOffset) < imgHeight)
                    {
                        Vector2 offset = new Vector2(widthOffset, heightOffset);
                        snapOffsets.Add(images[j].name, offset);
                    }
                }
            }
            images[i].GetComponent<ImageController>().setSnapOffsets(snapOffsets); // set offsets for snapping
        }
    }

    // set the snap positions for all images
    public void setInitialSnapPositions()
    {
        foreach (GameObject image in images)
        {
            image.GetComponent<ImageController>().setSnapPoints(images);
        }
    }
}
