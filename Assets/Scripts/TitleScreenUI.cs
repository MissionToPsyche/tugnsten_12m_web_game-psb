using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenUI : MonoBehaviour 
{
    public ChangeScene SceneChanger;
    public TitleController titleController;
    private TextController textController;
    public GameObject MinigameSelectMenu, Canvas, Console;
    public Label minigameText;
    private Button playButton, gameSelectButton, OptionsButton, CreditsButton, minigameBackButton, playMinigameButton;
    private VisualElement root, mainScreen, gameSelectScreen, gameSelectTop, gameSelectBottom, optionsScreen, creditsScreen,  optionsPopup;

    private void OnEnable()
    {
        // initializing the screens and buttons on the main screen
        root = GetComponent<UIDocument>().rootVisualElement;
        mainScreen = root.Q<VisualElement>("main-menu-screen");
        gameSelectScreen = root.Q<VisualElement>("game-select-screen");
        optionsScreen = root.Q<VisualElement>("options-screen");
        // creditsScreen = root.Q<VisualElement>("credits-screen");

        playButton = mainScreen.Q<Button>("play-button");
        gameSelectButton = mainScreen.Q<Button>("game-select-button");
        OptionsButton = mainScreen.Q<Button>("options-button");
        CreditsButton = mainScreen.Q<Button>("credits-button");

        playButton.clicked += () => playButtonClicked();
        gameSelectButton.clicked += () => minigameSelectClicked();
        OptionsButton.clicked += () => optionsButtonClicked();

        // MINIGAME SELECT SCREEN
        // initializing UI elements in this screen
        gameSelectTop = gameSelectScreen.Q<VisualElement>("game-select-top");
        gameSelectBottom = gameSelectScreen.Q<VisualElement>("game-select-bottom");

        minigameBackButton = gameSelectTop.Q<Button>("minigame-back-button");
        minigameBackButton.clicked += () => backButtonClicked();

        minigameText = gameSelectBottom.Q<Label>("minigame-text");
        textController.setText(minigameText.text);
        playMinigameButton = gameSelectBottom.Q<Button>("play-minigame-button");

        // initializing buttons on the game select screen
        optionsPopup = optionsScreen.Q<VisualElement>("options-container");

    }

    private void backButtonClicked()
    {
        mainScreen.visible = true;
        gameSelectScreen.visible = false;
        optionsScreen.visible = false;
        // creditsScreen.visible = false;

        MinigameSelectMenu.SetActive(false);
        Canvas.SetActive(false);
        Console.SetActive(false);
    }

    private void playButtonClicked()
    {
        SceneChanger.NextScene("Magnetometer_minigame");
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

        titleController.minigameSelect();
        // titleController.updateMinigame(minigameText);
        
    }
    private void optionsButtonClicked()
    {
        mainScreen.visible = false;
        optionsScreen.visible = true;
        // creditsScreen.visible = false;
    }
}