using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameScreenUI : MonoBehaviour
{
    public PersistentInt lastMinigameIndex;
    private int currentSceneIndex;
    private string currentSceneName;
    public TitleController titleController;
    public AudioClip clip;
    private Button optionsBtn, continueBtn, resetBtn, mainMenuBtn, infoBtn, closeOptionsBtn, closeInfoBtn, closeScoreBtn, scoreContinueBtn, scoreCloseBtn;
    private VisualElement root, gameScreen, gameBottomContainer, gameOptionsContainer, gameContinueContainer, gameTopContainer, topBorder, gameButtonContainer, optionsPanel, soundBar, optionsButtonContainer, infoScreen, infoPanel, blackScreen, tabs, scorePanel, scoreContainer, scoreButtonContainer;
    private Slider musicSlider, soundSlider;
    private ScrollView infoScrollView;
    private Label minigameTitle, timer, instructionsTab, contextTab, numberScore, letterScore;
    private UIDocument myUIDocument;

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

    private void OnEnable()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentSceneName = scene.name;
        currentSceneIndex = titleController.getSceneIndex(currentSceneName);

        // If not orbit game
        if (currentSceneIndex != 4)
        {
            lastMinigameIndex.Num = currentSceneIndex;
        }
        else
        {
            OrbitGameController controller = FindFirstObjectByType<OrbitGameController>().GetComponent<OrbitGameController>();
            controller.missionOrbit = lastMinigameIndex.Num;
        }

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
        InitializeMinigameTitle();
        // minigameTitle.text = titleController.getMinigameText(currentSceneIndex); // set minigame text in title controller

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
        closeOptionsBtn = optionsPanel.Q<Button>("x-button");
        resetBtn = optionsButtonContainer.Q<Button>("reset-button");
        mainMenuBtn = optionsButtonContainer.Q<Button>("main-menu-button");
        musicSlider = soundBar.Q<Slider>("music-slider");
        soundSlider = soundBar.Q<Slider>("sound-slider");
        InitializeSlider(musicSlider, 0);
        InitializeSlider(soundSlider, 1);

        ////////////////////////////////////////////////////////////////////////////////
        // INFO PANEL UI ELEMENTS
        infoScreen = root.Q<TemplateContainer>("info-screen");
        // infoTemplate = root.Q<TemplateContainer>("info-screen");
        infoPanel = infoScreen.Q<VisualElement>("info-panel");
        tabs = infoPanel.Q<VisualElement>("tabs");
        instructionsTab = tabs.Q<Label>("Instructions");
        contextTab = tabs.Q<Label>("science-context");
        infoScrollView = infoPanel.Q<ScrollView>("game-info");
        closeInfoBtn = infoPanel.Q<Button>("close-button");

        LoadInfo();

        ////////////////////////////////////////////////////////////////////////////////
        // SCORE PANEL UI ELEMENTS
        scorePanel = root.Q<VisualElement>("score-panel");
        scoreContainer = scorePanel.Q<VisualElement>("score-container");
        numberScore = scoreContainer.Q<Label>("number-score");
        letterScore = scoreContainer.Q<Label>("letter-score");
        scoreButtonContainer = scorePanel.Q<VisualElement>("score-button-container");
        scoreCloseBtn = scoreButtonContainer.Q<Button>("close-score-button");
        scoreContinueBtn = scoreButtonContainer.Q<Button>("continue-button");

        DisableKeyboardNavigation();
        OpenInfoPanel();
    }

    public void ShowScorePanel(float numberScore, string letterGrade)
    {
        this.numberScore.text = numberScore.ToString();
        letterScore.text = letterGrade;
        blackScreen.visible = true;
        scorePanel.visible = true;
    }

    private void BindUIEvents()
    {
        optionsBtn.clicked += () => { OptionsButtonClicked(); PlaySound(); };
        closeOptionsBtn.clicked += () => { ClosePanel(); PlaySound(); };
        mainMenuBtn.clicked += () => { SceneManager.LoadScene("Title"); PlaySound(); }; // return to title screen
        infoBtn.clicked += () => { OpenInfoPanel(); PlaySound(); };
        closeInfoBtn.clicked += () => { ClosePanel(); PlaySound(); };
        scoreCloseBtn.clicked += () => { ClosePanel(); PlaySound(); };
        scoreContinueBtn.clicked += () => { ContinueButtonClicked(); PlaySound(); };
        resetBtn.clicked += () => { PlaySound(); ClosePanel(); reset(); };
        continueBtn.clicked += () => PlaySound();
        musicSlider.RegisterCallback<ChangeEvent<float>>(musicValueChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(soundValueChanged);
        RegisterTabCallbacks();
    }

    private void reset()
    {
        resetBtn.SetEnabled(false);
    }

    private void CloseScorePanel()
    {
        scorePanel.visible = false;
    }

    public Button GetContinueButton()
    {
        return continueBtn;
    }

    public void OptionsButtonClicked()
    {
        resetBtn.SetEnabled(true);
        blackScreen.visible = true;
        optionsPanel.visible = true;
    }

    public void ContinueButtonClicked()
    {
        if (currentSceneIndex == 3) // Last minigame
        {
            // Go to score scene
            SceneManager.LoadScene(titleController.getSceneName(5));
        }
        else if (currentSceneIndex != 4) // Any minigame but orbit
        {
            lastMinigameIndex.Num += 1;
            // Load the orbit game
            SceneManager.LoadScene(titleController.getSceneName(4));
        }
        else if (lastMinigameIndex.Num == -1) // Standalone orbit
        {
            SceneManager.LoadScene("Title");
        }
        else // Normal orbit
        {
            if (lastMinigameIndex.Num < 4)
            {
                SceneManager.LoadScene(titleController.getSceneName(lastMinigameIndex.Num));
            }
        }
    }

    public void OpenInfoPanel()
    {
        HandleTabSeclected(instructionsTab);
        // infoPanel.visible = true;
        infoScreen.style.display = DisplayStyle.Flex;
        blackScreen.visible = true;
    }

    public void RegisterTabCallbacks()
    {
        UQueryBuilder<Label> tabs = GetAllTabs();
        tabs.ForEach((Label tab) =>
        {
            tab.RegisterCallback<ClickEvent>(TabOnClick);
        });
    }
    private void TabOnClick(ClickEvent evt)
    {
        PlaySound();
        Label clickedTab = evt.currentTarget as Label;
        HandleTabSeclected(clickedTab);
    }

    private void HandleTabSeclected(Label clickedTab)
    {
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
        if (tab.name == "Instructions")
        {
            LoadInfo();
        }
        else if (tab.name == "science-context")
        {
            LoadContext(minigameTitle.text);
        }
    }

    private UQueryBuilder<Label> GetAllTabs()
    {
        return root.Query<Label>(className: "tab");
    }

    private void UnselectTab(Label tab)
    {
        tab.RemoveFromClassList("selectedTab");
        // Debug.Log("Unselected tab");
        // tab.AddToClassList("unselectedTab");
    }

    public void LoadInStrunction(string minigameName)
    {
        string instructionUxmlPath = $"UI/UXML/{minigameName}Info";
        VisualTreeAsset gameInstrunctionTree = Resources.Load<VisualTreeAsset>(instructionUxmlPath);


        if (gameInstrunctionTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInstrunctionTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
        else
        {
            // Debug.Log($"{minigameTitle.text}Info.uxml file not found.");
        }
    }

    public void LoadContext(string minigameName)
    {
        string contextUxmlPath = $"UI/UXML/{minigameName}Context";
        VisualTreeAsset gameInfoTree = Resources.Load<VisualTreeAsset>(contextUxmlPath);


        if (gameInfoTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInfoTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
        else
        {
            // Debug.Log($"{minigameTitle.text}Context.uxml file not found.");
        }
    }

    public void LoadInfo()
    {
        if(currentSceneIndex == 4)  // load same orbit info in all orbit scene
        {
            LoadInStrunction("Orbit");
        }
        else
        {
            LoadInStrunction(minigameTitle.text);
        }

    }

    public void InitializeMinigameTitle()
    {
        // getting minigame text from title controller
        if (currentSceneIndex == 4 && lastMinigameIndex.Num == 0) // orbit A
        {
            minigameTitle.text = titleController.getMinigameText(currentSceneIndex) + " A";
        }
        else if (currentSceneIndex == 4 && lastMinigameIndex.Num == 1) // orbit B
        {
            minigameTitle.text = titleController.getMinigameText(currentSceneIndex) + " B";
        }
        else if (currentSceneIndex == 4 && lastMinigameIndex.Num == 2) // orbit C
        {
            minigameTitle.text = titleController.getMinigameText(currentSceneIndex) + " C";
        }
        else if (currentSceneIndex == 4 && lastMinigameIndex.Num == 3) // orbit D
        {
            minigameTitle.text = titleController.getMinigameText(currentSceneIndex) + " D";
        }
        else // all other minigames including standalone orbit
        {
            minigameTitle.text = titleController.getMinigameText(currentSceneIndex);
        }

    }



    public void ShowScorePanel()
    {
        scorePanel.visible = true;
        blackScreen.visible = true;
    }

    public void ClosePanel()
    {
        gameScreen.visible = true;
        optionsPanel.visible = false;
        // infoPanel.visible = false;
        infoScreen.style.display = DisplayStyle.None;
        scorePanel.visible = false;
        blackScreen.visible = false;
    }
    private void PlaySound()
    {
        SoundManager.Instance.PlaySound(clip);
    }
    public Label GetTimer()
    {
        return timer;
    }
    public void SetTimerText(string time)
    {
        timer.text = time;
    }

    public VisualElement GetBottomContainer()
    {
        return gameBottomContainer;
    }

    public VisualElement GetTopContainer()
    {
        return gameTopContainer;
    }

    public Button GetResetButton()
    {
        return resetBtn;
    }

    public Button GetOptionsButton()
    {
        return optionsBtn;
    }

    public Button getOptionsCloseButton()
    {
        return closeOptionsBtn;
    }

    public Button getInfoButton()
    {
        return infoBtn;
    }

    public Button getInfoCloseButton()
    {
        return closeInfoBtn;
    }

    public VisualElement getInfoScreen()
    {
        return infoScreen;
    }

    public VisualElement getOptionsPanel()
    {
        return optionsPanel;
    }

    private void musicValueChanged(ChangeEvent<float> evt)
    {
        // Debug.Log("Slider value changed: " + evt.newValue);
        GameObject musicSource = SoundManager.Instance.transform.GetChild(0).gameObject;
        // Debug.Log("Music source name: " + musicSource.name); // Log the name of the music source
        AudioSource audioSource = musicSource.GetComponent<AudioSource>();
        audioSource.volume = evt.newValue / 100;
    }
    private void soundValueChanged(ChangeEvent<float> evt)
    {
        // Debug.Log("Slider value changed: " + evt.newValue);
        GameObject soundSource = SoundManager.Instance.transform.GetChild(1).gameObject;
        // Debug.Log("Sound source name: " + soundSource.name); // Log the name of the sound source
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();
        audioSource.volume = evt.newValue / 100;
    }
    public void InitializeSlider(Slider slider, int child)
    {
        if (SoundManager.Instance != null)
        {
            GameObject audioSource = SoundManager.Instance.transform.GetChild(child).gameObject;
            AudioSource audio = audioSource.GetComponent<AudioSource>();
            slider.value = audio.volume * 100;
        }
    }
}