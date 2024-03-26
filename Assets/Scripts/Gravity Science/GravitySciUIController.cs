using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravitySciUIController : UIController
{
    public Waveform referenceWave = null;
    public Waveform userWave = null;
    public Slider controlSlider;

    private void Update() {
        userWave.SetWavelength(controlSlider.value);
    }

    public override void ResetUI()
    {
        controlSlider.value = 0.5f;
    }

    public override void ShowScore(int score)
    {
        throw new System.NotImplementedException();
    }
}
