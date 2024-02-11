using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumPeak
{
    public int location;
    public float intensity;
    // Gaussian function constant ro that controls peak width
    public float width;

    public SpectrumPeak(int location, float intensity, float width = 1.0f)
    {
        this.location = location;
        this.intensity = intensity;
        this.width = width;
    }
}
