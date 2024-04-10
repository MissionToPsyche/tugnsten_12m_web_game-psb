using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioClip thrusterClip;
    private AudioClip lightThrusterClip;
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
        lightThrusterClip = (AudioClip)Resources.Load("Sounds/thrusters_loopwav-14699.mp3");
    }

    public void PlaySound(AudioClip clip) {
        if(effectSource != null && clip != null) 
        {
            effectSource.PlayOneShot(clip);
        }
    }

    public void playThrusterSound()
    {
        effectSource.loop = true;
        PlaySound(thrusterClip);
    }

    public void stopThrusterSound()
    {
        effectSource.loop = false;
        effectSource.Stop();
    }
    
    public void playLightThrusterSound()
    {
        effectSource.loop = true;
        PlaySound(lightThrusterClip);
    }

    public void stopLightThrusterSound()
    {
        effectSource.loop = false;
        effectSource.Stop();
    }
}
