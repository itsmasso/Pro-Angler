using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : PersistentSingleton<AudioManager>
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public bool musicMuted, sfxMuted;

    private void Start()
    {
        musicMuted = false;
        sfxMuted = false;
    }
    public void PlayMusic(string name, bool enableLoop)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {

            musicSource.clip = sound.clip;
            musicSource.loop = enableLoop;
            musicSource.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Stop();
        }
    }
    //TODO add function that loops through all sources and mutes all except chosen one
    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        musicMuted = !musicMuted;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        sfxMuted = !sfxMuted;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
