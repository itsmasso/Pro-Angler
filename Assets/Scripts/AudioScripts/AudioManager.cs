using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Unity.VisualScripting.Member;

public class AudioManager : PersistentSingleton<AudioManager>
{
    public Sound[] musicSounds, sfxSounds, atmosphereSounds;
    public AudioSource musicSource, sfxSource, atmosphereSource, walkingSFXSource, reelingSFXSource;
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
    public void PlaySFX(string name, bool enableLoop)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else if(!enableLoop)
        {
            sfxSource.PlayOneShot(sound.clip);

        }
        else
        {
            sfxSource.clip = sound.clip;
            sfxSource.loop = enableLoop;
            sfxSource.Play();
        }
    }

    public void PlayAtmosphere(string name, bool enableLoop)
    {
        Sound sound = Array.Find(atmosphereSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {
            atmosphereSource.clip = sound.clip;
            atmosphereSource.loop = enableLoop;
            atmosphereSource.Play();
        }
    }

    public void StopAtmosphere(string name)
    {
        Sound sound = Array.Find(atmosphereSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {
            atmosphereSource.clip = sound.clip;
            atmosphereSource.Stop();
        }
    }

    public void PlayLoopedSFX(AudioSource source, string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {
            source.clip = sound.clip;
            source.loop = true;
            source.Play();
        }
    }


    public void StopLoopedSFX(AudioSource source, string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound Not Found!");
        }
        else
        {
            source.clip = sound.clip;
            source.Stop();
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

        atmosphereSource.mute = !sfxSource.mute;
        walkingSFXSource.mute = !sfxSource.mute;

    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        atmosphereSource.volume = volume;
        walkingSFXSource.volume = volume;
    }
}
