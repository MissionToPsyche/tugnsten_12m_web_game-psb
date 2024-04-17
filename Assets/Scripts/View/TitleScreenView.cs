using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenView : MonoBehaviour
{ // Public variables for editor assignment
    public ChangeScene SceneChanger;
    public TitleController titleController;
    public AudioClip clip;
    public GameObject MinigameSelectMenu, Console, Animation;
    // public OptionsScreenView optionsScreenView;

    // Private UI elements grouped by their functionality/screen
    private VisualElement root, mainScreen, buttonContainer, gameSelectScreen, gameSelectTop, gameSelectCenter, gameSelectBottom, optionsScreen, optionsPanel, soundbar, optionsButtonContainer, creditsScreen, blackScreen, infoPanel, infoScrollView, tabs, scorePanel, scoreContainer;

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
        // if (gameSelectScreen.visible)
        // {
            updateMinigameScreen();
        // }

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
        infoPanel = root.Q<VisualElement>("info-panel");
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

        // buttons on the minigame select screen
        closeMinigameBtn = gameSelectTop.Q<Button>("minigame-back-button");
        prevBtn = gameSelectCenter.Q<Button>("previous-button");
        prevBtn.style.display = DisplayStyle.None;
        nextBtn = gameSelectCenter.Q<Button>("next-button");
        nextBtn.style.display = DisplayStyle.None;
        playMinigameBtn = gameSelectBottom.Q<Button>("play-minigame-button");
        infoBtn = gameSelectScreen.Q<Button>("info-button");
        closeInfoBtn = infoPanel.Q<Button>("close-button");

        // INFO PANEL TABS
        tabs = infoPanel.Q<VisualElement>("tabs");
        instructionsTab = tabs.Q<Label>("Instructions");
        contextTab = tabs.Q<Label>("science-context");
        infoScrollView = infoPanel.Q<ScrollView>("game-info");
        closeInfoBtn = infoPanel.Q<Button>("close-button");

        // minigame title text
        minigameTitle = gameSelectBottom.Q<Label>("minigame-text");

        ////////////////////////////////////////////////////////////////////////////////
        // OPTIONS SCREEN UI ELEMENTS
        optionsPanel = optionsScreen.Q<VisualElement>("options-panel");
        soundbar = optionsPanel.Q<VisualElement>("sound-bar");
        optionsButtonContainer = optionsPanel.Q<VisualElement>("button-container");
        musicSlider = soundbar.Q<Slider>("music-slider");
        soundSlider = soundbar.Q<Slider>("sound-slider");
        closeOptionsBtn = optionsButtonContainer.Q<Button>("close-button");
        
        ////////////////////////////////////////////////////////////////////////////////
        // CREDITS SCREEN UI ELEMENTS
         closeCreditsBtn = creditsScreen.Q<Button>("close-button");
        // optionsScreenView.hideOptionsScreen();
    }

    private void BindUIEvents()
    {
        // Main Menu Buttons
        playBtn.clicked += () => { play(); playSound(); };
        gameSelectBtn.clicked += () => { minigameSelectClicked(); playSound(); };
        OptionsBtn.clicked += () => { optionsClicked(); playSound(); };
        CreditsBtn.clicked += () => { switchScreen(creditsScreen); playSound(); };

        // Minigame Select Screen
        closeMinigameBtn.clicked += () =>
        {
            switchScreen(mainScreen);
            minigameTitle.visible = false;
            MinigameSelectMenu.SetActive(false);
            Console.SetActive(false);
            slideCamera.ResetPosition();
            Animation.SetActive(true);
            prevBtn.style.display = DisplayStyle.None;
            nextBtn.style.display = DisplayStyle.None;
            playSound();
        };

        prevBtn.clicked += () => {prevMinigame(); playSound();};
        nextBtn.clicked += () => { nextMinigame(); playSound();};
        playMinigameBtn.clicked += () => { 
            playMinigame();
            playSound();
            closeMinigameBtn.SetEnabled(false);
        };
        infoBtn.clicked += () => { openInfoPanel(); playSound(); };
        // RegisterTabCallbacks(); // Register the tab callbacks
        closeInfoBtn.clicked += () => { closeInfoPanel(); playSound(); };

        // Options Screen
        musicSlider.RegisterCallback<ChangeEvent<float>>(musicValueChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(soundValueChanged);
        closeOptionsBtn.clicked += () => { switchScreen(mainScreen); playSound(); };
        
        // Credits Screen
        closeCreditsBtn.clicked += () => { switchScreen(mainScreen); playSound(); };
    }

    public void setMinigameText(string text)
    {
        minigameTitle.text = text;
    }


    private void switchScreen(VisualElement returnScreen)
    {
        foreach (VisualElement screen in screens)
        {
            screen.visible = false;
        }

        returnScreen.visible = true;
    }

    private void playSound()
    {
        SoundManager.Instance.PlaySound(clip);
    }
    private void play()
    {
        // minigameText.text = titleController.getScene();
        
        // Go to orbit game with first orbit
        lastSceneIndex.Num = 0;
        SceneManager.LoadScene(titleController.getSceneName(4));
    }

    private void updateMinigameScreen()
    {
        // update the minigame text
        titleController.updateMinigame(minigameTitle);

        // if the minigame is the first minigame, disable the previous button
        if (titleController.isFirstScene())
        {
            // prevBtn.style.display = DisplayStyle.None;
            prevBtn.visible = false;
        }
        else
        {
            prevBtn.visible = true;
        }

        // if the minigame is the last minigame, disable the next button
        if (titleController.isLastScene())
        {
            // nextBtn.style.display = DisplayStyle.None;
            nextBtn.visible = false;
        }
        else
        {
            nextBtn.visible = true;
        }
        RegisterTabCallbacks();
    }
    private void minigameSelectClicked()
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
    }

    private void prevMinigame()
    {
        titleController.getPrevScene();
        slideCamera.movePrevPos();
    }
    private void nextMinigame()
    {
        titleController.getNextScene();
        slideCamera.moveNextPos();
    }

      private void playMinigame()
    {   
        Debug.Log("change to: " + titleController.getScene());
        cameraZoom.startCameraMove(titleController.getScene());
    }

    public void openInfoPanel()
    {
        handleTabSeclected(instructionsTab);
        infoPanel.visible = true;
        blackScreen.visible = true;
    }

    public void closeInfoPanel()
    {
        infoPanel.visible = false;
        blackScreen.visible = false;

        //electTab(instructionsTab);
        //TabIsCurrentlySelected(instructionsTab);
        //UnselectTab(contextTab);
    }

    private void optionsClicked()
    {
        switchScreen(optionsScreen);
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
        Label clickedTab = evt.currentTarget as Label;
        handleTabSeclected(clickedTab);
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

    private void handleTabSeclected(Label clickedTab)
    {
        Debug.Log("tab: " + TabIsCurrentlySelected(clickedTab));
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
        Debug.Log("Unselected tab");
        // tab.AddToClassList("unselectedTab");
    }

    public void showInfo()
    {
        Debug.Log("text: " + minigameTitle.text);
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

}