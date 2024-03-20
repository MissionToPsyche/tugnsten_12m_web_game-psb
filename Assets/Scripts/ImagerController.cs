using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

public class ImagerController : GameController
{
    private List<GameObject> images;
    private Timer timer;
    [SerializeField] private TMP_Text text;
    // private bool gameRunning = false;
    [SerializeField] private GameObject buttonObj;

    override public void InitializeGame()
    {
        SliceImage sliceImage = GameObject.Find("GenImgSlices").GetComponent<SliceImage>();
        sliceImage.slice(); // generate and display images
        images = sliceImage.getImages();

        timer = GameObject.Find("Timer").GetComponent<Timer>();

        // temporary
        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(FinishGame);

        gameRunning = true;
        timer.startTimer();
        StartCoroutine(updateTimer());
    }

    private IEnumerator updateTimer()
    {
        while(gameRunning)
        {
            TimeSpan time = TimeSpan.FromSeconds(timer.getTime());
            text.text = time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2") + ":" + time.Milliseconds.ToString("D3");
            
            yield return null; // Wait for the next frame (makes like Update)
        }
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

    override public bool CheckWin()
    {

        return false;
    }

    override public void FinishGame()
    {
        timer.stopTimer();
        gameRunning = false;
    }

    override public int GetScore()
    {
        float excellentTime = 4.0f;
        float lowTime = 100f;
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

        return (int)score;

        // TODO: move this to a single file
        if (score > 9000)
        {
            // A
            Debug.Log("A");
        }
        else if (score > 8000)
        {
            // B
            Debug.Log("B");
        }
        else if (score > 6500)
        {
            // C
            Debug.Log("C");
        }
        else if (score > 4000)
        {
            // D
            Debug.Log("D");
        }
        else
        {
            // F
            Debug.Log("F");
        }
    }

}
