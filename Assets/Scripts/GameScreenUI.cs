using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenUI : MonoBehaviour 
{
    private Button optionsButton, continueButton;
    VisualElement root, gameScreen, optionsScreen, gameBottomContainer, gameTopContainer, gameButtonContainer, optionsPopup;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // GAME SCREEN
        gameScreen = root.Q<VisualElement>("game-screen");
        gameTopContainer = gameScreen.Q<VisualElement>("game-top-container");
        gameBottomContainer = gameScreen.Q<VisualElement>("game-bottom-container");
        gameButtonContainer = gameBottomContainer.Q<VisualElement>("button-container");
        continueButton = gameButtonContainer.Q<Button>("continue-button");
        optionsButton = gameButtonContainer.Q<Button>("options-button");
        optionsButton.clicked += () => optionsButtonClicked();

        // OPTIONS SCREEN
        optionsScreen = root.Q<VisualElement>("options-screen");
        optionsPopup = optionsScreen.Q<VisualElement>("options-container");
    }
    private void optionsButtonClicked()
    {
        gameScreen.visible = false;
        optionsScreen.visible = true;
    }

    private void continueButtonClicked()
    {
        
    }
}