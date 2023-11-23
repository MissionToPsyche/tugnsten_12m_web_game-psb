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

    public void ShowText(string message)
    {
        headerText.text = message;
    }
}
