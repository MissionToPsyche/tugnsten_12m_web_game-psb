using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SliceImage : MonoBehaviour
{
    private Texture2D originalImage;
    private Texture2D[] pieces;
    private Image psyche;
    [SerializeField] Canvas canvas; // SerializeField makes this variable visible in unity editor but cannot be accessed by other scripts (unlike public variables)
    private string path;
    private byte[] bytes;

    public void slice() {
        // GameObject NewObj = new GameObject(); //Create the GameObject
        // Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
        // NewImage.sprite = currentSprite; //Set the Sprite of the Image Component on the new GameObject
        // NewObj.GetComponent<RectTransform>().SetParent(ParentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        // NewObj.SetActive(true); //Activate the GameObject


        // string path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        // byte[] bytes = File.ReadAllBytes(path);
        // originalImage = new Texture2D(1,1); // size will be replaced by image size
        // originalImage.LoadImage(bytes); // create a texture2d asset from the image


        GameObject imgObject = new GameObject();

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(canvas.transform); // setting parent
        trans.localScale = Vector3.one;
        trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center of canvas
        // trans.sizeDelta= new Vector2(150, 200); // custom size

        Image image = imgObject.AddComponent<Image>();
        // Texture2D tex = Resources.Load<Texture2D>("red");
        // sets the texture of the sprite to a rect section of the originalImage and specifies the center of the new image
        image.sprite = Sprite.Create(originalImage, new Rect(0, 0, originalImage.width/3, originalImage.height/3), new Vector2(0.5f, 0.5f));
        imgObject.transform.SetParent(canvas.transform); // sets the parent



        // string path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        // byte[] bytes = File.ReadAllBytes(path);
        // // Texture2D loadTexture = new Texture2D(1,1); // size will be replaced by image size
        // // loadTexture.LoadImage(bytes); // create a texture2d asset from the image
        // originalImage = new Texture2D(1,1);
        // originalImage.LoadImage(bytes);
        // int index = 0;

        // // splits into 9 even sections
        // for( int i = 0; i < 3; i++) // 3 sections x-axis
        // {
        //     for( int j = 0; j < 3; j++) // 3 sections y-axis
        //     {
        //         index = i*3+j;
        //         pieces[index] = new Texture2D(originalImage.width/3, originalImage.height/3); // width, height of new image
        //         Color[] pixels = originalImage.GetPixels(originalImage.width/3 * i, originalImage.height/3*j , originalImage.width/3, originalImage.height/3); // copies the pixels of the source image
        //         pieces[index].SetPixels(pixels); // sets pixels of new image
        //         pieces[index].Apply(); // allows the new image to be rendered (applies to from the CPU to the GPU)
        //     }
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        // psyche = GetComponent<Image>();
        // canvas = GetComponent<Canvas>();
        path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        bytes = File.ReadAllBytes(path);
        originalImage = new Texture2D(1,1); // size will be replaced by image size
        originalImage.LoadImage(bytes); // create a texture2d asset from the image


        // string path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        // byte[] bytes = File.ReadAllBytes(path);
        // // Texture2D loadTexture = new Texture2D(1,1); // size will be replaced by image size
        // // loadTexture.LoadImage(bytes); // create a texture2d asset from the image
        // originalImage = new Texture2D(1,1);
        // originalImage.LoadImage(bytes);
        // int index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
