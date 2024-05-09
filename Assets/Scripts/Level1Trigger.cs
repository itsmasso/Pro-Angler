using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Trigger : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    public static event Action onEnterLevel1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TransitionManager.Instance.ApplyCircleTransition();
            StartCoroutine(Transition(collision));
            //Trigger fade effect
            //delay > circle fade > tp while black > unfade
        }
    }

    private IEnumerator Transition(Collider2D collision)
    {
        yield return new WaitForSeconds(0.75f);
        onEnterLevel1?.Invoke();
        collision.transform.position = startPosition.position;
        
    }
}
