using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    private TextMeshProUGUI textMeshProComponent;
   

    public void setText(string text)
    {
        textMeshProComponent.text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        textMeshProComponent = GetComponent<TextMeshProUGUI>();
    }
}
