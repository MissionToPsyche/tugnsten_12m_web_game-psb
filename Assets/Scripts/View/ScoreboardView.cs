using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScoreboardView : MonoBehaviour
{
    private VisualElement root, scoreboardPanel, scoreContainer, numberScoreContainer, letterScoreContainer, totalScoreContainer;
    private Label magnetometerNumber, magnetometerLetter, imagerNumber, imagerLetter, gravityNumber, gravityLetter, spectNumber, spectLetter, OrbitNumber, OrbitLetter, TotalNumber, TotalLetter;
    private Button titleBtn;
    private ChangeScene SceneChanger;
    public void OnEnable()
    {
        InitializeUI();
        BindUIEvents();
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
        spectNumber = numberScoreContainer.Q<Label>("spect-number-score");
        OrbitNumber = numberScoreContainer.Q<Label>("Orbit-number-score");

        // Letter scores of the minigames
        magnetometerLetter = letterScoreContainer.Q<Label>("magnetometer-letter-score");
        imagerLetter = letterScoreContainer.Q<Label>("imager-letter-score");
        gravityLetter = letterScoreContainer.Q<Label>("gravity-letter-score");
        spectLetter = letterScoreContainer.Q<Label>("spect-letter-score");
        OrbitLetter = letterScoreContainer.Q<Label>("Orbit-letter-score");

        // Total scores of the minigames
        TotalNumber = totalScoreContainer.Q<Label>("total-number-score");
        TotalLetter = totalScoreContainer.Q<Label>("total-letter-score");
    
        // Close button
        titleBtn = root.Q<Button>("title-screen-button");
    }
    public void BindUIEvents()
    {
        titleBtn.clicked += () => ReturnToTitleScreen();
    }

    public void ReturnToTitleScreen()
    {
        Debug.Log("Title clicked");
        SceneManager.LoadScene("Title");
    }
}