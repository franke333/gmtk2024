using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : SingletonClass<AudioManager>
{

    [SerializeField]
    private AudioClip walkSound, music, musicBoss, eat, cardselect, damageTaken, gameOver, levelUp;

    private AudioSource walkSource, musicSource, eatSource, cardSelectSource, damageTakenSource, gameOverSource, levelUpSource;

    public bool Walking;

    public Slider slider;

    static float VolumeStatic = -1f;

    public float Volume
    {
        get
        {
            return AudioListener.volume;
        }
        set
        {
            VolumeStatic = value;
            AudioListener.volume = value;
        }
    }

    public void PlayEat()
    {
        eatSource.clip = eat;
        eatSource.Play();
    }

    public void PlayCardSelection()
    {
        cardSelectSource.clip = cardselect;
        cardSelectSource.Play();
    }

    public void PlayDamageTaken()
    {
        damageTakenSource.clip = damageTaken;
        damageTakenSource.Play();
    }

    public void PlayGameOver()
    {
        gameOverSource.clip = gameOver;
        gameOverSource.Play();
    }

    public void PlayLevelUp()
    {
        levelUpSource.clip = levelUp;
        levelUpSource.Play();
    }

    public void StartBossMusic()
    {
        musicSource.clip = musicBoss;
        musicSource.Play();
    }


    private void Update()
    {
        if (Walking)
        {
            if (!walkSource.isPlaying)
            {
                walkSource.clip = walkSound;
                walkSource.Play();
            }
        }
        else
        {
            walkSource.Stop();
        }
    }


    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    private void Start()
    {

        walkSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        eatSource = gameObject.AddComponent<AudioSource>();
        cardSelectSource = gameObject.AddComponent<AudioSource>();
        damageTakenSource = gameObject.AddComponent<AudioSource>();
        gameOverSource = gameObject.AddComponent<AudioSource>();
        levelUpSource = gameObject.AddComponent<AudioSource>();


        musicSource.loop = true;
        musicSource.volume = 0.5f;

        walkSource.volume = 0.03f;

        walkSource.loop = true;

        if (VolumeStatic != -1f)
        {
            Volume = VolumeStatic;
        }
        else
        {
            Volume = 0.5f;
        }

        slider.value = Volume;

        PlayMusic();
    }

}