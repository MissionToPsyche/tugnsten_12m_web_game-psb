using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using PlasticGui.WorkspaceWindow;

public class GameScreenUI : MonoBehaviour
{
    private int minigameIndex;
    private string currentSceneName;
    public TitleController titleController;
    public AudioClip clip;
    private Button optionsBtn, continueBtn, cancelBtn, mainMenuBtn;
    private VisualElement root, gameScreen, gameBottomContainer, infoPanel, gameTopContainer, topBorder, gameButtonContainer, optionsPanel, optionsContainerBottom;
    private Label minigameTitle, timer;
    private void OnEnable()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentSceneName = scene.name;
        minigameIndex = titleController.getSceneIndex(currentSceneName);

        // initializing visual elements
        root = GetComponent<UIDocument>().rootVisualElement;
        // game screen
        gameScreen = root.Q<VisualElement>("game-screen");
        gameTopContainer = gameScreen.Q<VisualElement>("game-top-container");
        gameBottomContainer = gameScreen.Q<VisualElement>("game-bottom-container");
        gameButtonContainer = gameBottomContainer.Q<VisualElement>("button-container");
        topBorder = gameTopContainer.Q<VisualElement>("top-border");
        timer = topBorder.Q<Label>("timer-label");

        minigameTitle = gameBottomContainer.Q<Label>("minigame-title");
        minigameTitle.text = titleController.getMinigameText(minigameIndex);
        // options screen
        optionsPanel = root.Q<VisualElement>("options-panel");
        optionsContainerBottom = optionsPanel.Q<VisualElement>("bottom-container");
        optionsBtn = gameButtonContainer.Q<Button>("options-button");
        optionsBtn.clicked += () => { optionsButtonClicked(); playSound(); };
        cancelBtn = optionsContainerBottom.Q<Button>("cancel-button");
        cancelBtn.clicked += () => { cancel(); playSound(); };
        mainMenuBtn = optionsContainerBottom.Q<Button>("main-menu-button");
        mainMenuBtn.clicked += () => { SceneManager.LoadScene("Title"); playSound(); };

        continueBtn = gameButtonContainer.Q<Button>("continue-button");
        // continueBtn.clicked += () => continueButtonClicked();
        // continueBtn.clicked += () => rightButtonClicked(string action);

        infoPanel = root.Q<VisualElement>("info-panel");
        
    }

    public Button getContinueButton()
    {
        return continueBtn;
    }

    public void optionsButtonClicked()
    {
        gameScreen.visible = false;
        optionsPanel.visible = true;
    }

    public void continueButtonClicked()
    {
        minigameIndex += 1;
        // titleController.setMinigame(minigameIndex);

        if (minigameIndex < 4)
        {
            SceneManager.LoadScene(titleController.getSceneName(minigameIndex));
        }
    }
    public void cancel()
    {
        // optionsScreen.visible = false;
        gameScreen.visible = true;
        // gameTopContainer.visible = true;
        // gameBottomContainer.visible = true;
        optionsPanel.visible = false;
    }
    private void playSound()
    {
        SoundManager.Instance.PlaySound(clip);
    }
    public Label GetTimer()
    {
        return timer;
    }
    public void setTimerText(string time)
    {
        timer.text = time;
    }

    public VisualElement getBottomContainer()
    {
        return gameBottomContainer;
    }

    public VisualElement getTopContainer()
    {
        return gameTopContainer;
    }
}