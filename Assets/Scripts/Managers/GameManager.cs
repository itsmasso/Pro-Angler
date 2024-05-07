using System;
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
        SceneManager.LoadScene("UI Manager", LoadSceneMode.Additive);
        SceneManager.LoadScene("PlayerScene", LoadSceneMode.Additive);
    }

    public void UpdateScene(int newScene)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        switch (newScene)
        {
            case 1:
                StartingArea();
                break;
            case 2:
                break;
            default:
                break;
        }



    }

    private void StartingArea()
    {
        SceneManager.LoadScene(1);
        
       
    }
}
