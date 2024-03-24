using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

public class ImagerController : GameController
{
    private ImagerUIController uiController;
    private List<GameObject> images;

    // temporary
    [SerializeField] private GameObject buttonObj;

    override public void InitializeGame()
    {
        uiController = GetComponent<ImagerUIController>();
        timer = GameObject.Find("GameTimer").GetComponent<GameTimer>();

        SliceImage sliceImage = GameObject.Find("GenImgSlices").GetComponent<SliceImage>();
        sliceImage.slice(); // generate and display images
        images = sliceImage.getImages();

        // temporary
        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(FinishGame);

        StartGame();
    }

    void Update()
    {
        uiController.ShowTime(timer.getTime());
        if(gameRunning)
        {
            
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

    override public void StartGame()
    {
        gameRunning = true;
        timer.startTimer();
    }

    override public void StopGame()
    {
        gameRunning = false;
        timer.stopTimer();
    }

    override public void FinishGame()
    {
        gameRunning = false;
        timer.stopTimer();
    }

    override public void CalcScore()
    {
        float excellentTime = 4.0f;
        float lowTime = 100f;
        float diff = lowTime - excellentTime;

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
