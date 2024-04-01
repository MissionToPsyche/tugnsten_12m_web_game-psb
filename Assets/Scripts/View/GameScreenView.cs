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
    private VisualElement root, gameScreen, gameBottomContainer, gameTopContainer, gameButtonContainer, optionsPopup, optionsContainerBottom;

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

        root.pickingMode = PickingMode.Ignore;
    }
    private void optionsButtonClicked()
    {
        // optionsScreen.visible = true;
        gameScreen.visible = false;
        // gameTopContainer.visible = false;
        // gameBottomContainer.visible = false;
        optionsPopup.visible = true;
    }

    private void continueButtonClicked()
    {
        minigameIndex += 1;
        // titleController.setMinigame(minigameIndex);

        if (minigameIndex < 4)
        {
            SceneManager.LoadScene(titleController.getSceneName(minigameIndex));
        }
    }
    private void cancel()
    {
        // optionsScreen.visible = false;
        gameScreen.visible = true;
        // gameTopContainer.visible = true;
        // gameBottomContainer.visible = true;
        optionsPopup.visible = false;
    }

}