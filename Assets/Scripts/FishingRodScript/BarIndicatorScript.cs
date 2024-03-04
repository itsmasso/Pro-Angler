using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarIndicatorScript : MonoBehaviour
{
    [SerializeField] private LayerMask hitIndicatorLayer;
    public static event Action onSuccessfulHit;
    public static event Action onMissedHit;
    private bool hitAlready = false;
    public bool playerHit;
    private bool wasDetected;
    void Start()
    {
        FishingMechanicScript.onPlayerHit += PlayerHit;
        hitAlready = false;
        wasDetected = false;
    }

    private void OnEnable()
    {
        hitAlready = false;
        wasDetected = false;
    }

    private void PlayerHit(bool _playerHit)
    {
        playerHit = _playerHit;
    }
    void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale,
    transform.rotation.eulerAngles.z, transform.position.normalized, 0, hitIndicatorLayer);

        if (hit.collider != null)
        {
            wasDetected = true;
        }

        if(hit.collider != null && !hitAlready && playerHit)
        {
            Debug.Log("hit");
            
            onSuccessfulHit?.Invoke();           
            gameObject.SetActive(false);
            hitAlready = true;
        }
        else if (hit.collider == null)
        {
            if (wasDetected && !hitAlready)
            {
                Debug.Log("missed");
                onMissedHit?.Invoke();
                gameObject.SetActive(false);
            }
        }
        
    }

    private void OnDestroy()
    {
        FishingMechanicScript.onPlayerHit -= PlayerHit;
    }
}
