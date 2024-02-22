// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection.Emit;
// using PlasticGui.WorkspaceWindow;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleController : MonoBehaviour
{
    private Vector3[] positions = new Vector3[] {
        new Vector3(0f, 0f, -10f),
        new Vector3(145f, 0f, -10f),
        new Vector3(293f, 0f, -10f),
        new Vector3(438f, 0f, -10f),
    };

    private string[] minigames = new string[] {
        "Magnetometer",
        "Multispectral Imager",
        "Gravity Science",
        "Spectrometer",
    };

    private string[] scenes = new string[] {
        "Magnetometer_minigame",
        "Imager Minigame",
        "Science_minigame",
        "Spectrometer_minigame",
    };
    private string nextScene = "Magnetometer_minigame";
    private int index;

    public Vector3[] getAllPositions()
    {
        return positions;
    }

    public Vector3 getPosition()
    {
        return positions[index];
    }

    public int getIndex()
    {
        return index;
    }

    public string getNextScene()
    {
        return nextScene;
    }

    // public string getMinigameTitle()
    // {
    //     return minigames[index];
    // }
    public void setMinigame(int index)
    {
        this.index = index;
        nextScene = scenes[index];
    }

    // Start is called before the first frame update
    public void minigameSelect(Label minigameText)
    {
        setMinigame(0);
        minigameText.text = minigames[index];

    }

    public void updateMinigame(Label minigameText)
    {
        if(minigameText != null && index >= 0 && index < minigames.Length)
        {
            minigameText.text = minigames[index];
            nextScene = scenes[index];
        }
        setMinigame(index);
    }
}
