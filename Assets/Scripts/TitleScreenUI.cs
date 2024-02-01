using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenUI : MonoBehaviour 
{
    public ChangeScene SceneChanger;
    public TitleController titleController;
    public GameObject MinigameSelectMenu, Canvas, Console;
    private Button playButton, gameSelectButton, OptionsButton, CreditsButton;
    private VisualElement root, mainScreen, gameSelectScreen, optionsScreen, creditsScreen;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        mainScreen = root.Q<VisualElement>("MainMenuScreen");
        gameSelectScreen = root.Q<VisualElement>("GameSelectScreen");
        optionsScreen = root.Q<VisualElement>("OptionsScreen");
        creditsScreen = root.Q<VisualElement>("CreditsScreen");
        // optionsWindow = root.Q<VisualElement>("OptionsWindow");

        optionsScreen.visible = false;

        playButton = mainScreen.Q<Button>("PlayButton");
        gameSelectButton = mainScreen.Q<Button>("GameSelectButton");
        OptionsButton = mainScreen.Q<Button>("OptionsButton");
        CreditsButton = mainScreen.Q<Button>("CreditsButton");

        playButton.clicked += () => playButtonClicked();
        gameSelectButton.clicked += () => minigameSelectClicked();
        OptionsButton.clicked += () => optionsButtonClicked();

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
    }
    private void optionsButtonClicked()
    {
        mainScreen.visible = false;
        optionsScreen.visible = true;
    }
}