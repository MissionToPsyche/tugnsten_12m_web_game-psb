using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioClip thrusterClip, lightThrusterClip;
    private bool maxThrustOn = false, lightThrustOn = false, sound_stopped = true;
    public static SoundManager Instance;
    
    [SerializeField] private AudioSource musicSource, effectSource;
    public void setEffectSource(AudioSource audioSource)
    {
        effectSource = audioSource;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        thrusterClip = (AudioClip)Resources.Load("Sounds/rocketthrustmaxx-100019");
        lightThrusterClip = (AudioClip)Resources.Load("Sounds/thrusters_loopwav-14699");
    }

    public void PlaySound(AudioClip clip) {
        if(effectSource != null && clip != null) 
        {
            stopSound();
            effectSource.PlayOneShot(clip);
        }
    }

    public void playThrusterSound()
    {
        if(!maxThrustOn)
        {
            stopSound();
            effectSource.loop = true;
            PlaySound(thrusterClip);
            maxThrustOn = true;
            lightThrustOn = false;
            sound_stopped = false;
        }
    }
    
    public void playLightThrusterSound()
    {
        if(!lightThrustOn)
        {
            stopSound();
            effectSource.loop = true;
            PlaySound(lightThrusterClip);
            lightThrustOn = true;
            maxThrustOn = false;
            sound_stopped = false;
        }
    }

    public void stopSound()
    {
        if (!sound_stopped) {
            effectSource.loop = false;
            effectSource.Stop();
            maxThrustOn = false;
            lightThrustOn = false;
            sound_stopped = true;
        }
    }
}
