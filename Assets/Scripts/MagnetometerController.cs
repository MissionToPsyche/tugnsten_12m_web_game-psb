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

    // Start is called before the first frame update
    void Start()
    {
        TorusGenerator torusGenerator = GameObject.Find("TorusGenerator").GetComponent<TorusGenerator>();
        this.torus = torusGenerator.drawTorus(numEllipses, numPoints);
        torus.torusObject.AddComponent<MoveTorus>();

        ArrowGenerator arrowGenerator = GameObject.Find("ArrowGenerator").GetComponent<ArrowGenerator>();
        List<(Vector3, Vector3, Vector3)> fieldPoints = arrowGenerator.getFieldPoints(torus, numPoints, numArrows);
        arrowGenerator.drawArrows(fieldPoints);

        // temporary
        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(checkCorrectness);
    }


    private void checkCorrectness()
    {
        float targetRotation = 0f;
        Vector3 targetScale = new(1, 1, 1);

        float scaleMargin = 0.15f;
        float rotationMargin = 10f;

        float rotation = torus.torusObject.transform.localRotation.eulerAngles.z;
        float rotation180 = rotation - Mathf.Sign(rotation) * 180;
        Vector3 scale = torus.torusObject.transform.localScale;

        float diff1 = Mathf.Abs(rotation - targetRotation);
        float diff2 = Mathf.Abs(rotation180 - targetRotation);

        if ((diff1 < rotationMargin || diff2 < rotationMargin) && Mathf.Abs(Vector3.Distance(scale, targetScale)) < scaleMargin)
        {
            Debug.Log("yay!");
            // return true;
        }
        else
        {
            Debug.Log("boo");
            // return false;
        }
    }
}
