using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellipse
{
    public float semiMajorAxis { get; }
    public float semiMinorAxis { get; }
    public Vector3 center { get; }
    public GameObject lineObject { get; set; }
    public List<(int, Vector3)> usablePoints { get; set; }
    public Ellipse(float semiMajorAxis, float semiMinorAxis, Vector3 center)
    {
        this.semiMajorAxis = semiMajorAxis;
        this.semiMinorAxis = semiMinorAxis;
        this.center = center;
    }
}
