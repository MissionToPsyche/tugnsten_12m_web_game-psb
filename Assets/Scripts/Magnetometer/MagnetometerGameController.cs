using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MagnetometerGameController : GameController
{
    public MagnetometerUIController ui;
    public TorusGenerator torusGenerator;
    public ArrowGenerator arrowGenerator;
    private int numArrows = 5;
    private int numEllipses = 5; // symmetric on both sides so 5 on each side for 10 total
    private int numPoints = 200;
    private Torus torus;
    private Vector3 magneticMoment;
    private Vector3 noFieldScale = new Vector3(0.25f, 0.25f, 0.25f);
    private MoveTorus moveTorus;

    public bool getGameRunning()
    {
        return gameRunning;
    }

    public void setTorus(Torus torus)
    {
        this.torus = torus;
    }

    override public void InitializeGame()
    {
        ui.SetController(this);
        ui.setIsSubmitted(false);
        SetRightBtn();

        ui.screenUI.GetResetButton().clicked -= initializeGameAction;
        ui.screenUI.GetResetButton().clicked += initializeGameAction;
        ui.screenUI.GetOptionsButton().clicked -= stopGameAction;
        ui.screenUI.GetOptionsButton().clicked += stopGameAction;
        ui.screenUI.getOptionsCloseButton().clicked -= startGameAction; 
        ui.screenUI.getOptionsCloseButton().clicked += startGameAction;
        ui.screenUI.getInfoButton().clicked -= stopGameAction; 
        ui.screenUI.getInfoButton().clicked += stopGameAction; 
        ui.screenUI.getInfoCloseButton().clicked -= startGameAction; 
        ui.screenUI.getInfoCloseButton().clicked += startGameAction; 

        GameObject[] gos = GameObject.FindGameObjectsWithTag("destroyOnReset");
        foreach(GameObject go in gos)
        {
            Destroy(go);
        }

        this.torus = torusGenerator.drawTorus(numEllipses, numPoints);
        // torus.torusObject.AddComponent<MoveTorus>();
        this.magneticMoment = torus.magneticMoment;

        this.moveTorus = torus.torusObject.GetComponent<MoveTorus>();

        List<(Vector3, Vector3, Vector3)> fieldPoints = arrowGenerator.getFieldPoints(torus, numPoints, numArrows);
        arrowGenerator.drawArrows(fieldPoints);

        score = -1;

        timer.resetTimer();
        ui.ResetUI();
    }

    void Update()
    {
        ui.ShowTime(timer.getTime());
        if(gameRunning)
        {
            if (torus.torusObject.transform.localScale.magnitude <= noFieldScale.magnitude)
            {
                ui.ShowNoFieldMsg();
            }
            if (torus.torusObject.transform.localScale.magnitude > noFieldScale.magnitude)
            {
                ui.HideNoFieldMsg();
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
        StopGame();
        moveTorus.uiFlag = true;
        ui.screenUI.getInfoCloseButton().clicked -= moveTorus.moveAction; 
        ui.screenUI.getOptionsCloseButton().clicked -= moveTorus.moveAction; 
        ui.screenUI.getOptionsCloseButton().clicked -= startGameAction;  
        ui.screenUI.getInfoCloseButton().clicked -= startGameAction; 
        ui.ShowScore(GetScore(), GetGrade());
        scorecard.MagnetometerScore = score;
    }

    override public void CalcScore()
    {
        float rotationWeight = 0.65f;
        float timeWeight = 0.1f;
        float scaleWeight = 1 - rotationWeight - timeWeight;

        Vector3 targetScale;
        Vector3 maxScale = new Vector3(3, 3, 3);
        float scaleMaxDeviation;
        Vector3 scale;
        float scaleDiff;
        float scalePercentage;

        float rotationMaxDeviation;
        float rotation;
        float rotationDiff;
        float rotationPercentage;

        float avgPercentage;

        float timePercentage = CalcTimePercent(timer.getTime(), 90, 3);

        if (magneticMoment == Vector3.zero)
        {
            rotationWeight = 0f;
            scaleWeight = 1 - rotationWeight - timeWeight;

            targetScale = noFieldScale;
            scaleMaxDeviation = Mathf.Abs(Vector3.Distance(maxScale, targetScale));
            scale = torus.torusObject.transform.localScale;
            if (scale.magnitude < targetScale.magnitude)
            {
                scale = targetScale;
            }
            scaleDiff = Mathf.Abs(Vector3.Distance(scale, targetScale));
            // Debug.Log("zero");
            scalePercentage = calc(scaleMaxDeviation, scaleDiff);

            avgPercentage = (scalePercentage * scaleWeight) + (timePercentage * timeWeight);
        }
        else
        {
            targetScale = new Vector3(1, 1, 1);
            scaleMaxDeviation = Mathf.Abs(Vector3.Distance(maxScale, targetScale));
            scale = torus.torusObject.transform.localScale;
            scaleDiff = Mathf.Abs(Vector3.Distance(scale, targetScale));
            scalePercentage = calc(scaleMaxDeviation, scaleDiff);

            rotationMaxDeviation = 180f;
            rotation = torus.torusObject.transform.localRotation.eulerAngles.z;
            rotationDiff = Mathf.Abs(rotation - rotationMaxDeviation);
            rotationDiff = rotationMaxDeviation - rotationDiff;
            rotationPercentage = calc(rotationMaxDeviation, rotationDiff);

            if(scale.magnitude < noFieldScale.magnitude) {
                rotationWeight = 0f;
                scaleWeight = 1 - rotationWeight - timeWeight;
            }

            avgPercentage = (scalePercentage * scaleWeight) + (rotationPercentage * rotationWeight) + (timePercentage * timeWeight);
        }

        score = Mathf.RoundToInt(avgPercentage * maxScore);
    }

    private float calc(float maxDeviation, float diff)
    {
        float percentage = (maxDeviation - diff) / maxDeviation;
        return percentage;
    }

    override public void SetRightBtn()
    {
        ui.screenUI.GetContinueButton().text = "Submit";
        ui.screenUI.GetContinueButton().clicked -= ui.rightBtnListenerAction; // Prevents multiple listeners
        ui.screenUI.GetContinueButton().clicked += ui.rightBtnListenerAction;
    }

}
