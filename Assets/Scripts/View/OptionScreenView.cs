using UnityEngine;
using UnityEngine.UIElements;

public class OptionsScreenView : MonoBehaviour
{
    public AudioClip clip;
    private VisualElement root, optionsPanel, soundBar, buttonContainer;
    private Slider musicSlider, soundSlider;
    private Button cancelBtn, applyBtn;
    void OnEnable()
    {
        InitializeUI();
        BindUIEvents();
    }
    private void InitializeUI()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        optionsPanel = root.Q<VisualElement>("options-panel");
        soundBar = optionsPanel.Q<VisualElement>("sound-bar");
        buttonContainer = optionsPanel.Q<VisualElement>("button-container");
        musicSlider = soundBar.Q<Slider>("music-slider");
        soundSlider = soundBar.Q<Slider>("sound-slider");
        cancelBtn = optionsPanel.Q<Button>("cancel-button");
        applyBtn = buttonContainer.Q<Button>("apply-button");

        hideOptionsScreen();
    }

    private void BindUIEvents()
    {
        musicSlider.RegisterCallback<ChangeEvent<float>>(musicValueChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(soundValueChanged);
        cancelBtn.clicked += () => { hideOptionsScreen(); playSound(); };
        
    }
    public void hideOptionsScreen()
    {
        optionsPanel.visible = false;
    }
    public void ShowOptionsScreen()
    {
        optionsPanel.visible = true;
    }

    private void musicValueChanged(ChangeEvent<float> evt)
    {
        Debug.Log("Slider value changed: " + evt.newValue);
        GameObject musicSource = SoundManager.Instance.transform.GetChild(0).gameObject;
        Debug.Log("Music source name: " + musicSource.name); // Log the name of the music source
        AudioSource audioSource = musicSource.GetComponent<AudioSource>();
        audioSource.volume = evt.newValue/100;
    }
    private void soundValueChanged(ChangeEvent<float> evt)
    {
        Debug.Log("Slider value changed: " + evt.newValue);
        GameObject soundSource = SoundManager.Instance.transform.GetChild(1).gameObject;
        Debug.Log("Sound source name: " + soundSource.name); // Log the name of the sound source
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();
        audioSource.volume = evt.newValue/100;
    }
    private void playSound()
    {
        SoundManager.Instance.PlaySound(clip);
    }
}