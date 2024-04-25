using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShop : MonoBehaviour
{
    [SerializeField] private GameObject shopDialogue;
    [SerializeField] private UI_manager uiManager;
    public void Exit_Shop()
    {
        
        shopDialogue.SetActive(true);
        gameObject.SetActive(false);
    }
}
