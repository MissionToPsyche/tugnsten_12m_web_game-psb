using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpacecraft : RailsSpacecraft
{
    // Spacecraft will appear and disappear at this percent around the orbit
    [Range(0, 1)]
    public float startPosition = 0;
    [Range(0, 1)]
    public float endPosition = 1;

    // Percent around the orbit at which the spacecraft reaches maximum size
    [Range(0, 1)]
    public float maxPosition;

    // Spacecraft will wait this many seconds before teleporting from end to start position.
    public float delay = 1;
    private float elapsedTime = 0;

    // Beginning at the start position, spacecraft will grow linearly to this
    // percent of its normal size until the maxSizePosition, then shrink again
    // to reach normal size at the end point.
    public float maxSize = 1;

    private Vector3 initialSize;

    // Rotates the start, end, and max positions around the orbit
    [Range(0, 1)]
    public float positionRotation;
    private int positionOffset;
    
    private float offsetIndex;
    private float initialIndex;

    private int startPoint;
    private int maxPoint;
    private int endPoint;

    private new void OnValidate()
    {
        startPoint = Mathf.RoundToInt(orbit.numOrbitPoints * startPosition);
        maxPoint = Mathf.RoundToInt(orbit.numOrbitPoints * maxPosition);
        endPoint = Mathf.RoundToInt(orbit.numOrbitPoints * endPosition);

        positionOffset = Mathf.RoundToInt(orbit.numOrbitPoints * positionRotation);
    }

    private new void Start()
    {
        initialSize = transform.localScale;

        startPoint = Mathf.RoundToInt(orbit.numOrbitPoints * startPosition);
        maxPoint = Mathf.RoundToInt(orbit.numOrbitPoints * maxPosition);
        endPoint = Mathf.RoundToInt(orbit.numOrbitPoints * endPosition);
        positionOffset = Mathf.RoundToInt(orbit.numOrbitPoints * positionRotation);

        currentIndex = ((startPoint - positionOffset) % orbit.numOrbitPoints + orbit.numOrbitPoints) % orbit.numOrbitPoints;
        initialIndex = currentIndex;
    }

    void Update()
    {
        offsetIndex = Mathf.Repeat(currentIndex + positionOffset, orbit.numOrbitPoints);

        if (offsetIndex < endPoint)
        {
            UpdatePosition();
            UpdateSize();
        }
        else
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= delay)
            {
                currentIndex = initialIndex;
                elapsedTime = 0;
                transform.localScale = new(1, 1, 1);
            }
        }
    }

    public void UpdateSize()
    {
        float progress;

        // Between start and max, else between max and end
        if (offsetIndex < maxPoint)
        {
            progress = Mathf.InverseLerp(startPoint, maxPoint, offsetIndex);
        }
        else
        {
            progress = 1;
        }

        float scale = Mathf.Lerp(1, maxSize, progress);

        transform.localScale = initialSize * scale;
    }

    public new void UpdatePosition()
    {
        int prevIndex = Mathf.FloorToInt(currentIndex);
        int nextIndex = (prevIndex + 1) % orbit.numOrbitPoints;
        float t = currentIndex - prevIndex;

        Vector3 prevPosition = orbit.orbitLine[prevIndex] + orbit.transform.position;
        Vector3 nextPosition = orbit.orbitLine[nextIndex] + orbit.transform.position;
        Vector3 position = Vector3.Lerp(prevPosition, nextPosition, t);

        // Increases speed based on scale to make the spacecraft appear to move toward the camera
        float speedMultiplier = Mathf.Max(Mathf.Pow(transform.localScale.magnitude, 2) / 10, 1);

        currentIndex += speed * speedMultiplier * Time.deltaTime;
        currentIndex = Mathf.Repeat(currentIndex, orbit.numOrbitPoints); // Wrap around after reaching the end

        transform.position = position;
    }
}
