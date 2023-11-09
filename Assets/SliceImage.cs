using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceImage : MonoBehaviour
{
    public Texture2D source;
    public Texture2D[] pieces;

    void slice() {
        // splits into 9 even sections
        for( int i = 0; i < 3; i++) // 3 sections x-axis
        {
            for( int j = 0; j < 3; j++) // 3 sections y-axis
            {
                int index = i*3+j;
                pieces[index] = new Texture2D(104,104); // width, height of new image
                var pixels = source.GetPixels(104 * i,104*j , 104,104); // copies the pixels of the source image
                pieces[index].SetPixels(pixels); // sets pixels of new image
                // pieces[index].Apply();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<PsycheImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
