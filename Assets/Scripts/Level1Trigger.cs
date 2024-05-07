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
            collision.transform.position = startPosition.position;
            onEnterLevel1?.Invoke();
            //Trigger fade effect
            //delay > circle fade > tp while black > unfade
        }
    }
}
