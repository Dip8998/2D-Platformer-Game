using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController instance;
    public static SoundController Instance { get { return instance; } }

    public AudioSource soundEffect;
    public AudioSource soundMusic;
    
    public bool isMute = false;
    public float Volume = 1.0f;
    public SoundType[] sounds;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic(Sounds.Music);
        SetVolume(Volume);
    }

    private void SetVolume(float volume)
    {
        soundMusic.volume = volume;
    }

    public void PlayMusic(Sounds sound)
    {
        if (isMute)
            return;

        AudioClip clip = GetSoundClip(sound);

        if(clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound type: " + sound);
        }
    }
    
    public void Play(Sounds sound)
    {
        if (isMute)
            return;

        AudioClip clip = GetSoundClip(sound);

        if(clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound type: " +  sound);
        }
    }

    private AudioClip GetSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(sounds, i => i.soundTypes == sound);

        if(item != null)
        {
            return item.audioClip;
        }

        return null;
    }

  

    [Serializable]
    public class SoundType
    {
        public Sounds soundTypes;
        public AudioClip audioClip;
    }

    public enum Sounds
    {
        BackButtonClick,
        StartButtonClick,
        LevelButtonClick,
        ConfirmButtonClick,
        Music,
        PlayerMove,
        PlayerDeath,
        PlayerJump,
        PlayerHurt,
        KeyPickUp,
        HealthPickup,
        LevelComplete,
        GameOver,
        ShieldPickUp,
        DoubleJumpPickUp,
        ShieldSound 
    }
}
