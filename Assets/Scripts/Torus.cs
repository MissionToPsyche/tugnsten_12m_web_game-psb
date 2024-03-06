using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torus
{
    private List<Ellipse> ellipses;
    public Vector3 magneticMoment { get; set; }
    public GameObject torusObject { get; set; }
    public Torus()
    {
        this.ellipses = new List<Ellipse>();
    }
    public List<Ellipse> getEllipses()
    {
        return ellipses;
    }
}
