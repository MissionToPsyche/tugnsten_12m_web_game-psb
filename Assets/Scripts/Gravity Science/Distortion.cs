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

    public Distortion(float position, float trueIntensity)
    {
        this.position = position;
        this.trueIntensity = trueIntensity;
    }
}
