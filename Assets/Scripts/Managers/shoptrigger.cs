
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class shoptrigger : MonoBehaviour
{   [SerializeField] private UI_manager uiManager;
    [SerializeField] private GameObject interactButton;
    private bool isHoveringDoor;
    private bool pressedButton;
    [SerializeField] private GameObject shopUI;
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
            shopUI.SetActive(true);
            pressedButton = false;
            uiManager.isTimerRunning = false;
        }
    }
}