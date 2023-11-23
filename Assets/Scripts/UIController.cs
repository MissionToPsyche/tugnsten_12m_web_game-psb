using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI headerText;

    // Start is called before the first frame update
    private void Start()
    {
        headerText.text = "";
    }

    public void ShowWin()
    {
        headerText.text = "Orbit Reached";
    }

    public void ShowCrash()
    {
        headerText.text = "Spacecraft Crashed!";
    }   

    public void ShowEscape()
    {
        headerText.text = "Spacecraft Escaped Orbit!";
    } 
}
