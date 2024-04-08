using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DistortedOrbit : RailsOrbit
{
    // Factor that decreases the intensity of distortions
    public float intensityReduction = 6;

    public Vector3[] undistortedOrbitLine;

    public List<Distortion> distortions;

    // Start is called before the first frame update
    void Start()
    {
        DrawOrbit();
        undistortedOrbitLine = (Vector3[])orbitLine.Clone();
    }

    public void ApplyDistortions()
    {
        Vector3[] tempPositions = (Vector3[])undistortedOrbitLine.Clone();

        foreach (Distortion distortion in distortions)
        {
            int centerPoint = Mathf.RoundToInt(numOrbitPoints * distortion.position);

            // Bitwise OR ensures that the number of points is odd so it can be centered properly.
            int numPointsAffected = Mathf.RoundToInt(numOrbitPoints * distortion.size) | 1;

            int halfPointsAffected = numPointsAffected / 2;

            // Loops over numPointsAffected x values, with half the range below 0 and half above. 
            for (int x = -halfPointsAffected; x <= halfPointsAffected; x++)
            {
                // Implements Gaussian function Ae^(-x^2/2c^2) where A is the
                // intensity and c is the width. Intensity is reduced to make
                // the distortions a reasonable size.
                float y = distortion.intensity / intensityReduction * Mathf.Exp(-Mathf.Pow(x, 2) / Mathf.Pow(2 * 10, 2));

                // The point being operated on in terms of the actual orbit.
                int currentOrbitPoint = centerPoint + x;

                // Wraps the point if it falls off the beginning or end of the array.
                currentOrbitPoint = (currentOrbitPoint + numOrbitPoints) % numOrbitPoints;

                // Direction vector pointing away from Psyche through the current point.
                Vector3 radialDirection = tempPositions[currentOrbitPoint].normalized;

                // Shifts the point outward by y.
                tempPositions[currentOrbitPoint] += y * radialDirection;
            }
        }

        lr.positionCount = tempPositions.Length;
        lr.SetPositions(tempPositions);
        orbitLine = (Vector3[])tempPositions.Clone();
    }
}
