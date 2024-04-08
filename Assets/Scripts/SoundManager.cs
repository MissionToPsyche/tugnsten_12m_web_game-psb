using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
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
    }

    public void PlaySound(AudioClip clip) {
        if(effectSource != null && clip != null) 
        {
            effectSource.PlayOneShot(clip);
        }
    }
}
