using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

public class ImagerGameController : GameController
{
    public ImagerUIController ui;
    public SliceImage sliceImage;
    public List<GameObject> images {get; set; }

    override public void InitializeGame()
    {
        ui.SetController(this);
        SetRightBtn();
        ui.screenUI.getResetButton().clicked -= () => { InitializeGame(); };
        ui.screenUI.getResetButton().clicked += () => { InitializeGame(); };

        GameObject[] gos = GameObject.FindGameObjectsWithTag("destroyOnReset");
        foreach(GameObject go in gos)
        {
            Destroy(go);
        }

        sliceImage.slice(); // generate and display images
        images = sliceImage.getImages();

        score = -1;

        ui.ResetUI();
        StopGame();
        StartGame();  // TODO: MOVE OUT AND CONNECT TO UI BUTTONS
        // TODO: delete images when resetting
    }

    void Update()
    {
        ui.ShowTime(timer.getTime());
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
        // TODO: maybe don't need the foreach loop
        foreach (GameObject img in images)
        {
            if(!isAllSnapPointsEqual(img))
            {
                return false;
            }
        }
        return true;
    }

    public bool isAllSnapPointsEqual(GameObject img)
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
        StopGame();
        ui.ShowScore(GetScore(), GetGrade());
        scorecard.ImagerScore = score;
        ui.setIsSubmitted(true);
        ui.screenUI.getContinueButton().SetEnabled(true);
    }

    override public void CalcScore()
    {
        float timePercent = CalcTimePercent(timer.getTime(), 100, 4);    
        score = Mathf.RoundToInt(maxScore - (timePercent * maxScore));
    }

    override public void SetRightBtn()
    {
        ui.screenUI.getContinueButton().text = "Continue";
        ui.screenUI.getContinueButton().SetEnabled(false);
        ui.screenUI.getContinueButton().clicked -= ui.RightBtnListener; // Prevents multiple listeners
        ui.screenUI.getContinueButton().clicked += ui.RightBtnListener;
    }

}
