using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenUI : MonoBehaviour
{
    private Button optionsBtn, continueBtn, cancelBtn;
    VisualElement root, gameScreen, optionsScreen, gameBottomContainer, gameTopContainer, gameButtonContainer, optionsContainer, optionsContainerBottom;
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
        optionsContainer = optionsScreen.Q<VisualElement>("options-container");
        optionsContainerBottom = optionsContainer.Q<VisualElement>("bottom-container");
        // 
        optionsBtn = gameButtonContainer.Q<Button>("options-button");
        optionsBtn.clicked += () => optionsButtonClicked();
        cancelBtn = optionsContainerBottom.Q<Button>("cancel-button");
        cancelBtn.clicked += () => cancel();

        continueBtn = gameButtonContainer.Q<Button>("continue-button");
        continueBtn.clicked += () => continueButtonClicked();


    }
    private void optionsButtonClicked()
    {
        optionsScreen.visible = true;
        gameScreen.visible = false;
    }

    private void continueButtonClicked()
    {

    }
    private void cancel()
    {
        optionsScreen.visible = false;
        gameScreen.visible = true;
    }
}