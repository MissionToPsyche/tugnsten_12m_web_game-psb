using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameScreenUI : MonoBehaviour
{
    private int minigameIndex;
    private string currentSceneName;
    public TitleController titleController;
    private Button optionsBtn, continueBtn, cancelBtn, mainMenuBtn;
    private VisualElement root, gameScreen, optionsScreen, gameBottomContainer, gameTopContainer, gameButtonContainer, optionsContainer, optionsContainerBottom;
    
    public VisualElement getBottomStrip()
    {
        return gameBottomContainer;
    }
    public VisualElement getTopStrip()
    {
        return gameTopContainer;
    }
    
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
        optionsScreen = root.Q<VisualElement>("options-screen");
        optionsContainer = optionsScreen.Q<VisualElement>("options-container");
        optionsContainerBottom = optionsContainer.Q<VisualElement>("bottom-container");
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
    private void optionsButtonClicked()
    {
        optionsScreen.visible = true;
        gameScreen.visible = false;
    }

    private void continueButtonClicked()
    {
        minigameIndex += 1;
        // titleController.setMinigame(minigameIndex);

        if (minigameIndex <= 3)
        {
            SceneManager.LoadScene(titleController.getSceneName(minigameIndex));
        }
    }
    private void cancel()
    {
        optionsScreen.visible = false;
        gameScreen.visible = true;
    }
}