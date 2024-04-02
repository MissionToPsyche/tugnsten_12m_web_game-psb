using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

public class ImagerGameController : GameController
{
    public ImagerUIController uiController;
    public SliceImage sliceImage;
    private List<GameObject> images;

    override public void InitializeGame()
    {
        uiController = GameObject.Find("Main UI Canvas").GetComponent<ImagerUIController>();
        uiController.SetController(this);
        uiController.screenUI = GameObject.Find("UIDocument").GetComponent<GameScreenUI>();
        SetRightBtn();

        sliceImage = GameObject.Find("Data Generator").GetComponent<SliceImage>();
        sliceImage.slice(); // generate and display images
        images = sliceImage.getImages();

        score = -1;

        uiController.ResetUI();
        StartGame();
    }

    void Update()
    {
        uiController.ShowTime(timer.getTime());
        if(gameRunning)
        {
            if(checkIsDone())
            {
                FinishGame();
            }
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

    public bool checkIsDone()
    {
        foreach (GameObject img in images)
        {
            if(!isAllSnapPointsEqual(img))
            {
                return false;
            }
        }
        return true;
    }

    private bool isAllSnapPointsEqual(GameObject img)
    {
        const float tolerance = 0.001f;
        Vector2 minPoint = Vector2.positiveInfinity;
        Vector2 maxPoint = Vector2.negativeInfinity;

        // Assuming getSnapPoints() is modified to return all snapPoints across all images
        Dictionary<string, Vector2>.ValueCollection snapPoints = img.GetComponent<ImageController>().getSnapPoints();

        foreach (Vector2 snapPoint in snapPoints)
        {
            minPoint = Vector2.Min(minPoint, snapPoint);
            maxPoint = Vector2.Max(maxPoint, snapPoint);
        }

        // Check if the bounding box defined by minPoint and maxPoint is within the tolerance
        bool isWithinTolerance = (maxPoint - minPoint).magnitude <= tolerance;

        return isWithinTolerance;
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
        CalcScore();
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

    override public void SetRightBtn()
    {
        uiController.screenUI.getContinueButton().text = "Continue";
        uiController.screenUI.getContinueButton().SetEnabled(false);
        uiController.screenUI.getContinueButton().clicked += uiController.RightBtnListener;
    }

}
