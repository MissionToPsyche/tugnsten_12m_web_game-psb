using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellipse : MonoBehaviour
{
    public float semiMajorAxis { get; }
    public float semiMinorAxis { get; }
    public Vector3 center { get; }
    public GameObject lineObject { get; set; }
    public Ellipse(float semiMajorAxis, float semiMinorAxis, Vector3 center)
    {
        this.semiMajorAxis = semiMajorAxis;
        this.semiMinorAxis = semiMinorAxis;
        this.center = center;
    }
}
