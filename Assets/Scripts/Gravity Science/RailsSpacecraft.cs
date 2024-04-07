using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsSpacecraft : MonoBehaviour
{
    public RailsOrbit orbit;

    public float speed = 35.0f;
    private float currentIndex = 0;

    void OnValidate()
    {
        // Adding the orbit's position makes the spacecraft's origin the same as the orbit's origin.
        transform.position = orbit.orbitLine[0] + orbit.transform.position;
    }

    void Start()
    {
        currentIndex = 0;
        transform.position = orbit.orbitLine[0] + orbit.transform.position;
    }

    public void UpdatePosition()
    {
        currentIndex += speed * Time.deltaTime;
        currentIndex = Mathf.Repeat(currentIndex, orbit.numOrbitPoints); // Wrap around after reaching the end

        int prevIndex = Mathf.FloorToInt(currentIndex);
        int nextIndex = (prevIndex + 1) % orbit.numOrbitPoints;

        float lerp = currentIndex - prevIndex;

        Vector3 prevPosition = orbit.orbitLine[prevIndex] + orbit.transform.position;
        Vector3 nextPosition = orbit.orbitLine[nextIndex] + orbit.transform.position;

        transform.position = Vector3.Lerp(prevPosition, nextPosition, lerp);
    }
}
