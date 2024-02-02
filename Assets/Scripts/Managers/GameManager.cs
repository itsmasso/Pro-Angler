using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    public int currentScene { get; private set; }


    private void Start()
    {
        UpdateScene(1);
    }

    public void UpdateScene(int newScene)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        switch (newScene)
        {
            case 1:
                MenuLobby();
                break;
            case 2:
                Level_1();
                break;
            default:
                break;
        }

    }

    public void MenuLobby()
    {
        SceneManager.LoadScene(1);
    }

    private void Level_1()
    {
        //TODO
    }
}
