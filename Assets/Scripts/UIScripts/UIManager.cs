using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] uiPanels;

    void Start()
    {
        shoptrigger.onShopEnter += HideUI;
        ShopDialogue.onExitShop += ShowUI;
    }

    private void HideUI()
    {
        foreach(GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }
    }

    private void ShowUI()
    {
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(true);
        }
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        shoptrigger.onShopEnter -= HideUI;
        ShopDialogue.onExitShop -= ShowUI;
    }
}
