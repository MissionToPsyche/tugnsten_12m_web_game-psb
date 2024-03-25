using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MagnetometerController : GameController
{
    private MagnetometerUIController uiController;
    private int numArrows = 3;
    private int numEllipses = 5;
    private int numPoints = 200;
    private Torus torus;
    [SerializeField] private GameObject buttonObj;
    private Vector3 magneticMoment;
    private Vector3 noFieldScale = new Vector3(0.25f, 0.25f, 0.25f);

    override public void InitializeGame()
    {
        uiController = GetComponent<MagnetometerUIController>();
        timer = GameObject.Find("GameTimer").GetComponent<GameTimer>();

        TorusGenerator torusGenerator = GameObject.Find("TorusGenerator").GetComponent<TorusGenerator>();
        this.torus = torusGenerator.drawTorus(numEllipses, numPoints);
        torus.torusObject.AddComponent<MoveTorus>();
        this.magneticMoment = torus.magneticMoment;

        ArrowGenerator arrowGenerator = GameObject.Find("ArrowGenerator").GetComponent<ArrowGenerator>();
        List<(Vector3, Vector3, Vector3)> fieldPoints = arrowGenerator.getFieldPoints(torus, numPoints, numArrows);
        arrowGenerator.drawArrows(fieldPoints);

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
            if (torus.torusObject.transform.localScale.magnitude <= noFieldScale.magnitude)
            {
                uiController.ShowNoFieldMsg();
            }
            if (torus.torusObject.transform.localScale.magnitude > noFieldScale.magnitude)
            {
                uiController.HideNoFieldMsg();
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
        CalcScore();
    }

    override public void CalcScore()
    {
        float rotationWeight = 0.7f;
        float scaleWeight = 1 - rotationWeight;

        Vector3 targetScale;
        float scaleMaxDeviation;
        Vector3 scale;
        float scaleDiff;
        float scalePercentage;

        float targetRotation;
        float rotationMaxDeviation;
        float rotation;
        float rotationDiff;
        float rotationPercentage;

        float avgPercentage;

        if (magneticMoment == Vector3.zero)
        {
            targetScale = noFieldScale;
            scaleMaxDeviation = 3.0f - targetScale.x;
            scale = torus.torusObject.transform.localScale;
            if (scale.magnitude < targetScale.magnitude)
            {
                scale = targetScale;
            }
            scaleDiff = Mathf.Abs(Vector3.Distance(scale, targetScale));
            scalePercentage = calc(scaleMaxDeviation, scaleDiff);

            avgPercentage = Mathf.Clamp(scalePercentage, 0, 1);
        }
        else
        {
            targetScale = new(1, 1, 1);
            scaleMaxDeviation = 3.0f - targetScale.x;
            scale = torus.torusObject.transform.localScale;
            scaleDiff = Mathf.Abs(Vector3.Distance(scale, targetScale));
            scalePercentage = calc(scaleMaxDeviation, scaleDiff);

            targetRotation = 0f;
            rotationMaxDeviation = 180f;
            rotation = torus.torusObject.transform.localRotation.eulerAngles.z;
            rotationDiff = Mathf.Abs(rotation - targetRotation);
            rotationPercentage = calc(rotationMaxDeviation, rotationDiff);

            avgPercentage = (scalePercentage * scaleWeight) + (rotationPercentage * rotationWeight);
        }

        score = Mathf.RoundToInt(avgPercentage * maxScore);
        Debug.Log("score: " + score);
    }

    private float calc(float maxDeviation, float diff)
    {
        float percentage = (maxDeviation - diff) / maxDeviation;
        return percentage;
    }
}
