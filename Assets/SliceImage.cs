using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SliceImage : MonoBehaviour
{
    private Texture2D originalImage;
    [SerializeField] Canvas canvas; // SerializeField makes this variable visible in unity editor but cannot be accessed by other scripts (unlike public variables)
    private string path;
    private byte[] bytes;
    private List<Vector2> starts;
    private List<GameObject> images;

    public void slice() 
    {
        // can probably take out (just here to reset for testing)
        foreach(GameObject obj in images)
        {
            Destroy(obj);
        }
        images.Clear();
        starts.Clear();


        Texture2D slicedTexture = createSectionOfOriginal();

        float slicedWidth = slicedTexture.width;
        float slicedHeight = slicedTexture.height;

        // image
        // int numImgs = 4;
        // float imgSize = 0.5f;
        // int numImgs = 5;
        // float imgSize = 0.5f;
        int numImgs = 6;
        float imgSize = 0.4f;
        // int numImgs = 7;
        // float imgSize = 0.4f;
        float imgWidth = slicedWidth*imgSize;
        float imgHeight = slicedHeight*imgSize;

        // overlap
        float minOverlap = 0.8f;
        float yOverlap = imgHeight*minOverlap;
        float xOverlap = imgWidth*minOverlap;
        Vector2 overlap = new Vector2(xOverlap, yOverlap);

        // difference
        float maxOverlap = 0.3f;
        float yDiff = imgHeight*maxOverlap;
        float xDiff = imgWidth*maxOverlap;
        Vector2 diff = new Vector2(xDiff, yDiff);

        // find a different img
        int tries = 10;
        float spaceX = slicedWidth/tries;
        float spaceY = slicedHeight/tries;
        float space = Mathf.Min(spaceX, spaceY);
        int gridSize = 6;
        List<int[]> cells = new List<int[]>();
        for(int i = 0; i < Mathf.CeilToInt(gridSize*imgSize); i++)
        {
            for(int j = 0; j < Mathf.CeilToInt(gridSize*imgSize); j++)
            {
                cells.Add(new int[] {i, j});
            }
        }


        // creates images
        for(int imgNum = 0; imgNum < numImgs; imgNum++) 
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
                Debug.Log("do: " + ctr);

                int[] cell = cells[randomCell];
                int cellX = cell[0];
                int cellY = cell[1];
                float cellWidth = slicedWidth / gridSize;
                float cellHeight = slicedHeight / gridSize;

                // get a start point in the cell
                float adjustedCellWidth = cellWidth/space;
                float adjustedCellHeight = cellHeight/space;
                float randX = Random.Range(0, (int)adjustedCellWidth) * space;
                float randY = Random.Range(0, (int)adjustedCellHeight) * space;
                imgXStart = cellX * cellWidth + randX;
                imgYStart = cellY * cellHeight + randY;
                start = new Vector2(imgXStart, imgYStart);

                // TO DO: write code to use most different image
                if(ctr > tries)
                {
                    break;
                }

            }while(!isStartDifferent(start, diff) && !isOverlap(start, overlap));

            starts.Add(start);
            cells.RemoveAt(randomCell);

            GameObject imgObject = createImageObject(imgWidth, imgHeight, start, slicedTexture, imgNum);

            images.Add(imgObject);
        }
    }

    private bool isStartDifferent(Vector2 newStart, Vector2 diff)
    {
        foreach(Vector2 start in starts)
        {
            if (Mathf.Abs(start.x - newStart.x) < diff.x && Mathf.Abs(start.y - newStart.y) < diff.y)
            {
                return false;
            }
        }
        return true;
    }

    private bool isOverlap(Vector2 newStart, Vector2 overlap)
    {
        foreach(Vector2 start in starts)
        {
            if (Mathf.Abs(start.x - newStart.x) > overlap.x && Mathf.Abs(start.y - newStart.y) > overlap.y)
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
        float edgeAdjustment = 1/3f; // left side
        float sectionWidth = originalWidth*sectionSize;
        float sectionHeight = originalHeight*sectionSize;
        // TO DO: multiple section width/height by a right edge adjustment ?? V
        float sectionXStart = Random.Range(originalWidth*edgeAdjustment, originalWidth-sectionWidth);
        float sectionYStart = Random.Range(originalHeight*edgeAdjustment, originalHeight-sectionHeight);

        Rect regionToCopy = new Rect(sectionXStart, sectionYStart, sectionWidth, sectionHeight); 
        Texture2D slicedTexture = new Texture2D((int)regionToCopy.width, (int)regionToCopy.height); // Create a new Texture2D to store the sliced part
        // Get the pixels from the original texture within the specified region
        Color[] pixels = originalImage.GetPixels((int)regionToCopy.x, (int)regionToCopy.y, (int)regionToCopy.width, (int)regionToCopy.height);
        slicedTexture.SetPixels(pixels); // Set the pixels in the new texture
        slicedTexture.Apply(); // Apply changes to the new texture

        return slicedTexture;
    }

    private GameObject createImageObject(float imgWidth, float imgHeight, Vector2 start, Texture2D slicedTexture, int imgNum)
    {

        float displaySize = 300f;
        int separation = 100;

        List<Vector2> initialPositions = new List<Vector2>();
        for(int i = 0; i < 2; i ++)
        {
            for(int j = 0; j < 4; j++)
            {
                initialPositions.Add(new Vector2(300f + (displaySize + separation) * j, 900f - (displaySize + separation) * i));
            }
        }
        

        GameObject imgObject = new GameObject();
        imgObject.name = "img"+ imgNum;

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(canvas.transform); // setting parent
        trans.localScale = Vector3.one;
        // TO DO: position still needs to be determined V
        // trans.anchoredPosition = new Vector2((imgWidth + 20)*imgNum, (imgHeight + 20)*imgNum); // setting position
        // trans.sizeDelta = new Vector2(imgWidth, imgHeight); // set the size
        trans.anchoredPosition = new Vector2(initialPositions[imgNum].x, initialPositions[imgNum].y); // setting position
        trans.sizeDelta = new Vector2(displaySize, displaySize); // set the size of the image/gameobject

        // adding canvas group component
         CanvasGroup group = imgObject.AddComponent<CanvasGroup>();
         group.alpha = 1;
         group.blocksRaycasts = true;

        // add script component
        // ScrptName script = imgObject.AddComponent<ScrptName>();

        // adding image component
        Image image = imgObject.AddComponent<Image>();

        Vector2 imgSize = new Vector2(imgWidth, imgHeight);
        // sets the texture of the sprite to a section of the slicedImage and specifies the center of the new image
        image.sprite = Sprite.Create(slicedTexture, new Rect(start, imgSize), new Vector2(0.5f, 0.5f));
        
        imgObject.transform.SetParent(canvas.transform); // sets the parent

        return imgObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        bytes = File.ReadAllBytes(path);
        originalImage = new Texture2D(1,1); // size will be replaced by image size
        originalImage.LoadImage(bytes); // create a texture2d asset from the image

        starts = new List<Vector2>();
        images = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
