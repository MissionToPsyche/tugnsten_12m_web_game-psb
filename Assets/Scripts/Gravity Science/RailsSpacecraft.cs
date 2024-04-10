using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsSpacecraft : MonoBehaviour
{
    public RailsOrbit orbit;

    public float speed = 35.0f;

    protected float currentIndex = 0;

    protected void OnValidate()
    {
        // Adding the orbit's position makes the spacecraft's origin the same as the orbit's origin.
        transform.position = orbit.orbitLine[0] + orbit.transform.position;
    }

    protected void Start()
    {
        currentIndex = 0;
        transform.position = orbit.orbitLine[0] + orbit.transform.position;
    }

    public void UpdatePosition()
    {
        int prevIndex = Mathf.FloorToInt(currentIndex);
        int nextIndex = (prevIndex + 1) % orbit.numOrbitPoints;
        float t = currentIndex - prevIndex;
        
        Vector3 prevPosition = orbit.orbitLine[prevIndex] + orbit.transform.position;
        Vector3 nextPosition = orbit.orbitLine[nextIndex] + orbit.transform.position;
        Vector3 position = Vector3.Lerp(prevPosition, nextPosition, t);

        // Approximates Kepler's law using the altitude to vary speed based on
        // position in orbit.
        Vector3 radiusVector = position - orbit.parent.transform.position;
        float radius = radiusVector.magnitude;
        float speedMultiplier = 1.0f / Mathf.Sqrt(radius);

        currentIndex += speed * speedMultiplier * Time.deltaTime;
        currentIndex = Mathf.Repeat(currentIndex, orbit.numOrbitPoints); // Wrap around after reaching the end

        transform.position = position;
    }
}
