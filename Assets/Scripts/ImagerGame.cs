using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ImagerGame : MonoBehaviour
{
    private string path;
    private byte[] bytes;

    private Texture2D originalImage;
    private List<GameObject> Images;
    private SliceImage slice;

    private Canvas canvas;

    public void sliceImages()
    {
        // slice = new SliceImage();
        slice.slice();
    }

    void Start()
    {
        path = "Assets/Psyche_Mission_RubinAsteroid_171203.png";
        bytes = File.ReadAllBytes(path);
        originalImage = new Texture2D(1, 1); // size will be replaced by image size
        originalImage.LoadImage(bytes); // create a texture2d asset from the image
    }

    void Update()
    {

    }
}