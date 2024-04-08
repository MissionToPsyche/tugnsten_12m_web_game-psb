using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public float xRotationRate = 1;
    public float yRotationRate = 1;
    public float zRotationRate = 1;

    void Update()
    {
        float xRotationAmount = xRotationRate * Time.deltaTime;
        float yRotationAmount = yRotationRate * Time.deltaTime;
        float zRotationAmount = zRotationRate * Time.deltaTime;

        transform.Rotate(xRotationAmount, yRotationAmount, zRotationAmount);
    }
}
