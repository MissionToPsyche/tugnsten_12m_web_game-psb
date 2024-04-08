// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection.Emit;
// using PlasticGui.WorkspaceWindow;
using PlasticGui.WorkspaceWindow;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleController : MonoBehaviour
{
    private Vector3[] positions = new Vector3[] {
        new Vector3(0f, 0f, -10f),
        new Vector3(145f, 0f, -10f),
        new Vector3(293f, 0f, -10f),
        new Vector3(438f, 0f, -10f),
        new Vector3(583f, 0f, -10f),
    };

    private string[] minigames = new string[] {
        "Magnetometer",
        "Multispectral Imager",
        "Gravity Science",
        "Spectrometer",
        "Orbit",
    };

    private string[] scenes = new string[] {
        "Magnetometer_minigame",
        "Imager Minigame",
        "Science_minigame",
        "Spectrometer_minigame",
        "Orbit_subgame",
    };
    private string currentScene = "Magnetometer_minigame";
    private int index;

    public Vector3[] getAllPositions()
    {
        return positions;
    }

    public Vector3 getPosition()
    {
        return positions[index];
    }

    public string getScene()
    {
        return currentScene;
<<<<<<< HEAD
    }

    public bool isFirstScene()
    {
        return index == 0;
    }

    public bool isLastScene()
    {
        return index == scenes.Length - 1;
    }
    public void getNextScene()
    {
        // avoid re-traversing through the array
        if (index < positions.Length - 1)
        {
            index = (index + 1) % positions.Length; // making sure its in bound
            currentScene = scenes[index];
        }

    }

    public void getPrevScene()
    {
        // avoid re-traversing through the array
        if (index > 0)
        {
            index = (index - 1 + scenes.Length) % scenes.Length; // making sure its in bound

        }
=======
>>>>>>> 8d9c88ec218c0f3db5ae6d2a3c5653b39ac5a2fb
    }

    public void setMinigame(int index)
    {
        this.index = index;
        currentScene = scenes[index];
    }

    public string getMinigameText(int index)
    {
        return minigames[index];
    }

    public void minigameSelect(Label minigameText)
    {
        setMinigame(0);
        minigameText.text = minigames[index];
    }

    public void updateMinigame(Label minigameText)
    {
        if (minigameText != null && index >= 0 && index < minigames.Length)
        {
            minigameText.text = minigames[index];
            currentScene = scenes[index];
        }
        setMinigame(index);
    }
    public int getSceneIndex(string name)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i] == name)
            {
                return i;
            }
        }
        Debug.Log("Scene not found");
        return -1;
    }
    public void setSceneIndex(int index)
    {
        this.index = index;
    }
    public string getSceneName(int index)
    {
        return scenes[index];
    }

}
