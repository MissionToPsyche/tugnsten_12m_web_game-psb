using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenView : MonoBehaviour
{ 
    // Public variables for editor assignment
    public TitleController titleController;
    public AudioClip clip;
    public GameObject MinigameSelectMenu, Console, Animation;
    // public OptionsScreenView optionsScreenView;

    // Private UI elements grouped by their functionality/screen
    private VisualElement root, mainScreen, buttonContainer, gameSelectScreen, gameSelectTop, gameSelectCenter, gameSelectBottom, optionsScreen, optionsPanel, soundbar, optionsButtonContainer, creditsScreen, blackScreen, infoScreen, infoPanel, tabs, scorePanel, scoreContainer;
    private ScrollView infoScrollView, creditsScrollView;

    private Button playBtn, gameSelectBtn, OptionsBtn, CreditsBtn, closeMinigameBtn, playMinigameBtn, closeOptionsBtn, closeCreditsBtn, nextBtn, prevBtn, infoBtn, closeInfoBtn;

    private Slider musicSlider, soundSlider;
    private Label minigameTitle,instructionsTab, contextTab;
    private CameraZoom cameraZoom;
    private SlideCamera slideCamera;

    public PersistentInt lastSceneIndex;

    public Scorecard scorecard;

    // Collections for easier management
    private List<VisualElement> screens = new List<VisualElement>();

    private void OnEnable()
    {
        InitializeUI();
        BindUIEvents();
        
        // Resets all persistent data
        scorecard.ResetScorecard();
        lastSceneIndex.Num = -1;
    }

    void Update()
    {
        UpdateMinigameScreen();

        if(titleController.gameSelect && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !titleController.cameraZooming && !titleController.inInfoPanel)
        {
            PlayMinigame();
            PlaySound();
        }

    }

    private void InitializeUI()
    {   
        ////////////////////////////////////////////////////////////////////////////////
        // MAIN SCREEN UI ELEMENTS
        cameraZoom = Camera.main.GetComponent<CameraZoom>();
        cameraZoom.setTitleController(titleController);
        slideCamera = Camera.main.GetComponent<SlideCamera>();

        root = GetComponent<UIDocument>().rootVisualElement;
        mainScreen = root.Q<VisualElement>("main-menu-screen");
        gameSelectScreen = root.Q<VisualElement>("game-select-screen");
        optionsScreen = root.Q<VisualElement>("options-screen");
        creditsScreen = root.Q<VisualElement>("credits-screen");
        infoScreen = root.Q<TemplateContainer>("info-screen");
        blackScreen = root.Q<VisualElement>("black-screen");
        screens.AddRange(new VisualElement[] { mainScreen, gameSelectScreen, optionsScreen, creditsScreen });
        buttonContainer = mainScreen.Q<VisualElement>("button-container");

        // buttons on the main screen
        playBtn = buttonContainer.Q<Button>("play-button");
        gameSelectBtn = buttonContainer.Q<Button>("game-select-button");
        OptionsBtn = buttonContainer.Q<Button>("options-button");
        CreditsBtn = buttonContainer.Q<Button>("credits-button");

        ////////////////////////////////////////////////////////////////////////////////
        // MINIGAME SELECT UI ELEMENTS
        gameSelectTop = gameSelectScreen.Q<VisualElement>("game-select-top");
        gameSelectCenter = gameSelectScreen.Q<VisualElement>("game-select-center");
        gameSelectBottom = gameSelectScreen.Q<VisualElement>("game-select-bottom");

        // minigame title text
        minigameTitle = gameSelectBottom.Q<Label>("minigame-text");

        // buttons on the minigame select screen
        closeMinigameBtn = gameSelectTop.Q<Button>("minigame-back-button");
        prevBtn = gameSelectCenter.Q<Button>("previous-button");
        prevBtn.style.display = DisplayStyle.None;
        nextBtn = gameSelectCenter.Q<Button>("next-button");
        nextBtn.style.display = DisplayStyle.None;
        playMinigameBtn = gameSelectBottom.Q<Button>("play-minigame-button");
        infoBtn = gameSelectScreen.Q<Button>("info-button");

        // INFO PANEL TABS
        infoPanel = infoScreen.Q<VisualElement>("info-panel");
        tabs = infoPanel.Q<VisualElement>("tabs");
        instructionsTab = tabs.Q<Label>("Instructions");
        contextTab = tabs.Q<Label>("science-context");
        infoScrollView = infoPanel.Q<ScrollView>("game-info");
        closeInfoBtn = infoPanel.Q<Button>("close-button");

        ////////////////////////////////////////////////////////////////////////////////
        // OPTIONS SCREEN UI ELEMENTS
        optionsPanel = optionsScreen.Q<VisualElement>("options-panel");
        soundbar = optionsPanel.Q<VisualElement>("sound-bar");
        optionsButtonContainer = optionsPanel.Q<VisualElement>("button-container");
        musicSlider = soundbar.Q<Slider>("music-slider");
        soundSlider = soundbar.Q<Slider>("sound-slider");
        InitializeSlider(musicSlider, 0);
        InitializeSlider(soundSlider, 1);
        closeOptionsBtn = optionsButtonContainer.Q<Button>("close-button");
        
        ////////////////////////////////////////////////////////////////////////////////
        // CREDITS SCREEN UI ELEMENTS
        closeCreditsBtn = creditsScreen.Q<Button>("close-button");
        creditsScrollView = creditsScreen.Q<ScrollView>("credits-content");
        ShowCredits();
        DisableKeyboardNavigation();
    }

    private void BindUIEvents()
    {
        // Main Menu Buttons
        playBtn.clicked += () => { Play(); PlaySound(); };
        gameSelectBtn.clicked += () => { MinigameSelectClicked(); PlaySound(); };
        OptionsBtn.clicked += () => { OptionsClicked(); PlaySound(); };
        CreditsBtn.clicked += () => { SwitchScreen(creditsScreen); PlaySound(); };

        // Minigame Select Screen
        closeMinigameBtn.clicked += () =>
        {
            SwitchScreen(mainScreen);
            minigameTitle.visible = false;
            MinigameSelectMenu.SetActive(false);
            Console.SetActive(false);
            slideCamera.ResetPosition();
            Animation.SetActive(true);
            prevBtn.style.display = DisplayStyle.None;
            nextBtn.style.display = DisplayStyle.None;
            PlaySound();
            titleController.gameSelect = false;
        };

        prevBtn.clicked += () => {PrevMinigame(); PlaySound();};
        nextBtn.clicked += () => { NextMinigame(); PlaySound();};
        playMinigameBtn.clicked += () => { 
            PlayMinigame();
            PlaySound();
            closeMinigameBtn.SetEnabled(false);
        };
        infoBtn.clicked += () => { OpenInfoPanel(); PlaySound(); };
        closeInfoBtn.clicked += () => { CloseInfoPanel(); PlaySound(); };

        // Options Screen
        musicSlider.RegisterCallback<ChangeEvent<float>>(musicValueChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(soundValueChanged);

        closeOptionsBtn.clicked += () => { SwitchScreen(mainScreen); PlaySound(); };
        
        // Credits Screen
        closeCreditsBtn.clicked += () => { SwitchScreen(mainScreen); PlaySound(); };
    }

    public void SetMinigameText(string text)
    {
        minigameTitle.text = text;
    }


    private void SwitchScreen(VisualElement returnScreen)
    {
        foreach (VisualElement screen in screens)
        {
            screen.visible = false;
        }

        returnScreen.visible = true;
    }

    private void PlaySound()
    {
        SoundManager.Instance.PlaySound(clip);
    }
    private void Play()
    {
        // Go to orbit game with first orbit
        lastSceneIndex.Num = 0;
        SceneManager.LoadScene(titleController.getSceneName(4));
    }

    private void UpdateMinigameScreen()
    {
        // update the minigame text
        titleController.updateMinigame(minigameTitle);
        
        // if the minigame is the first minigame, disable the previous button
        if (titleController.isFirstScene())
        {
            prevBtn.visible = false;
        }
        else
        {
            prevBtn.visible = true;
        }

        // if the minigame is the last minigame, disable the next button
        if (titleController.isLastScene())
        {
            nextBtn.visible = false;
        }
        else
        {
            nextBtn.visible = true;
        }
        RegisterTabCallbacks();
    }
    private void MinigameSelectClicked()
    {
        // Hides the animation
        Animation.SetActive(false);
        
        // show the minigame select menu
        MinigameSelectMenu.SetActive(true);
        Console.SetActive(true);

        // hide the screens
        mainScreen.visible = false;

        // set minigame select menu to active
        gameSelectScreen.visible = true;
        minigameTitle.visible = true;
        prevBtn.style.display = DisplayStyle.Flex;
        nextBtn.style.display = DisplayStyle.Flex;
        titleController.minigameSelect(minigameTitle);
        titleController.gameSelect = true;
    }

    private void PrevMinigame()
    {
        titleController.getPrevScene();
        slideCamera.movePrevPos();
    }
    private void NextMinigame()
    {
        titleController.getNextScene();
        slideCamera.moveNextPos();
    }

    private void PlayMinigame()
    {
        cameraZoom.startCameraMove(titleController.getScene());
    }

    public void OpenInfoPanel()
    {
        HandleTabSeleted(instructionsTab);
        infoScreen.style.display = DisplayStyle.Flex;
        blackScreen.visible = true;
        titleController.inInfoPanel = true;
    }

    public void CloseInfoPanel()
    {
        infoScreen.style.display = DisplayStyle.None;
        blackScreen.visible = false;
        GetAllTabs().ForEach(UnselectTab);
        titleController.inInfoPanel = false;
        
        // Reset the scroll view to the top
        infoScrollView.scrollOffset = Vector2.zero;
    }

    private void OptionsClicked()
    {
        SwitchScreen(optionsScreen);
    }

    // Slider Functions
    private void musicValueChanged(ChangeEvent<float> evt)
    {
        GameObject musicSource = SoundManager.Instance.transform.GetChild(0).gameObject;
        AudioSource audioSource = musicSource.GetComponent<AudioSource>();
        audioSource.volume = evt.newValue / 100;
    }
    private void soundValueChanged(ChangeEvent<float> evt)
    {
        GameObject soundSource = SoundManager.Instance.transform.GetChild(1).gameObject;
        AudioSource audio = soundSource.GetComponent<AudioSource>();
        audio.volume = evt.newValue / 100;
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

    ////////////////////////////////////////////////////////
    // Info panel tabs methods
    public void RegisterTabCallbacks()
    {
        UQueryBuilder<Label> tabs = GetAllTabs();
        tabs.ForEach((Label tab) => {
            tab.RegisterCallback<ClickEvent>(TabOnClick);
        });
    }
    private void TabOnClick(ClickEvent evt)
    {
        PlaySound();
        Label clickedTab = evt.currentTarget as Label;
        HandleTabSeleted(clickedTab);
    }
     private static bool TabIsCurrentlySelected(Label tab)
    {
        return tab.ClassListContains("selectedTab");
    }

    private void SelectTab(Label tab)
    {   
        // reset scroll position when tab is switched
        infoScrollView.scrollOffset = Vector2.zero;
        
        tab.AddToClassList("selectedTab");
        if(tab.name == "Instructions")
        {
            LoadInstruction(minigameTitle.text);
        }
        else if(tab.name == "science-context")
        {
            LoadContext(minigameTitle.text);
        }
    }

    private void HandleTabSeleted(Label clickedTab)
    {
        if (!TabIsCurrentlySelected(clickedTab))
        {
            GetAllTabs().Where(
                (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)
            ).ForEach(UnselectTab);
            SelectTab(clickedTab);
        }
    }

    private UQueryBuilder<Label> GetAllTabs()
    {
        return root.Query<Label>(className: "tab");
    }

    private void UnselectTab(Label tab)
    {
        tab.RemoveFromClassList("selectedTab");
    }

    public void LoadInstruction(string minigameName)
    {
        string infoUxmlPath = $"UI/UXML/{minigameName}Info";
        VisualTreeAsset gameInfoTree = Resources.Load<VisualTreeAsset>(infoUxmlPath);


        if (gameInfoTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInfoTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
        else {}
    }

    public void LoadContext(string minigameName)
    {
        string infoUxmlPath = $"UI/UXML/{minigameTitle.text}Context";
        VisualTreeAsset gameInfoTree = Resources.Load<VisualTreeAsset>(infoUxmlPath);

        if (gameInfoTree != null)
        {
            infoScrollView.contentContainer.Clear();
            VisualElement gameInfoContent = gameInfoTree.Instantiate();
            infoScrollView.contentContainer.Add(gameInfoContent);
        }
        else{ }
    }
    
    private void ShowCredits()
    {
        string creditsUxmlPath = $"UI/UXML/Credits";
        VisualTreeAsset creditsTree = Resources.Load<VisualTreeAsset>(creditsUxmlPath);


        if (creditsTree != null)
        {
            VisualElement creditsContent = creditsTree.Instantiate();
            creditsScrollView.contentContainer.Add(creditsContent);
        }
        else{}
    }
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

}