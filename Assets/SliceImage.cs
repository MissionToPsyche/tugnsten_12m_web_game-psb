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

    public void slice() {
        float fullWidth = originalImage.width;
        float fullHeight = originalImage.height;
        int count = 1;

        for(int i = 0; i < 3; i++) 
        {
            for(int j = 0; j < 3; j++)
            {
                GameObject imgObject = new GameObject();
                imgObject.name = "img"+ count;

                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvas.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2((fullWidth/3 + 20)*i, (fullHeight/3 + 20)*j); // setting position, will be on center of canvas
                trans.sizeDelta = new Vector2(fullWidth/3, fullHeight/3); // set the size

                Image image = imgObject.AddComponent<Image>();

                // sets the texture of the sprite to a rect section of the originalImage and specifies the center of the new image
                image.sprite = Sprite.Create(originalImage, new Rect(fullWidth/3*i, fullHeight/3*j, fullWidth/3, fullHeight/3), new Vector2(0.5f, 0.5f));
                
                imgObject.transform.SetParent(canvas.transform); // sets the parent

                count++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        bytes = File.ReadAllBytes(path);
        originalImage = new Texture2D(1,1); // size will be replaced by image size
        originalImage.LoadImage(bytes); // create a texture2d asset from the image
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
