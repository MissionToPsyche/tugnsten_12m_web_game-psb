using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenView : MonoBehaviour
{ // Public variables for editor assignment
    public ChangeScene SceneChanger;
    public TitleController titleController;
    public AudioClip clip;
    public GameObject MinigameSelectMenu, Console;
    public OptionsScreenView optionsScreenView;

    // Private UI elements grouped by their functionality/screen
    private VisualElement root, mainScreen, buttonContainer, gameSelectScreen, gameSelectTop, gameSelectCenter, gameSelectBottom, optionsScreen, optionsPanel, soundbar, optionsButtonContainer, creditsScreen;

    private Button playBtn, gameSelectBtn, OptionsBtn, CreditsBtn, minigameBackBtn, playMinigameBtn, cancelBtn, closeBtn, nextBtn, prevBtn;

    private Slider musicSlider, soundSlider;
    private Label minigameText;
    private CameraZoom cameraZoom;


    // Collections for easier management
    private List<VisualElement> screens = new List<VisualElement>();

    private void OnEnable()
    {
        InitializeUI();
        BindUIEvents();
    }

    void Update()
    {
        titleController.updateMinigame(minigameText);
    }

    private void InitializeUI()
    {   
        ////////////////////////////////////////////////////////////////////////////////
        // MAIN SCREEN UI ELEMENTS
        cameraZoom = Camera.main.GetComponent<CameraZoom>();
        cameraZoom.setTitleController(titleController);

        root = GetComponent<UIDocument>().rootVisualElement;
        mainScreen = root.Q<VisualElement>("main-menu-screen");
        gameSelectScreen = root.Q<VisualElement>("game-select-screen");
        optionsScreen = root.Q<VisualElement>("options-screen");
        creditsScreen = root.Q<VisualElement>("credits-screen");
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
        minigameBackBtn = gameSelectTop.Q<Button>("minigame-back-button");
        prevBtn = gameSelectCenter.Q<Button>("previous-button");
        nextBtn = gameSelectCenter.Q<Button>("next-button");
        playMinigameBtn = gameSelectBottom.Q<Button>("play-minigame-button");
        
        // minigame title text
        minigameText = gameSelectBottom.Q<Label>("minigame-text");

        ////////////////////////////////////////////////////////////////////////////////
        // OPTIONS SCREEN UI ELEMENTS
        optionsPanel = optionsScreen.Q<VisualElement>("options-panel");
        soundbar = optionsPanel.Q<VisualElement>("sound-bar");
        optionsButtonContainer = optionsPanel.Q<VisualElement>("options-button-container");
        
        cancelBtn = optionsButtonContainer.Q<Button>("cancel-button");

        musicSlider = soundbar.Q<Slider>("music-slider");
        soundSlider = soundbar.Q<Slider>("sound-slider");

        ////////////////////////////////////////////////////////////////////////////////
        // CREDITS SCREEN UI ELEMENTS
         closeBtn = creditsScreen.Q<Button>("close-button");
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
        minigameBackBtn.clicked += () =>
        {
            switchScreen(mainScreen);
            minigameText.visible = false;
            MinigameSelectMenu.SetActive(false);
            Console.SetActive(false);
        };

        playMinigameBtn.clicked += () => playMinigame();

        // Options Screen
        musicSlider.RegisterCallback<ChangeEvent<float>>(musicValueChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(soundValueChanged);
        cancelBtn.clicked += () => switchScreen(mainScreen);
        

        // Credits Screen
        closeBtn.clicked += () => switchScreen(mainScreen);
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

    private void minigameSelectClicked()
    {
        // show the minigame select menu
        MinigameSelectMenu.SetActive(true);
        Console.SetActive(true);

        // hide the screens
        mainScreen.visible = false;

        // set minigame select menu to active
        gameSelectScreen.visible = true;
        minigameText.visible = true;
        titleController.minigameSelect(minigameText);
    }

      private void playMinigame()
    {   
        // minigameText.text = titleController.getScene();
        
        Debug.Log("change to: " + titleController.getScene());
        cameraZoom.startCameraMove(titleController.getScene());
    }

    private void optionsClicked()
    {
        switchScreen(optionsScreen);
        // optionsScreenView.ShowOptionsScreen();
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