using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using PlasticGui.WorkspaceWindow;

public class GameScreenUI : MonoBehaviour
{
    private int minigameIndex;
    private string currentSceneName;
    public TitleController titleController;
    public Canvas canvas;
    private Button optionsBtn, continueBtn, cancelBtn, mainMenuBtn;
    private VisualElement root, gameScreen, gameBottomContainer, gameTopContainer, topBorder, gameButtonContainer, optionsPopup, optionsContainerBottom;
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

        timer.text = "00:00";

        minigameTitle = gameBottomContainer.Q<Label>("minigame-title");
        minigameTitle.text = titleController.getMinigameText(minigameIndex);
        // options screen
        optionsPopup = root.Q<VisualElement>("options-popup");
        optionsContainerBottom = optionsPopup.Q<VisualElement>("bottom-container");
        // 
        optionsBtn = gameButtonContainer.Q<Button>("options-button");
        optionsBtn.clicked += () => optionsButtonClicked();
        cancelBtn = optionsContainerBottom.Q<Button>("cancel-button");
        cancelBtn.clicked += () => cancel();
        mainMenuBtn = optionsContainerBottom.Q<Button>("main-menu-button");
        mainMenuBtn.clicked += () => SceneManager.LoadScene("Title");

        continueBtn = gameButtonContainer.Q<Button>("continue-button");
        continueBtn.clicked += () => continueButtonClicked();

    }
    public void optionsButtonClicked()
    {
        // optionsScreen.visible = true;
        gameScreen.visible = false;
        // gameTopContainer.visible = false;
        // gameBottomContainer.visible = false;
        optionsPopup.visible = true;
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
        optionsPopup.visible = false;
    }

    public Label GetTimer()
    {
        return timer;
    }
    public void SetTimer(string time)
    {
        timer.text = time;
    }
}