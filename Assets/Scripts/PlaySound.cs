using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    public AudioSource audio;
    void Start(){
        SoundManager.Instance.PlaySound(clip);
    }

    public void PlayButton()
    {
        audio.Play();
    }
}
