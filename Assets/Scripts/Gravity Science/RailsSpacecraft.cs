using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsSpacecraft : MonoBehaviour
{
    public DistortedOrbit orbit;

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

    // Animates the spacecraft by moving it between points on the orbit in sequence.
    void Update()
    {
        currentIndex += speed * Time.deltaTime;
        currentIndex = Mathf.Repeat(currentIndex, orbit.ellipsePoints); // Wrap around after reaching the end

        int prevIndex = Mathf.FloorToInt(currentIndex);
        int nextIndex = (prevIndex + 1) % orbit.ellipsePoints;

        float lerp = currentIndex - prevIndex;

        Vector3 prevPosition = orbit.distortedOrbitLine[prevIndex] + orbit.transform.position;
        Vector3 nextPosition = orbit.distortedOrbitLine[nextIndex] + orbit.transform.position;

        transform.position = Vector3.Lerp(prevPosition, nextPosition, lerp);
    }
}
