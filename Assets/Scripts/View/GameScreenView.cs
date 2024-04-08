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
    private Button optionsBtn, continueBtn, resetBtn, mainMenuBtn, infoBtn, xBtn, closeBtn;
    private VisualElement root, gameScreen, gameBottomContainer, gameTopContainer, topBorder, gameButtonContainer, optionsPanel, soundBar, optionsButtonContainer, infoPanel;
    private Label minigameTitle, timer;
    private void OnEnable()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentSceneName = scene.name;
        minigameIndex = titleController.getSceneIndex(currentSceneName);

        InitializeUI();
        BindUIEvents();
    }

    private void InitializeUI()
    {
        ////////////////////////////////////////////////////////////////////////////////
        // MINIGAME SCREEN UI ELEMENTS
        root = GetComponent<UIDocument>().rootVisualElement;
        // game screen
        gameScreen = root.Q<VisualElement>("game-screen");
        gameTopContainer = gameScreen.Q<VisualElement>("game-top-container");
        gameBottomContainer = gameScreen.Q<VisualElement>("game-bottom-container");
        gameButtonContainer = gameBottomContainer.Q<VisualElement>("game-button-container");
        topBorder = gameTopContainer.Q<VisualElement>("top-border");
        timer = topBorder.Q<Label>("timer-label");
        minigameTitle = gameBottomContainer.Q<Label>("minigame-title");
        minigameTitle.text = titleController.getMinigameText(minigameIndex); // setting the minigame title

        //buttons on the game screen
        infoBtn = gameTopContainer.Q<Button>("help-button");
        optionsBtn = gameButtonContainer.Q<Button>("options-button");

        continueBtn = gameButtonContainer.Q<Button>("continue-button");
        // continueBtn.clicked += () => continueButtonClicked();
        // continueBtn.clicked += () => rightButtonClicked(string action);

        ////////////////////////////////////////////////////////////////////////////////
        // OPTIONS SCREEN UI ELEMENTS
        // // loading the options panel UXML file
        // VisualTreeAsset optionsPanelTree = Resources.Load<VisualTreeAsset>("Assets/UI/UXML/OptionsPanel.uxml");
        // // cloning the UXML content and add it to root element
        // Debug.Log(optionsPanelTree == null ? "Failed to load OptionsPanel.uxml" : "OptionsPanel.uxml loaded successfully");
        // optionsPanel = optionsPanelTree.CloneTree();
        // root.Add(optionsPanel);

        optionsPanel = root.Q<VisualElement>("options-panel");
        soundBar = optionsPanel.Q<VisualElement>("sound-bar");
        optionsButtonContainer = optionsPanel.Q<VisualElement>("options-button-container");

        // buttons on the options screen
        xBtn = optionsPanel.Q<Button>("x-button");
        resetBtn = optionsButtonContainer.Q<Button>("reset-button");
        mainMenuBtn = optionsButtonContainer.Q<Button>("main-menu-button");

        ////////////////////////////////////////////////////////////////////////////////
        // INFO PANEL UI ELEMENTS
        infoPanel = root.Q<VisualElement>("info-panel");
        closeBtn = infoPanel.Q<Button>("close-button");
    }

    private void BindUIEvents()
    {
        optionsBtn.clicked += () => { optionsButtonClicked(); playSound(); };
        // cancelBtn.clicked += () => cancel();
        xBtn.clicked += () => { cancel(); playSound(); };
        mainMenuBtn.clicked += () => { SceneManager.LoadScene("Title"); playSound(); }; // return to title screen
        infoBtn.clicked += () => { infoPanel.visible = true; playSound(); };
        closeBtn.clicked += () => { infoPanel.visible = false; playSound(); };
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
        gameScreen.visible = true;
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