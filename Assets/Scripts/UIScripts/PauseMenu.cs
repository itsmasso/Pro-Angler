using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private Slider musicSlider, sfxSlider;

    public static event Action<bool> onPaused;

    private bool isPaused;

    private float musicVolume;
    private float sfxVolume;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        pauseMenu.SetActive(false);
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    public void OnPressEscape(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;
        onPaused?.Invoke(isPaused);
        if (ctx.performed && !isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }

        if(ctx.performed && isPaused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
    public void Resume()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "NextDialogueSFX", false);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void Options()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "NextDialogueSFX", false);
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);

    }

    public void ChangeMusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void ChangeSFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    public void ConfirmVolumeChanges()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "NextDialogueSFX", false);
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }


    public void Quit()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "NextDialogueSFX", false);
        Time.timeScale = 1;
        GameManager.Instance.UpdateScene(1);
    }
}
