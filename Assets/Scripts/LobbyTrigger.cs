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
            TransitionManager.Instance.ApplyCircleTransition();
            StartCoroutine(Transition(collision));

        }
    }

    private IEnumerator Transition(Collider2D collision)
    {
        yield return new WaitForSeconds(0.75f);
        onEnterLobby?.Invoke();
        collision.transform.position = startPosition.position;
       
    }

}
