using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public GameObject skipButton;
    public GameObject restartButton;
    public GameObject continueButton;

    // Start is called before the first frame update
    private void Start()
    {
        ResetUI();
    }

    public void ShowText(string message)
    {
        headerText.text = message;
    }

    public void ResetUI()
    {
        ShowText("");
        restartButton.SetActive(false);
        continueButton.SetActive(false);
    }

    public void EnterFailState()
    {
        restartButton.SetActive(true);
    }

    public void EnterWinState()
    {
        restartButton.SetActive(true);
        continueButton.SetActive(true);
    }
}
