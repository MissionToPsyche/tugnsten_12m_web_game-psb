using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySciGenerator : MonoBehaviour
{
    // Number of points at which the user can distort the orbit
    public int numPositions = 3;

    // Minimum distance between distortions, represented as percent progress
    // along orbit. minSeparation * numPositions must be less than 1, and for
    // performance reasons should be less than 0.75;
    public float minSeparation = 0.15f;

    // Gets a list of distortions represented as (progress along orbit, distortion strength).
    public List<Distortion> GetDistortions(int orbitPoints)
    {
        if (numPositions * minSeparation > 1)
        {
            Debug.LogError("GravitySciGenerator: minSeparation too high for numPositions!");
        }
        else if (numPositions * minSeparation > 0.75)
        {
            Debug.LogWarning("GravitySciGenerator: performance impact due to minSeparation being too high.");
        }

        List<float> positions = new();

        // Generates positions
        for (int i = 0; i < numPositions; i++)
        {
            float newPos = Random.Range(0f, 1.0f);

            // Checks that the new position is far enough away from all existing positions.
            for (int j = 0; j < positions.Count; j++)
            {
                // If too close...
                if (Mathf.Abs(positions[j] - newPos) < minSeparation)
                {
                    // ...regenerate...
                    newPos = Random.Range(0f, 1.0f);
                    // ...and check again from the first. 
                    j = -1;
                }
            }

            positions.Add(newPos);
        }

        List<Distortion> distortions = new();

        // Generates distortion for each position
        for (int i = 0; i < numPositions; i++)
        {
            float intensity = Random.Range(-1.0f, 1.0f);

            distortions.Add(new(positions[i], intensity, orbitPoints));
        }

        return distortions;
    }
}
