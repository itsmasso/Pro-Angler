using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTrigger : MonoBehaviour
{

    [SerializeField] private Transform startPosition;
    public static event Action onEnterLobby;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = startPosition.position;
            onEnterLobby?.Invoke();
            //Trigger fade effect
            // Trigger fade effect
            //delay > circle fade > tp while black > unfade
        }
    }

}
