using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EmissionSpectra
{
    public static SortedDictionary<string, Element> elements;

    // Minimum and maximum x values
    public static (int, int) spectraRange;

    static EmissionSpectra()
    {
        elements = new SortedDictionary<string, Element>(){
            {"h", new("Hydrogen (H)")},
            {"c", new("Carbon (C)")},
            {"o", new("Oxygen (O)")},
            {"mg", new("Magnesium (Mg)")},
            {"si", new("Silicon (Si)")},
            {"s", new("Sulfur (S)")},
            {"ca", new("Calcium (C)")},
            {"fe", new("Iron (Fe)")},
            {"ni", new("Nickel (Ni)")},
        };

        // Peaks fudged from data at
        // https://www.atomtrace.com/elements-database/, which is from
        // laser-induced plasma spectroscopy. Scientifically, these are
        // completely inappropriate for a gamma ray spectrometer, but actual GRS
        // data is hard to find and messy.
        //
        // Due to the above, the location of these peaks is in nanometer
        // wavelengths ranging [200, 900]. Intensity is [0.0, 1.0].

        elements["h"].AddPeaks(new(){
            new(655, 1.0f),
        });

        elements["c"].AddPeaks(new(){
            new(250, 1.0f),
        });

        elements["o"].AddPeaks(new(){
            new(780, 1.0f),
            new(845, 0.5f),
        });

        elements["mg"].AddPeaks(new(){
            new(280, 0.6f),
            new(285, 1.0f),
        });

        elements["si"].AddPeaks(new(){
            new(200, 1.0f),
            new(250, 1.0f),
            new(290, 0.7f),
        });

        elements["s"].AddPeaks(new(){
            new(410, 0.1f),
            new(470, 0.3f),
            new(605, 0.5f),
            new(675, 1.0f),
            new(770, 0.3f),
        });

        elements["ca"].AddPeaks(new(){
            new(210, 0.2f),
            new(310, 0.9f),
            new(365, 0.4f),
            new(395, 0.7f),
            new(430, 1.0f),
            new(620, 0.3f),
        });

        elements["fe"].AddPeaks(new(){
            new(250, 1.0f),
            new(300, 1.0f),
            new(360, 1.0f),
            new(405, 0.6f),
            new(440, 0.6f),
            new(495, 0.2f),
            new(540, 0.1f),
        });

        elements["ni"].AddPeaks(new(){
            new(235, 1.0f),
            new(300, 0.4f),
            new(340, 1.0f),
            new(400, 0.2f),
            new(510, 0.2f),
        });

        spectraRange = (200, 900);
    }
}
