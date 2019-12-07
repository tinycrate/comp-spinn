using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameSoundManager : Manager<GameSoundManager> {
    public static AudioClip GameOverAudioClip { get; private set; }
    public static AudioClip JumpAudioClip { get; private set; }
    public static AudioClip LevelClearAudioClip { get; private set; }

    private AudioSource backgroundMusic;
    private AudioSource soundPlayer;

    protected override void Init() {
        backgroundMusic = ComponentUtils.Get<AudioSource>(GameObject.Find("BackgroundMusic"));
        GameOverAudioClip = Resources.Load<AudioClip>("Sounds/gameOver");
        JumpAudioClip = Resources.Load<AudioClip>("Sounds/jump");
        LevelClearAudioClip = Resources.Load<AudioClip>("Sounds/victory");
        soundPlayer = ComponentUtils.Get<AudioSource>(GameObject.Find("SoundPlayer"));
    }

    public void PlaySound(AudioClip clip) {
        soundPlayer.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic() {
        backgroundMusic.Play();
    }

    public void StopBackgroundMusic() {
        backgroundMusic.Stop();
    }

}

