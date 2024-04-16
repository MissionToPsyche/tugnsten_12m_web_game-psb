using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenView : MonoBehaviour
{ // Public variables for editor assignment
    public ChangeScene SceneChanger;
    public TitleController titleController;
    public AudioClip clip;
    public GameObject MinigameSelectMenu, Console, Animation;
    // public OptionsScreenView optionsScreenView;

    // Private UI elements grouped by their functionality/screen
    private VisualElement root, mainScreen, buttonContainer, gameSelectScreen, gameSelectTop, gameSelectCenter, gameSelectBottom, optionsScreen, optionsPanel, soundbar, optionsButtonContainer, creditsScreen, blackScreen, infoPanel, tabs, scorePanel, scoreContainer;

    private Button playBtn, gameSelectBtn, OptionsBtn, CreditsBtn, closeMinigameBtn, playMinigameBtn, closeOptionsBtn, closeCreditsBtn, nextBtn, prevBtn, infoBtn, closeInfoBtn;

    private Slider musicSlider, soundSlider;
    private Label minigameText;
    private CameraZoom cameraZoom;
    private SlideCamera slideCamera;

    // Collections for easier management
    private List<VisualElement> screens = new List<VisualElement>();

    private void OnEnable()
    {
        InitializeUI();
        BindUIEvents();
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

        // minigame title text
        minigameText = gameSelectBottom.Q<Label>("minigame-text");

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
            minigameText.visible = false;
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
        infoBtn.clicked += () => openInfoPanel();
        closeInfoBtn.clicked += () => { infoPanel.visible = false; blackScreen.visible = false; playSound(); };

        // Options Screen
        musicSlider.RegisterCallback<ChangeEvent<float>>(musicValueChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(soundValueChanged);
        closeOptionsBtn.clicked += () => { switchScreen(mainScreen); playSound(); };
        
        // Credits Screen
        closeCreditsBtn.clicked += () => { switchScreen(mainScreen); playSound(); };
    }

    public void setMinigameText(string text)
    {
        minigameText.text = text;
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
        minigameText.text = titleController.getScene();
        SceneChanger.NextScene(minigameText.text);
    }

    private void updateMinigameScreen()
    {
        // update the minigame text
        titleController.updateMinigame(minigameText);

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
        minigameText.visible = true;
        prevBtn.style.display = DisplayStyle.Flex;
        nextBtn.style.display = DisplayStyle.Flex;
        titleController.minigameSelect(minigameText);
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
        infoPanel.visible = true;
        blackScreen.visible = true;
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
}