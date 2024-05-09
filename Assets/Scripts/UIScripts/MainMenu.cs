using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

//This method handles menu and methods for going to different menu screens.
//later will add a way to more efficiently and cleanly switch between menus
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu, settings, startGame, backButton;


    [SerializeField]
    private Button continueGameButton;

    //private int sceneToStart; if adding more levels which require new scenes

    void Start()
    {


        menu.SetActive(true);
        settings.SetActive(false);
        startGame.SetActive(false);
        backButton.SetActive(false);
    }

    public void OnMainMenu()
    {
        menu.SetActive(true);
        backButton.SetActive(false);
    }

    public void OnSettingsMenu()
    {
        menu.SetActive(false);
        settings.SetActive(true);
        backButton.SetActive(true);
    }

    public void OnStartGame()
    {
        //TEMP
        //menu.SetActive(false);
    
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

    public void OnBack()
    {
        OnMainMenu();
        settings.SetActive(false);
        startGame.SetActive(false);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

}