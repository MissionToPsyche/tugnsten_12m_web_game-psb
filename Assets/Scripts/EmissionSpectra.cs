using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EmissionSpectra
{
    // Lists of emission peaks of the form (position, intensity) indexed by
    // their lowercased element symbol. 
    public static Dictionary<string, List<(int, float)>> spectra;

    // List of all available element symbols populated from `spectra`
    public static List<string> elements;

    // Range of the graph
    public static (int, int) spectraRange;

    [RuntimeInitializeOnLoadMethod]
    static void InitializeSpectra()
    {
        // Peaks fudged from data at
        // https://www.atomtrace.com/elements-database/, which is from
        // laser-induced plasma spectroscopy. Scientifically, these are
        // completely inappropriate for a gamma ray spectrometer, but actual GRS
        // data is hard to find and messy.
        //
        // Due to the above, the location of these peaks is in nanometer
        // wavelengths ranging [200nm, 900nm]. Intensity is [0.0, 1.0].
        spectra = new Dictionary<string, List<(int, float)>>(){
            // hydrogen
            {"h", new(){
                (655, 1.0f),
            }},
            // carbon
            {"c", new(){
                (250, 1.0f),
            }},
            // oxygen
            {"o", new(){
                (780, 1.0f),
                (845, 0.5f),
            }},
            // magnesium
            {"mg", new(){
                (280, 0.6f),
                (285, 1.0f),
            }},
            // silicon
            {"si", new(){
                (200, 1.0f),
                (250, 1.0f),
                (290, 0.7f),
            }},
            // sulfur
            {"s", new(){
                (410, 0.1f),
                (470, 0.3f),
                (605, 0.5f),
                (675, 1.0f),
                (770, 0.3f),
            }},
            // calcium
            {"ca", new(){
                (210, 0.2f),
                (310, 0.9f),
                (365, 0.4f),
                (395, 0.7f),
                (430, 1.0f),
                (620, 0.3f),
            }},
            // iron
            {"fe", new(){
                (250, 1.0f),
                (300, 1.0f),
                (360, 1.0f),
                (405, 0.6f),
                (440, 0.6f),
                (495, 0.2f),
                (540, 0.1f),
            }},
            // nickel
            {"ni", new(){
                (235, 1.0f),
                (300, 0.4f),
                (340, 1.0f),
                (400, 0.2f),
                (510, 0.2f),
            }},
        };

        // Extracts the keys from the spectra dictionary
        elements = new(spectra.Keys);

        spectraRange = (200, 900);
    }

}
