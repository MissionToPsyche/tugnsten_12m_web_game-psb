using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Orbit : MonoBehaviour
{
    public PointMass parent;
    public LineRenderer lr;

    // TODO: Add constructor with peri/apo and rotation

    public void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
}
