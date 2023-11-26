using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OrbitTests
{
    private GameObject GetDummyOrbit()
    {
        GameObject gameOb = new();
        gameOb.AddComponent<Orbit>();
        
        GameObject dummyMarker = new();
        dummyMarker.AddComponent<SpriteRenderer>();

        gameOb.GetComponent<Orbit>().warningMarker = dummyMarker;
        gameOb.GetComponent<Orbit>().apoapsisMarker = dummyMarker;
        gameOb.GetComponent<Orbit>().periapsisMarker = dummyMarker;
        
        GameObject parent = new();
        parent.AddComponent<PointMass>();
        parent.transform.position = new(0, 0, 0);
        gameOb.GetComponent<Orbit>().parent = parent.GetComponent<PointMass>();

        return gameOb;
    }

    [Test]
    public void BinarySearchMin()
    {
        Vector3[] points = new Vector3[] {
            new(1.0f, 2.0f, 3.0f), // Min
            new(1.5f, 2.5f, 3.5f),
            new(2.0f, 3.0f, 4.0f),
            new(3.0f, 4.0f, 5.0f),
            new(4.0f, 5.0f, 6.0f),
            new(5.0f, 6.0f, 7.0f),
            new(6.0f, 7.0f, 8.0f), 
            new(7.0f, 8.0f, 9.0f),
            new(8.0f, 9.0f, 10.0f),
            new(9.0f, 10.0f, 11.0f), 
            new(10.0f, 11.0f, 12.0f), // Max
            new(9.5f, 10.5f, 11.5f),
            new(9.0f, 10.0f, 11.0f),
            new(8.0f, 9.0f, 10.0f),
            new(7.0f, 8.0f, 9.0f),
            new(6.0f, 7.0f, 8.0f),
            new(5.0f, 6.0f, 7.0f),
            new(4.0f, 5.0f, 6.0f), 
            new(3.0f, 4.0f, 5.0f),
            new(1.5f, 2.5f, 3.5f)
        };

        GameObject dummyOrbit = GetDummyOrbit();

        Assert.AreEqual(points[0], dummyOrbit.GetComponent<Orbit>().BinarySearchMin(points));
    }

    [Test]
    public void BinarySearchMax()
    {
        Vector3[] points = new Vector3[] {
            new(1.0f, 2.0f, 3.0f), // Min
            new(1.5f, 2.5f, 3.5f),
            new(2.0f, 3.0f, 4.0f),
            new(3.0f, 4.0f, 5.0f),
            new(4.0f, 5.0f, 6.0f),
            new(5.0f, 6.0f, 7.0f),
            new(6.0f, 7.0f, 8.0f), 
            new(7.0f, 8.0f, 9.0f),
            new(8.0f, 9.0f, 10.0f),
            new(9.0f, 10.0f, 11.0f), 
            new(10.0f, 11.0f, 12.0f), // Max
            new(9.5f, 10.5f, 11.5f),
            new(9.0f, 10.0f, 11.0f),
            new(8.0f, 9.0f, 10.0f),
            new(7.0f, 8.0f, 9.0f),
            new(6.0f, 7.0f, 8.0f),
            new(5.0f, 6.0f, 7.0f),
            new(4.0f, 5.0f, 6.0f), 
            new(3.0f, 4.0f, 5.0f),
            new(1.5f, 2.5f, 3.5f)
        };

        GameObject dummyOrbit = GetDummyOrbit();

        Assert.AreEqual(points[10], dummyOrbit.GetComponent<Orbit>().BinarySearchMax(points));
    }
}
