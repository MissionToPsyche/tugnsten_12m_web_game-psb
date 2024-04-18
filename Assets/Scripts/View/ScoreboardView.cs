using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class ScoreboardView : MonoBehaviour
{
    private VisualElement root, scoreboardPanel, scoreContainer, numberScoreContainer, letterScoreContainer, totalScoreContainer;
    private Label magnetometerNumber, magnetometerLetter, imagerNumber, imagerLetter, gravityNumber, gravityLetter, spectNumber, spectLetter, orbitNumber, orbitLetter, AverageNumber, AverageLetter;
    private Button titleBtn;
    private ChangeScene SceneChanger;
    public Scorecard scorecard;
    private int[] scores = {-1, -1, -1, -1, -1, -1};
    private string[] displayScores = {"-", "-", "-", "-", "-", "-", "-"};
    private string[] displayGrades = {"-", "-", "-", "-", "-", "-", "-"};

    private void DisableKeyboardNavigation()
    {
        var myUIDDocument = GetComponent<UIDocument>();
        var rootElement = myUIDDocument.rootVisualElement;

        // Use Query to select all elements in the UIDDocument
        var allElements = rootElement.Query<VisualElement>().ToList();

        // Disable keyboard navigation for all elements
        foreach (var element in allElements)
        {
            element.focusable = false;
            element.UnregisterCallback<KeyDownEvent>(OnKeyDownEvent);
        }
    }

    private void OnKeyDownEvent(KeyDownEvent evt)
    {
        // Prevent the default keyboard navigation behavior
        evt.StopPropagation();
    }

    public void OnEnable()
    {
        InitializeUI();
        BindUIEvents();
        GetData();
        SetDisplayData();
        DisplayData();
    }

    public void InitializeUI()
    {
        // all visual containers
        root = GetComponent<UIDocument>().rootVisualElement;
        scoreboardPanel = root.Q<VisualElement>("scoreboard-panel");
        scoreContainer = scoreboardPanel.Q<VisualElement>("score-display-container");
        numberScoreContainer = scoreContainer.Q<VisualElement>("number-score-container");
        letterScoreContainer = scoreContainer.Q<VisualElement>("letter-score-container");
        totalScoreContainer = scoreboardPanel.Q<VisualElement>("total-score-container");
        
        // Number scores of the minigames
        magnetometerNumber = numberScoreContainer.Q<Label>("magnetometer-number-score");
        imagerNumber = numberScoreContainer.Q<Label>("imager-number-score");
        gravityNumber = numberScoreContainer.Q<Label>("gravity-number-score");
        spectNumber = numberScoreContainer.Q<Label>("spectrometer-number-score");
        orbitNumber = numberScoreContainer.Q<Label>("orbit-number-score");

        // Letter scores of the minigames
        magnetometerLetter = letterScoreContainer.Q<Label>("magnetometer-letter-score");
        imagerLetter = letterScoreContainer.Q<Label>("imager-letter-score");
        gravityLetter = letterScoreContainer.Q<Label>("gravity-letter-score");
        spectLetter = letterScoreContainer.Q<Label>("spectrometer-letter-score");
        orbitLetter = letterScoreContainer.Q<Label>("orbit-letter-score");

        // Total scores of the minigames
        AverageNumber = totalScoreContainer.Q<Label>("total-number-score");
        AverageLetter = totalScoreContainer.Q<Label>("total-letter-score");
    
        // Close button
        titleBtn = root.Q<Button>("title-screen-button");
        DisableKeyboardNavigation();
    }

    private void GetData()
    {
        scores[0] = scorecard.MagnetometerScore;
        scores[1] = scorecard.ImagerScore;
        scores[2] = scorecard.GravityScore;
        scores[3] = scorecard.SpectrometerScore;
        scores[4] = orbitAvg();
        scores[5] = AvgScore();
    }

    private void setGrades()
    {
        for(int i = 0; i < scores.Length; i++)
        {
            displayGrades[i] = GetGrade(scores[i]);
        }
    }

    private void SetDisplayData()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if(scores[i] != -1)
            {
                displayScores[i] = scores[i].ToString();
            }
            else{
                displayScores[i] = "-";
            }
        }

        setGrades();
    }

    private void DisplayData()
    {
        // display scores
        magnetometerNumber.text = displayScores[0];
        imagerNumber.text = displayScores[1];
        gravityNumber.text = displayScores[2];
        spectNumber.text = displayScores[3];
        orbitNumber.text = displayScores[4];
        AverageNumber.text = displayScores[5];

        // display grades
        magnetometerLetter.text = displayGrades[0];
        imagerLetter.text = displayGrades[1];
        gravityLetter.text = displayGrades[2];
        spectLetter.text = displayGrades[3];
        orbitLetter.text = displayGrades[4];
        AverageLetter.text = displayGrades[5];
    }

    private int AvgScore()
    {
        int avg = 0;
        int sum = 0;
        int ctr = 0;
        foreach (int score in scores)
        {
            if(score != -1)
            {
                ctr++;
                sum += score;
            }
        }
        try
        {
            avg = sum / ctr;
        }
        catch(Exception e)
        {
            avg = -1;
        }
        return avg;
    }

    private int orbitAvg()
    {
        int[] orbitScores = {-1, -1, -1, -1};
        orbitScores = scorecard.OrbitScore;
        int sum = 0;
        int avg = -1;
        int ctr = 0;
        foreach (int score in orbitScores)
        {
            if(score != -1)
            {
                ctr++;
                sum += score;
            }
        }
        try
        {
            avg = sum / ctr;
        }
        catch(Exception e)
        {
            avg = -1;
        }
        return avg;
    }

    private string GetGrade(int score)
    {
        if (score < 0)
        {
            return "-";
        }

        if (score >= 9600)
            return "A+";
        else if (score >= 9200)
            return "A";
        else if (score >= 8800)
            return "A-";
        else if (score >= 8400)
            return "B+";
        else if (score >= 8000)
            return "B";
        else if (score >= 7600)
            return "B-";
        else if (score >= 7200)
            return "C+";
        else if (score >= 6800)
            return "C";
        else if (score >= 6200)
            return "C-";
        else if (score >= 5500)
            return "D+";
        else if (score >= 4000)
            return "D";
        else if (score >= 2500)
            return "D-";
        else
            return "F";
    }




    public void BindUIEvents()
    {
        titleBtn.clicked += () => { playSound(); ReturnToTitleScreen(); };
    }

    public void ReturnToTitleScreen()
    {
        // Debug.Log("Title clicked");
        SceneManager.LoadScene("Title");
    }

    private void playSound()
    {
        AudioClip clip = (AudioClip)Resources.Load("Sounds/analog-appliance-button");
        SoundManager.Instance.PlaySound(clip);
    }
}