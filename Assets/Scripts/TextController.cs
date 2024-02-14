using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using System.Reflection.Emit;
using UnityEngine.UIElements;

public class TextController : MonoBehaviour
{
    // private TextMeshProUGUI textMeshProComponent;
   private Label minigameText;

    public void setText(string text)
    {
        minigameText.text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        // minigameText = GetComponent<TextMeshProUGUI>();
        minigameText = GetComponent<Label>();
    }
}
