using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

//This method handles menu and methods for going to different menu screens.
//later will add a way to more efficiently and cleanly switch between menus
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu, settings, startGame;

    [SerializeField] private Slider musicSlider, sfxSlider;

    //private int sceneToStart; if adding more levels which require new scenes

    void Start()
    {

        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        menu.SetActive(true);
        settings.SetActive(false);
        startGame.SetActive(false);
 
    }

    public void OnMainMenu()
    {
        menu.SetActive(true);

    }

    public void OnSettingsMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "NextDialogueSFX", false);
        menu.SetActive(false);
        settings.SetActive(true);

    }

    public void OnStartGame()
    {
        //TEMP
        //menu.SetActive(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "NextDialogueSFX", false);
        GameManager.Instance.UpdateScene(2); //loading starting area scene

        //startGame.SetActive(true);
        //backButton.SetActive(true);
    }

    public void OnNewGame()
    {
        //reset save data
        GameManager.Instance.UpdateScene(2); //loading starting area scene
    }

    public void OnContinueGame()
    {
        //sceneToStart = ResourceSystem.Instance.saveData.playerData.currentGameLevel;
        //GameManager.Instance.UpdateScene(sceneToStart);

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
        OnMainMenu();
        settings.SetActive(false);
        startGame.SetActive(false);
    }


    public void OnQuitGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "NextDialogueSFX", false);
        Application.Quit();
    }

}