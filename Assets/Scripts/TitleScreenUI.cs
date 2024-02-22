using System.Collections.Generic;
using System.Threading;
using Codice.Client.Common.GameUI;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenUI : MonoBehaviour 
{
    public ChangeScene SceneChanger;
    public TitleController titleController;
    public GameObject MinigameSelectMenu, Canvas, Console;
    private Label minigameText;
    private Button playBtn, gameSelectBtn, OptionsBtn, CreditsBtn, minigameBackBtn, playMinigameBtn, easyBtn, mediumBtn, hardBtn, cancelBtn, closeBtn;
    private VisualElement root, mainScreen, gameSelectScreen, gameSelectTop, gameSelectBottom, optionsScreen,  optionsContainer, soundbar, difficultyContainer, bottomContainer, creditsScreen;
    private List<VisualElement> screens = new List<VisualElement>();
    private void OnEnable()
    {
        // MAIN SCREEN UI ELEMENTS
        root = GetComponent<UIDocument>().rootVisualElement;
        mainScreen = root.Q<VisualElement>("main-menu-screen");
        gameSelectScreen = root.Q<VisualElement>("game-select-screen");
        optionsScreen = root.Q<VisualElement>("options-screen");
        creditsScreen = root.Q<VisualElement>("credits-screen");
        screens.Add(mainScreen);
        screens.Add(gameSelectScreen);
        screens.Add(optionsScreen);
        screens.Add(creditsScreen);

        playBtn = mainScreen.Q<Button>("play-button");
        gameSelectBtn = mainScreen.Q<Button>("game-select-button");
        OptionsBtn = mainScreen.Q<Button>("options-button");
        CreditsBtn = mainScreen.Q<Button>("credits-button");

        playBtn.clicked += () => playMinigameClicked();
        gameSelectBtn.clicked += () => minigameSelectClicked();
        OptionsBtn.clicked += () => optionsClicked();
        CreditsBtn.clicked += () => switchScreen(creditsScreen);

        // MINIGAME SELECT SCREEN UI ELEMENTS
        gameSelectTop = gameSelectScreen.Q<VisualElement>("game-select-top");
        gameSelectBottom = gameSelectScreen.Q<VisualElement>("game-select-bottom");

        minigameBackBtn = gameSelectTop.Q<Button>("minigame-back-button");
        minigameBackBtn.clicked += () => {
            switchScreen(mainScreen);
            minigameText.visible = false;
            MinigameSelectMenu.SetActive(false);
            Canvas.SetActive(false);
            Console.SetActive(false);
        };
        minigameText = gameSelectBottom.Q<Label>("minigame-text");
        playMinigameBtn = gameSelectBottom.Q<Button>("play-minigame-button");
        playMinigameBtn.clicked += () => playMinigameClicked();

        // OPTIONS SCREEN UI ELEMENTS
        optionsContainer = optionsScreen.Q<VisualElement>("options-container");
        soundbar = optionsContainer.Q<VisualElement>("sound-bar");
        difficultyContainer = optionsContainer.Q<VisualElement>("difficulty-container");
        bottomContainer = optionsContainer.Q<VisualElement>("bottom-container");
        
        cancelBtn = bottomContainer.Q<Button>("cancel-button");
        cancelBtn.clicked += () => switchScreen(mainScreen);


        // CREDITS SCREEN UI ELEMENTS
        closeBtn = creditsScreen.Q<Button>("close-button");
        closeBtn.clicked += () => switchScreen(mainScreen);
    }

    void Update()
    {
        titleController.updateMinigame(minigameText);
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

    private void playMinigameClicked()
    {
        minigameText.text = titleController.getNextScene();
        SceneChanger.NextScene(minigameText.text);
    }
    private void minigameSelectClicked()
    {
        // show the minigame select menu
        MinigameSelectMenu.SetActive(true);
        Canvas.SetActive(true);
        Console.SetActive(true);

        // hide the screens
        mainScreen.visible = false;

        // set minigame select menu to active
        gameSelectScreen.visible = true;
        minigameText.visible = true;
        titleController.minigameSelect(minigameText);
    }
    private void optionsClicked()
    {
        switchScreen(optionsScreen);
        // more implementation here
    }
}