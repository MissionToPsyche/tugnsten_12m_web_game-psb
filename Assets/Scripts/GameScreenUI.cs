using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenUI : MonoBehaviour 
{
    private Button optionsButton, continueButton;
    VisualElement root, gameScreen, optionsScreen;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        gameScreen = root.Q<VisualElement>("game-screen");
        optionsScreen = root.Q<VisualElement>("options-screen");
        optionsButton = gameScreen.Q<Button>("options-button");
        continueButton = gameScreen.Q<Button>("continue-button");
        optionsButton.clicked += () => optionsButtonClicked();
        // continueButton.clicked += () => continueButtonClicked();
    }
        private void optionsButtonClicked()
    {
        gameScreen.visible = false;
        optionsScreen.visible = true;
    }
}