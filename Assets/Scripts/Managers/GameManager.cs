using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    public int currentScene { get; private set; }
    [SerializeField] private GameObject loaderCanvas;
 
    private void Start()
    {
        MainMenu();
    
    }

    public void LoadScene(string sceneName)
    {
        
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loaderCanvas.SetActive(true);
        var scene = SceneManager.LoadSceneAsync(sceneName);
        AsyncOperation loadOperation = scene;
        
        while (!loadOperation.isDone)
        {
            //add progress bar here if needed
            yield return null;
        }
        loaderCanvas.SetActive(false);
    }

    public void UpdateScene(int newScene)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
      

        switch (newScene)
        {
            case 1:
                MainMenu();
                break;
            case 2:
                StartingArea();
                break;
            default:
                break;
        }



    }

    private void MainMenu()
    {
        AudioManager.Instance.PlayMusic("MenuMusic", true);
        //AudioManager.Instance.StopMusic("Level1Music");
        LoadScene("Main Menu");    

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "PersistScene" && scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync("PersistScene");
            }
        }
    }

    private void StartingArea()
    {
        AudioManager.Instance.StopAudioSource(AudioManager.Instance.musicSource);
        AudioManager.Instance.PlayAtmosphere(AudioManager.Instance.beachAtmosphereSource,"BeachAtmosphere", true);
        LoadScene("StartingArea");
        SceneManager.LoadSceneAsync("PersistScene", LoadSceneMode.Additive);


    }
}
