using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element
{
    public string name;
    public List<SpectrumPeak> peaks;

    public Element(string name)
    {
        this.name = name;
    }

    public void AddPeaks(List<SpectrumPeak> peaks)
    {
        this.peaks = peaks;
    }
}
