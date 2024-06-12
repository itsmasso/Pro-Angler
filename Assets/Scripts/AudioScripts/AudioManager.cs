using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Unity.VisualScripting.Member;

public class AudioManager : PersistentSingleton<AudioManager>
{
    public Sound[] musicSounds, sfxSounds, atmosphereSounds;
    public AudioSource musicSource, sfxSource, beachAtmosphereSource, nightTimeAtmosphereSource, walkingSFXSource, reelingSFXSource;
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

    //TODO add function that loops through all sources and mutes all except chosen one
    public void PlaySFX(AudioSource source, string name, bool enableLoop)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else if(!enableLoop)
        {
            source.PlayOneShot(sound.clip);

        }
        else
        {
            source.clip = sound.clip;
            source.loop = enableLoop;
            source.Play();
        }
    }

    public void PlayAtmosphere(AudioSource source, string name, bool enableLoop)
    {
        Sound sound = Array.Find(atmosphereSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {
            source.clip = sound.clip;
            source.loop = enableLoop;
            source.Play();
        }
    }


    public void StopAudioSource(AudioSource source)
    {
        source.Stop();
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

        beachAtmosphereSource.mute = !sfxSource.mute;
        walkingSFXSource.mute = !sfxSource.mute;

    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        beachAtmosphereSource.volume = volume;
        walkingSFXSource.volume = volume;
    }
}
