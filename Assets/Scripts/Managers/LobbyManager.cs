using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : PersistentSingleton<LobbyManager>
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform toLobbyPosition;
    [SerializeField] private Transform lobbyPlayer;
    [SerializeField] private bool firstSpawn;
  

    private void Start()
    {
        firstSpawn = false;
        if (!firstSpawn)
        {
            lobbyPlayer.position = startPosition.position;
            firstSpawn = true;
        }
        else
        {
            lobbyPlayer.position = toLobbyPosition.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
