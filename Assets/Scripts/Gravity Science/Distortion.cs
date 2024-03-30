using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distortion
{
    [Range(0, 1.0f)]
    public float position = 0;

    // True intensity, not shown
    [Range(-1.0f, 1.0f)]
    public float trueIntensity = 0;

    // User-controlled intensity, shown
    [Range(-1.0f, 1.0f)]
    public float intensity = 0;

    // Intended for variable size fluctuations, but never worked right so don't touch. 
    public float size = 0.5f;

    public int numPointsAffected;
    public int centerPoint;
    public int firstPoint;
    public int lastPoint;

    public Distortion(float position, float trueIntensity, int orbitPoints)
    {
        this.position = position;
        this.trueIntensity = trueIntensity;

        // Bitwise OR ensures that the number of points is odd so it can be centered properly.
        numPointsAffected = Mathf.RoundToInt(orbitPoints * size) | 1;
        int halfPointsAffected = numPointsAffected / 2;

        centerPoint = Mathf.RoundToInt(orbitPoints * position);

        firstPoint = centerPoint - halfPointsAffected;
        lastPoint = centerPoint + halfPointsAffected;

        // Wraps the values if they fall off either end of the array
        firstPoint = (firstPoint + orbitPoints) % orbitPoints;
        lastPoint = (lastPoint + orbitPoints) % orbitPoints;
    }
}
