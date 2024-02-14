using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element
{
    public string name;
    public List<SpectrumPeak> peaks;
    [Range(0, 1)]
    public float quantity = 1.0f;

    public Element(string name, float quantity = 1.0f)
    {
        this.name = name;
        this.quantity = quantity;
    }

    public void AddPeaks(List<SpectrumPeak> peaks)
    {
        this.peaks = peaks;
    }

    public Element Clone() {
        Element copied = new(name, quantity);
        copied.AddPeaks(peaks);
        return copied;
    }
}
