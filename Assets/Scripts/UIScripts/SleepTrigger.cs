using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SleepTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactButton;
    private bool isHoveringTrigger;
    private bool pressedButton;
    public static event Action onSleep;

    void Start()
    {
        interactButton.SetActive(false);
        isHoveringTrigger = false;
        pressedButton = false;
    }



    public void OnPressedInteractButton(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isHoveringTrigger)
        {
            interactButton.SetActive(false);
            pressedButton = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactButton.SetActive(true);
            isHoveringTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactButton.SetActive(false);
            isHoveringTrigger = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isHoveringTrigger && pressedButton)
        {

            onSleep?.Invoke();

            pressedButton = false;


        }
    }
}
