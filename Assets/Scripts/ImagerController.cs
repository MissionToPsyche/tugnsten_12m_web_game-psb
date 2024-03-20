using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ImagerController : MonoBehaviour
{
    private List<GameObject> images;
    private Timer timer;

    void Start()
    {
        SliceImage sliceImage = GameObject.Find("GenImgSlices").GetComponent<SliceImage>();
        sliceImage.slice(); // generate and display images
        images = sliceImage.getImages();

        timer = GameObject.Find("Timer").GetComponent<Timer>();
        timer.startTimer();
        // StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        timer.startTimer();
        yield return new WaitForSeconds(5);
        timer.stopTimer();
        yield return new WaitForSeconds(5);
        timer.startTimer();
        yield return new WaitForSeconds(5);
        timer.clearTimer();
    }

    // TODO: maybe move out of the controller class
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

    private void grade()
    {
        // TODO: consolodate maxScore to a single file
        float maxScore = 10000;
        float excellentTime = 4.0f;
        float lowTime = 40f;
        float diff = lowTime - excellentTime;
        float score;

        float time = timer.getTime();

        if(time < excellentTime)
        {
            score = maxScore;
        }
        else
        {
            // Calculate the normalized time (0 to 1)
            float normalizedTime = Mathf.Clamp01((time - excellentTime) / diff);

            // Map the normalized time to a score between 10000 and 0
            score = Mathf.RoundToInt(maxScore - (normalizedTime * maxScore));
        }
        Debug.Log("score: " + score);
    }

}
