using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OrbitTests
{
    private Vector3[] points = new Vector3[] {
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
        parent.GetComponent<PointMass>().mass = 50000;
        gameOb.GetComponent<Orbit>().parent = parent.GetComponent<PointMass>();

        return gameOb;
    }

    [Test]
    public void BinarySearchMin()
    {
        GameObject dummyOrbit = GetDummyOrbit();

        Assert.AreEqual(points[0], dummyOrbit.GetComponent<Orbit>().BinarySearchMin(points));
    }

    [Test]
    public void BinarySearchMax()
    {
        GameObject dummyOrbit = GetDummyOrbit();

        Assert.AreEqual(points[10], dummyOrbit.GetComponent<Orbit>().BinarySearchMax(points));
    }

    [Test]
    public void FixedOrbitFlatEccentric()
    {
        GameObject dummyOrbit = GetDummyOrbit();
        dummyOrbit.GetComponent<Orbit>().apoapsisDistance = 4;
        dummyOrbit.GetComponent<Orbit>().periapsisDistance = 3;
        dummyOrbit.GetComponent<Orbit>().rotation = 0;

        dummyOrbit.GetComponent<Orbit>().CalcOrbitFixed();

        // Check that new characteristics are right.
        // Approximate appear to be necessary because of float weirdness.
        Assert.True(Vector3.Distance(new Vector3(4, 0, 0), dummyOrbit.GetComponent<Orbit>().apoapsisPosition) < 0.001);
        Assert.True(Vector3.Distance(new Vector3(-3, 0, 0), dummyOrbit.GetComponent<Orbit>().periapsisPosition) < 0.001);

        // Check that the parameters remain unchanged
        Assert.AreEqual(4, dummyOrbit.GetComponent<Orbit>().apoapsisDistance);
        Assert.AreEqual(3, dummyOrbit.GetComponent<Orbit>().periapsisDistance);
        Assert.AreEqual(0, dummyOrbit.GetComponent<Orbit>().rotation);
    }

    [Test]
    public void FixedOrbitRotatedEccentric()
    {
        GameObject dummyOrbit = GetDummyOrbit();
        dummyOrbit.GetComponent<Orbit>().apoapsisDistance = 4;
        dummyOrbit.GetComponent<Orbit>().periapsisDistance = 3;
        dummyOrbit.GetComponent<Orbit>().rotation = 90;

        dummyOrbit.GetComponent<Orbit>().CalcOrbitFixed();

        // Check that new characteristics are right.
        // Approximate appear to be necessary because of float weirdness.
        Assert.True(Vector3.Distance(new Vector3(0, 4, 0), dummyOrbit.GetComponent<Orbit>().apoapsisPosition) < 0.001);
        Assert.True(Vector3.Distance(new Vector3(0, -3, 0), dummyOrbit.GetComponent<Orbit>().periapsisPosition) < 0.001);

        // Check that the parameters remain unchanged
        Assert.AreEqual(4, dummyOrbit.GetComponent<Orbit>().apoapsisDistance);
        Assert.AreEqual(3, dummyOrbit.GetComponent<Orbit>().periapsisDistance);
        Assert.AreEqual(90, dummyOrbit.GetComponent<Orbit>().rotation);
    }

    [Test]
    public void FixedOrbitFlatCircular()
    {
        GameObject dummyOrbit = GetDummyOrbit();
        dummyOrbit.GetComponent<Orbit>().apoapsisDistance = 3.01f;
        dummyOrbit.GetComponent<Orbit>().periapsisDistance = 3;
        dummyOrbit.GetComponent<Orbit>().rotation = 0;

        dummyOrbit.GetComponent<Orbit>().CalcOrbitFixed();

        // Check that new characteristics are right.
        // Approximate appears to be necessary because of float weirdness.
        Assert.True(Vector3.Distance(new Vector3(3, 0, 0), dummyOrbit.GetComponent<Orbit>().apoapsisPosition) < 0.1);
        Assert.True(Vector3.Distance(new Vector3(-3, 0, 0), dummyOrbit.GetComponent<Orbit>().periapsisPosition) < 0.1);

        // Check that the parameters remain unchanged
        Assert.True(Mathf.Abs(3 - dummyOrbit.GetComponent<Orbit>().apoapsisDistance) < 0.02);
        Assert.True(Mathf.Abs(3 - dummyOrbit.GetComponent<Orbit>().periapsisDistance) < 0.02);
        Assert.AreEqual(0, dummyOrbit.GetComponent<Orbit>().rotation);
    }

    [Test]
    public void FixedOrbitRotatedCircular()
    {
        GameObject dummyOrbit = GetDummyOrbit();
        dummyOrbit.GetComponent<Orbit>().apoapsisDistance = 3.01f;
        dummyOrbit.GetComponent<Orbit>().periapsisDistance = 3;
        dummyOrbit.GetComponent<Orbit>().rotation = 90;

        dummyOrbit.GetComponent<Orbit>().CalcOrbitFixed();

        // Check that new characteristics are right.
        // Approximate appears to be necessary because of float weirdness.
        Assert.True(Vector3.Distance(new Vector3(0, 3, 0), dummyOrbit.GetComponent<Orbit>().apoapsisPosition) < 0.1);
        Assert.True(Vector3.Distance(new Vector3(0, -3, 0), dummyOrbit.GetComponent<Orbit>().periapsisPosition) < 0.1);

        // Check that the parameters remain unchanged
        Assert.True(Mathf.Abs(3 - dummyOrbit.GetComponent<Orbit>().apoapsisDistance) < 0.02);
        Assert.True(Mathf.Abs(3 - dummyOrbit.GetComponent<Orbit>().periapsisDistance) < 0.02);
        Assert.AreEqual(90, dummyOrbit.GetComponent<Orbit>().rotation);
    }

    [Test]
    public void LiveOrbitCalc()
    {
        GameObject dummyOrbit = GetDummyOrbit();

        dummyOrbit.GetComponent<Orbit>().CalcOrbitFromOrbiter(new Vector3(0, -2, 0), new Vector3(1.5f, 0, 0));

        Debug.Log(dummyOrbit.GetComponent<Orbit>().apoapsisPosition);
        Debug.Log(dummyOrbit.GetComponent<Orbit>().periapsisPosition);
        Debug.Log(dummyOrbit.GetComponent<Orbit>().rotation);
        Debug.Log(dummyOrbit.GetComponent<Orbit>().apoapsisDistance);
        Debug.Log(dummyOrbit.GetComponent<Orbit>().periapsisDistance);

        Assert.True(Vector3.Distance(new Vector3(0, 3, 0), dummyOrbit.GetComponent<Orbit>().apoapsisPosition) < 0.1);
        Assert.True(Vector3.Distance(new Vector3(0, -3, 0), dummyOrbit.GetComponent<Orbit>().periapsisPosition) < 0.1);
    }

    [Test]
    public void VelocityCalc()
    {

    }

    [Test]
    public void PositionCalc()
    {

    }

    [Test]
    public void VirtualVelocityCalc()
    {
        GameObject dummyOrbit = GetDummyOrbit();

        Orbit.VirtualOrbiter virtualOrbiter = new(
            dummyOrbit.GetComponent<Orbit>().parent,
            new Vector3(1.5f, 0, 0),
            new Vector3(0, -2, 0)
        );
        
        virtualOrbiter.UpdateVelocity();

        Debug.Log(virtualOrbiter.velocity);
        Debug.Log(virtualOrbiter.position);
    }

    [Test]
    public void VirtualPositionCalc()
    {

    }

    [Test]
    public void VelocityParity()
    {

    }

    [Test]
    public void PositionParity()
    {

    }
}
