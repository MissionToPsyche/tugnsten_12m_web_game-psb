using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Magnetometer
{
    public float tolerance = 0.001f;

    // TORUS GENERATOR:
    public TorusGenerator GetTorusGenerator()
    {
        GameObject torusGeneratorObj = new();
        torusGeneratorObj.AddComponent<TorusGenerator>();
        TorusGenerator torusGenerator = torusGeneratorObj.GetComponent<TorusGenerator>();
        return torusGenerator;
    }
    // drawTorus method creates a Torus object with correct magnetic moment and adds it to the torusObject
    [Test]
    public void test_draw_torus()
    {
        // Arrange
        TorusGenerator torusGenerator = GetTorusGenerator();
        int numEllipses = 2;
        int numPoints = 10;

        // Act
        Torus torus = torusGenerator.drawTorus(numEllipses, numPoints);

        // Assert
        Assert.IsNotNull(torus);
        Assert.IsNotNull(torus.torusObject);
        Assert.AreEqual(numEllipses*2, torus.getEllipses().Count);
        Assert.IsNotNull(torus.magneticMoment);
    }

    // createEllipse method creates an Ellipse object with correct semiMajorAxis, semiMinorAxis, and usablePoints
    [Test]
    public void test_create_ellipse_with_correct_parameters()
    {
        // Arrange
        TorusGenerator torusGenerator = GetTorusGenerator();
        int numEllipses = 2;
        int numPoints = 10;

        // Act
        Torus torus = torusGenerator.drawTorus(numEllipses, numPoints);
        Ellipse ellipse = torus.getEllipses()[0];

        // Assert
        Assert.IsNotNull(ellipse);
        Assert.That(ellipse.semiMajorAxis, Is.InRange(0.3148148f - tolerance, 0.5666667f + tolerance));
        Assert.That(ellipse.semiMinorAxis, Is.InRange(0.1736111f - tolerance, 0.3125000f + tolerance));
        Assert.That(ellipse.usablePoints.Count, Is.InRange(0f, ellipse.lineObject.GetComponent<LineRenderer>().positionCount));
    }

    // mapEllipseScale method maps a magnetic moment strength to an ellipse scale value
    [Test]
    public void test_map_ellipse_scale()
    {
        // Arrange
        TorusGenerator torusGenerator = GetTorusGenerator();
        float magStrengthMin = 2 * Mathf.Pow(10f, 13f);
        float magStrengthMax = 2 * Mathf.Pow(10f, 15f);
        float magStrength = Random.Range(magStrengthMin, magStrengthMax);

        // Act
        float ellipseScale = torusGenerator.mapEllipseScale(magStrengthMin, magStrengthMax, magStrength);

        // Assert
        Assert.That(ellipseScale, Is.InRange(torusGenerator.GetMinScale(), torusGenerator.GetMaxScale()));
    }

    // mapTorusScaleRange method maps an ellipse scale value to a torus scale range
    [Test]
    public void test_map_torus_scale_range()
    {
        // Arrange
        TorusGenerator torusGenerator = GetTorusGenerator();
        float ellipseScale = 3.0f;

        // Act
        (float, float) scaleRange = torusGenerator.mapTorusScaleRange(ellipseScale);

        // Assert
        Assert.That(scaleRange.Item1, Is.InRange(0.3f, 0.55f));
        Assert.That(scaleRange.Item2, Is.InRange(1.0f, 1.8f));
    }

    // generateMagneticMoments method generates a list of random magnetic moments with correct magnitude and components
    [Test]
    public void test_generate_magnetic_moments_with_correct_magnitude_and_components()
    {
        // Arrange
        TorusGenerator torusGenerator = GetTorusGenerator();
        float minMagnitude = 2 * Mathf.Pow(10f, 13f);
        float maxMagnitude = 2 * Mathf.Pow(10f, 15f);

        // Act
        List<Vector3> magMoments = torusGenerator.generateMagneticMoments(minMagnitude, maxMagnitude);

        // Assert
        Assert.IsNotNull(magMoments);
        Assert.AreEqual(10, magMoments.Count);

        foreach (Vector3 moment in magMoments)
        {
            Assert.That(moment.magnitude, Is.EqualTo(0f).Or.InRange(minMagnitude, maxMagnitude));
            Assert.AreEqual(0f, moment.z);
        }
    }

    // setScaleAndRotation method sets the correct scale and rotation for the torusObject
    [Test]
    public void test_set_scale_and_rotation()
    {
        // Arrange
        TorusGenerator torusGenerator = GetTorusGenerator();
        int numEllipses = 2;
        int numPoints = 10;

        // Act
        Torus torus = torusGenerator.drawTorus(numEllipses, numPoints);
        torusGenerator.setScaleAndRotation(2.5f);

        // Assert
        Transform t = torus.torusObject.transform;
        Assert.AreEqual(0, t.eulerAngles.x);
        Assert.AreEqual(0, t.eulerAngles.y);
        Assert.That(t.eulerAngles.z, Is.InRange(0f, 360f));
        Assert.That(t.localScale.x, Is.InRange(0.3f, 1.8f) & Is.EqualTo(t.localScale.y) & Is.EqualTo(t.localScale.z), "Local scale is not within the expected range or not equal to y and z.");
    }

    // ARROW GENERATOR:
    public ArrowGenerator GetArrowGenerator()
    {
        GameObject arrowGeneratorObj = new();
        arrowGeneratorObj.AddComponent<ArrowGenerator>();
        ArrowGenerator arrowGenerator = arrowGeneratorObj.GetComponent<ArrowGenerator>();
        return arrowGenerator;
    }

    [Test]
    public void test_generate_field_points()
    {
        // Arrange
        ArrowGenerator arrowGenerator = GetArrowGenerator();
        TorusGenerator torusGenerator = GetTorusGenerator();
        Torus torus = torusGenerator.drawTorus(5, 100);
        int numPoints = 100;
        int numArrows = 5;

        // Act
        List<(Vector3, Vector3, Vector3)> fieldPoints = arrowGenerator.getFieldPoints(torus, numPoints, numArrows);

        // Assert
        Assert.AreEqual(numArrows, fieldPoints.Count);
        foreach ((Vector3, Vector3, Vector3) point in fieldPoints)
        {
            Assert.IsNotNull(point.Item1);
            Assert.IsNotNull(point.Item2);
            Assert.IsNotNull(point.Item3);
        }
    }

    // calcMagField method calculates magnetic field vectors
    [Test]
    public void test_calcMagField_returns_correct_magnetic_field_vector()
    {
        // Arrange
        ArrowGenerator arrowGenerator = GetArrowGenerator();
        Vector3 r = new Vector3(0, 2, 0);
        float minMagnitude = 2 * Mathf.Pow(10f, 13f);
        Vector3 magneticMoment = new Vector3(minMagnitude, 0, 0);

        // Act
        Vector3 magField = arrowGenerator.calcMagField(r, magneticMoment);

        // Assert
        Assert.IsTrue(Vector3.Distance(new Vector3(-250000.00f, -0.01f, 0.00f), magField) < tolerance);
    }

    // The arrow GameObjects are assigned a rotation based on the direction of the magnetic field vector
    [Test]
    public void test_arrow_rotation_scale_position()
    {
        // Arrange
        ArrowGenerator arrowGenerator = GetArrowGenerator();
        TorusGenerator torusGenerator = GetTorusGenerator();
        Torus torus = torusGenerator.drawTorus(5, 100);
        int numPoints = 100;
        int numArrows = 5;

        List<(Vector3, Vector3, Vector3)> fieldPoints = arrowGenerator.getFieldPoints(torus, numPoints, numArrows);
        fieldPoints = new List<(Vector3, Vector3, Vector3)>();
        fieldPoints.Add((new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(0, 1, 0)));

        // Act
        arrowGenerator.drawArrows(fieldPoints);
        GameObject arrow = GameObject.Find("arrow0");

        Vector3 expectedRotation = new Vector3(0, 0, 135);
        float fieldMagnitude = fieldPoints[0].Item2.magnitude;
        float modifiedMagnitude = arrowGenerator.mapRange(fieldMagnitude);
        Vector3 expectedMagnitude = new Vector3(modifiedMagnitude, modifiedMagnitude, 1);
        // Assert
        Assert.AreEqual(expectedRotation, arrow.transform.rotation.eulerAngles);
        Assert.AreEqual(new Vector3(1, 0, -1), arrow.transform.position);
        Assert.AreEqual(expectedMagnitude, arrow.transform.localScale);
    }

    // drawArrows sets the scale of the GameObjects to a random value between 0.06 and 0.3 when the magnitude of the second element of the input tuples is 0
    [Test]
    public void test_drawArrows_sets_scale_when_magnitude_is_zero()
    {
        // Arrange
        ArrowGenerator arrowGenerator = GetArrowGenerator();
        TorusGenerator torusGenerator = GetTorusGenerator();
        Torus torus = torusGenerator.drawTorus(5, 100);
        int numPoints = 100;
        int numArrows = 5;
        List<(Vector3, Vector3, Vector3)> fieldPoints = arrowGenerator.getFieldPoints(torus, numPoints, numArrows);
        fieldPoints = new List<(Vector3, Vector3, Vector3)>();
        fieldPoints.Add((new Vector3(1, 0, 0), Vector3.zero, new Vector3(2, 0, 0)));
        fieldPoints.Add((new Vector3(2, 0, 0), Vector3.zero, new Vector3(3, 0, 0)));
        fieldPoints.Add((new Vector3(3, 0, 0), Vector3.zero, new Vector3(4, 0, 0)));

        // Act
        arrowGenerator.drawArrows(fieldPoints);

        // Assert
        foreach ((Vector3, Vector3, Vector3) point in fieldPoints)
        {
            GameObject arrow = GameObject.Find("arrow" + fieldPoints.IndexOf(point));
            float scale = arrow.transform.localScale.x;

            Assert.That(scale, Is.InRange(0.06f, 0.3f));
        }
    }

    // calcMagField returns a zero vector when the magnetic moment is zero
    [Test]
    public void test_calcMagField_returns_zero_vector_when_magnetic_moment_is_zero()
    {
        // Arrange
        ArrowGenerator arrowGenerator = GetArrowGenerator();
        Vector3 r = new Vector3(0, 2, 0);
        Vector3 magneticMoment = Vector3.zero;

        // Act
        Vector3 magField = arrowGenerator.calcMagField(r, magneticMoment);

        // Assert
        Assert.AreEqual(Vector3.zero, magField);
    }

    // MOVE TORUS
    // Test if the torus rotates correctly when the mouse is moved
    // [Test]
    // public void test_rotate_torus()
    // {
    //     // Arrange
    //     GameObject gameObject = new GameObject();
    //     MoveTorus moveTorus = gameObject.AddComponent<MoveTorus>();
    //     moveTorus.startDistance = 0f;
    //     moveTorus.startScale = new Vector3(1, 1, 1);
    //     moveTorus.initialMousePosition = new Vector3(1f, 1f, 0f);
    //     moveTorus.initialTorusRotation = Quaternion.identity;
    //     moveTorus.northObject = gameObject;
    //     moveTorus.southObject = gameObject;

    //     // Simulate mouse movement
    //     Input.mousePosition = new Vector3(100f, 100f, 0f);

    //     // Assert
    //     Assert.AreNotEqual(moveTorus.initialTorusRotation, moveTorus.transform.rotation);
    // }

    // // Verify if the scaling of the torus is applied correctly
    // [Test]
    // public void test_apply_scaling()
    // {
    //     // Arrange
    //     GameObject gameObject = new GameObject();
    //     MoveTorus moveTorus = gameObject.AddComponent<MoveTorus>();
    //     float expectedScale = 3.0f;

    //     // Act

    //     // Assert
    //     Assert.AreEqual(expectedScale, gameObject.transform.localScale.x);
    //     Assert.AreEqual(expectedScale, gameObject.transform.localScale.y);
    //     Assert.AreEqual(expectedScale, gameObject.transform.localScale.z);
    // }

    // MAGNETOMETER GAME CONTROLLER
    // Calculate score based on rotation, time, and scale
    [Test]
    public void test_calculate_score_based_on_rotation_time_and_scale()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        MagnetometerGameController controller = gameObject.AddComponent<MagnetometerGameController>();
        controller.timer = gameObject.AddComponent<GameTimer>();
        controller.timer.setTime(30.0f);
        Torus torus = new Torus();
        GameObject torusObj = new GameObject();
        torusObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        torusObj.transform.Rotate(new Vector3(0, 0, 90f));
        torus.torusObject = torusObj;
        controller.setTorus(torus);

        // Act
        controller.CalcScore();

        // Assert
        Assert.AreEqual(8492, controller.score);
    }
}
