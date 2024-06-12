
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;


public class shoptrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactButton;
    private bool isHoveringDoor;
    private bool pressedButton;
    [SerializeField] private GameObject shopUI;
    public static event Action onShopEnter;
  
    void Start()
    {
        interactButton.SetActive(false);
        isHoveringDoor = false;
        pressedButton = false;
        shopUI.SetActive(false);
    }



    public void OnPressedEnterShopButton(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isHoveringDoor)
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
                isHoveringDoor = true;
            }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
            if (other.CompareTag("Player"))    
            {
            interactButton.SetActive(false);
            isHoveringDoor = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isHoveringDoor && pressedButton)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSource, "OpeningShopDoorSFX", false);
            onShopEnter?.Invoke();
            shopUI.SetActive(true);          
            pressedButton = false;
    

        }
    }
}
