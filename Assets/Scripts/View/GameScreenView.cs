using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameScreenUI : MonoBehaviour
{
    private int minigameIndex;
    private string currentSceneName;
    public TitleController titleController;
    public AudioClip clip;
    private Button optionsBtn, continueBtn, resetBtn, mainMenuBtn, infoBtn, xBtn, closeBtn;
    private VisualElement root, gameScreen, gameBottomContainer, gameOptionsContainer, gameContinueContainer, gameTopContainer, topBorder, gameButtonContainer, optionsPanel, soundBar, optionsButtonContainer, infoPanel, blackScreen, tabs, scorePanel, scoreContainer;
    private Slider musicSlider, soundSlider;
    private ScrollView infoScrollView;
    private Label minigameTitle, timer, instructionsTab, contextTab, numberScore, letterScore;
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
        blackScreen = root.Q<VisualElement>("black-screen");
        // game screen
        gameScreen = root.Q<VisualElement>("game-screen");
        gameTopContainer = gameScreen.Q<VisualElement>("game-top-container");
        gameBottomContainer = gameScreen.Q<VisualElement>("game-bottom-container");
        // gameButtonContainer = gameBottomContainer.Q<VisualElement>("game-button-container");
        gameOptionsContainer = gameBottomContainer.Q<VisualElement>("options-button-container");
        gameContinueContainer = gameBottomContainer.Q<VisualElement>("continue-button-container");
        topBorder = gameTopContainer.Q<VisualElement>("top-border");
        timer = topBorder.Q<Label>("timer-label");
        minigameTitle = gameBottomContainer.Q<Label>("minigame-title");
        minigameTitle.text = titleController.getMinigameText(minigameIndex); // setting the minigame title

        //buttons on the game screen
        infoBtn = gameTopContainer.Q<Button>("help-button");
        optionsBtn = gameOptionsContainer.Q<Button>("options-button");

        continueBtn = gameContinueContainer.Q<Button>("continue-button");
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
        musicSlider = soundBar.Q<Slider>("music-slider");
        soundSlider = soundBar.Q<Slider>("sound-slider");

        ////////////////////////////////////////////////////////////////////////////////
        // INFO PANEL UI ELEMENTS
        infoPanel = root.Q<VisualElement>("info-panel");
        tabs = infoPanel.Q<VisualElement>("tabs");
        instructionsTab = tabs.Q<Label>("instructions");
        contextTab = tabs.Q<Label>("science-context");
        infoScrollView = infoPanel.Q<ScrollView>("game-info");
        showInfo();
        closeBtn = infoPanel.Q<Button>("close-button");

        ////////////////////////////////////////////////////////////////////////////////
        // SCORE PANEL UI ELEMENTS
        scorePanel = root.Q<VisualElement>("score-panel");
        scoreContainer = scorePanel.Q<VisualElement>("score-container");
        numberScore = scoreContainer.Q<Label>("number-score");
        letterScore = scoreContainer.Q<Label>("letter-score");
    }

    private void BindUIEvents()      
    {
        optionsBtn.clicked += () => { optionsButtonClicked(); playSound(); };
        xBtn.clicked += () => { closePanel(); playSound(); };
        mainMenuBtn.clicked += () => { SceneManager.LoadScene("Title"); playSound(); }; // return to title screen
        infoBtn.clicked += () => { infoClicked(); playSound(); };
        closeBtn.clicked += () => { closePanel(); playSound(); };
        resetBtn.clicked += () => playSound();
        musicSlider.RegisterCallback<ChangeEvent<float>>(musicValueChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(soundValueChanged);
        RegisterTabCallbacks();
    }

    public Button getContinueButton()
    {
        return continueBtn;
    }

    public void optionsButtonClicked()
    {
        blackScreen.visible = true;
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

    public void infoClicked()
    {
        infoPanel.visible = true;
        blackScreen.visible = true;
    }

    public void RegisterTabCallbacks()
    {
        UQueryBuilder<Label> tabs = GetAllTabs();
        tabs.ForEach((Label tab) => {
            tab.RegisterCallback<ClickEvent>(TabOnClick);
        });
    }
    private void TabOnClick(ClickEvent evt)
    {
        Label clickedTab = evt.currentTarget as Label;
        if (!TabIsCurrentlySelected(clickedTab))
        {
            GetAllTabs().Where(
                (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)
            ).ForEach(UnselectTab);
            SelectTab(clickedTab);
        }
    }
     private static bool TabIsCurrentlySelected(Label tab)
    {
        return tab.ClassListContains("selectedTab");
    }

    private void SelectTab(Label tab)
    {
        tab.AddToClassList("selectedTab");
        if(tab.name == "Instructions")
        {
            showInfo();
        }
        else if(tab.name == "science-context")
        {
            ShowContext();
        }
    }

    private UQueryBuilder<Label> GetAllTabs()
    {
        return root.Query<Label>(className: "tab");
    }

    private void UnselectTab(Label tab)
    {
        tab.RemoveFromClassList("selectedTab");
        Debug.Log("Unselected tab");
        // tab.AddToClassList("unselectedTab");
    }

    public void showInfo()
    {
        string infoUxmlPath = $"UI/UXML/{minigameTitle.text}Info";
        VisualTreeAsset gameInfoTree = Resources.Load<VisualTreeAsset>(infoUxmlPath);


        if (gameInfoTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInfoTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
        else
        {
            Debug.Log($"{minigameTitle.text}Info.uxml file not found.");
        }
    }

    public void ShowContext()
    {
        string infoUxmlPath = $"UI/UXML/{minigameTitle.text}Context";
        VisualTreeAsset gameInfoTree = Resources.Load<VisualTreeAsset>(infoUxmlPath);


        if (gameInfoTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInfoTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
        else
        {
            Debug.Log($"{minigameTitle.text}Context.uxml file not found.");
        }
    }

    public void showScorePanel()
    {
        scorePanel.visible = true;
        blackScreen.visible = true;
    }

    public void closePanel()
    {
        gameScreen.visible = true;
        optionsPanel.visible = false;
        infoPanel.visible = false;
        blackScreen.visible = false;
        scorePanel.visible = false;
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

    public Button getResetButton()
    {
        return resetBtn;
    }

    public Button getOptionsButton()
    {
        return optionsBtn;
    }

    public Button getOptionsCloseButton()
    {
        return xBtn;
    }

    public Button getInfoButton()
    {
        return infoBtn;
    }

    public Button getInfoCloseButton()
    {
        return closeBtn;
    }

    public VisualElement getInfoPanel()
    {
        return infoPanel;
    }

    public VisualElement getOptionsPanel()
    {
        return optionsPanel;
    }

    private void musicValueChanged(ChangeEvent<float> evt)
    {
        Debug.Log("Slider value changed: " + evt.newValue);
        GameObject musicSource = SoundManager.Instance.transform.GetChild(0).gameObject;
        Debug.Log("Music source name: " + musicSource.name); // Log the name of the music source
        AudioSource audioSource = musicSource.GetComponent<AudioSource>();
        audioSource.volume = evt.newValue / 100;
    }
    private void soundValueChanged(ChangeEvent<float> evt)
    {
        Debug.Log("Slider value changed: " + evt.newValue);
        GameObject soundSource = SoundManager.Instance.transform.GetChild(1).gameObject;
        Debug.Log("Sound source name: " + soundSource.name); // Log the name of the sound source
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();
        audioSource.volume = evt.newValue / 100;
    }
}