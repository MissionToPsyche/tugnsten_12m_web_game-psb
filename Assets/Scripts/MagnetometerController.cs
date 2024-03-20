using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MagnetometerController : MonoBehaviour
{
    private int numArrows = 3;
    private int numEllipses = 5;
    private int numPoints = 200;
    private Torus torus;
    [SerializeField] private GameObject buttonObj;
    [SerializeField] private GameObject buttonObj2;
    private Vector3 magneticMoment;

    // Start is called before the first frame update
    void Start()
    {
        TorusGenerator torusGenerator = GameObject.Find("TorusGenerator").GetComponent<TorusGenerator>();
        this.torus = torusGenerator.drawTorus(numEllipses, numPoints);
        torus.torusObject.AddComponent<MoveTorus>();
        this.magneticMoment = torus.magneticMoment;

        ArrowGenerator arrowGenerator = GameObject.Find("ArrowGenerator").GetComponent<ArrowGenerator>();
        List<(Vector3, Vector3, Vector3)> fieldPoints = arrowGenerator.getFieldPoints(torus, numPoints, numArrows);
        arrowGenerator.drawArrows(fieldPoints);

        // temporary
        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(grade);
        Button button2 = buttonObj2.GetComponent<Button>();
        button2.onClick.AddListener(noField);
    }

    private void noField()
    {
        float score;

        if(magneticMoment == Vector3.zero)
        {
            score = 10000f;
            Debug.Log("A");
        }
        else
        {
            score = 0f;
            Debug.Log("F");
        }
        Debug.Log("score: " + score);
    }

    private void grade()
    {
        float maxScore = 10000;
        float score;

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

        if(magneticMoment == Vector3.zero)
        {
            float penalty = 0.2f;

            targetScale = new(0, 0, 0);
            scaleMaxDeviation = 3.0f - targetScale.x;
            scale = torus.torusObject.transform.localScale;
            scaleDiff = Mathf.Abs(Vector3.Distance(scale, targetScale));
            scalePercentage = calc(scaleMaxDeviation, scaleDiff);
            
            avgPercentage = scalePercentage - penalty;
            avgPercentage = Mathf.Clamp(avgPercentage, 0, 1);
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

            avgPercentage = (scalePercentage + rotationPercentage) / 2f;
        }

        score = Mathf.RoundToInt(avgPercentage * maxScore);
        Debug.Log("score: " + score);

        if(score > 9000)
        {
            // A
            Debug.Log("A");
        }
        else if(score > 8000)
        {
            // B
            Debug.Log("B");
        }
        else if(score > 6500)
        {
            // C
            Debug.Log("C");
        }
        else if(score > 4000)
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

    private float calc(float maxDeviation, float diff)
    {
        float percentage = (maxDeviation - diff) / maxDeviation;
        return percentage;
    }
}
