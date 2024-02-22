using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenUI : MonoBehaviour
{
    private Button optionsButton, continueButton;
    VisualElement root, gameScreen, optionsScreen, gameBottomContainer, gameTopContainer, gameButtonContainer, optionsPopup;
    private void OnEnable()
    {
        // initializing visual elements
        root = GetComponent<UIDocument>().rootVisualElement;
        // game screen
        gameScreen = root.Q<VisualElement>("game-screen");
        gameTopContainer = gameScreen.Q<VisualElement>("game-top-container");
        gameBottomContainer = gameScreen.Q<VisualElement>("game-bottom-container");
        gameButtonContainer = gameBottomContainer.Q<VisualElement>("button-container");
        // options screen
        optionsScreen = root.Q<VisualElement>("options-screen");
        optionsPopup = optionsScreen.Q<VisualElement>("options-container");

        // 
        optionsButton = gameButtonContainer.Q<Button>("options-button");
        optionsButton.clicked += () => optionsButtonClicked();

        continueButton = gameButtonContainer.Q<Button>("continue-button");
        continueButton.clicked += () => continueButtonClicked();


    }
    private void optionsButtonClicked()
    {
        if (optionsButton != null)
        {
            optionsScreen.visible = true;
            gameScreen.style.opacity = 0.5f;
            Debug.Log("Options button clicked!");
        }
        else
        {
            Debug.Log("Options button not found!");
        }

    }

    private void continueButtonClicked()
    {

    }
}