using System.Collections.Generic;
using UnityEngine;

public class ImagerGameHelper : MonoBehaviour
{
    private List<GameObject> images;

    void Start()
    {
        SliceImage sliceImage = GameObject.Find("GenImgSlices").GetComponent<SliceImage>();
        sliceImage.slice(); // generate and display images
        images = sliceImage.getImages();
    }

    public void updateSnapPositions(GameObject imageMoved)
    {
        foreach (GameObject img in images)
        {
            if(img != imageMoved)
            {
                img.GetComponent<ImageController>().updateSnapPoint(imageMoved.name, imageMoved.GetComponent<RectTransform>().anchoredPosition);
            }
        }
    }

}
