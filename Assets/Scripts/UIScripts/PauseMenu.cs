using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image musicIcon;
    [SerializeField] private Sprite musicMutedSprite;
    [SerializeField] private Sprite musicUnmutedSprite;
    private bool musicMuted = false;
    private bool isPaused;
    public void OnPressEscape(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;

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
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void MuteMusic()
    {
        musicMuted = !musicMuted;
        AudioManager.Instance.ToggleMusic();
        if (musicMuted)
        {
            musicIcon.sprite = musicMutedSprite;
        }
        else
        {
            musicIcon.sprite = musicUnmutedSprite;
        }
    }

    public void Menu()
    {
        Time.timeScale = 1;
        GameManager.Instance.UpdateScene(1);
    }
}
