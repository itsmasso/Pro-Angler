
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public static event Action<UpgradeScriptableObj> onBuyUpgrade;

    [Header("Bait Upgrades")]
    [SerializeField] private UpgradeScriptableObj[] baitUpgrades;
    [SerializeField] private Transform shopContentBait;
    [SerializeField] private GameObject baitTab;

    [Header("Bucket Upgrades")]
    [SerializeField] private UpgradeScriptableObj[] bucketUpgrades;
    [SerializeField] private Transform shopContentBucket;
    [SerializeField] private GameObject bucketTab;

    [Header("Stamina Upgrades")]
    [SerializeField] private UpgradeScriptableObj[] staminaUpgrades;
    [SerializeField] private Transform shopContentStamina;
    [SerializeField] private GameObject staminaTab;

    [Header("Rod Upgrades")]
    [SerializeField] private UpgradeScriptableObj[] rodUpgrades;
    [SerializeField] private Transform shopContentRod;
    [SerializeField] private GameObject rodTab;

    [Header("Mod Upgrades")]
    [SerializeField] private UpgradeScriptableObj[] modUpgrades;
    [SerializeField] private Transform shopContentMods;
    [SerializeField] private GameObject modsTab;

    [Header("References")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private GameObject shopDialogue;
    
    private void HandleShopContent(UpgradeScriptableObj[] upgrades, Transform shopContent)
    {
        foreach (UpgradeScriptableObj upgrade in upgrades)
        {
            GameObject item = Instantiate(itemPrefab, shopContent);
            upgrade.itemRef = item;
            if(upgrade.upgradeIcon != null)
                item.GetComponent<ShopItemScript>().ChangeUpgradeSprite(upgrade.upgradeIcon);
            item.GetComponent<ShopItemScript>().ChangeName(upgrade.upgradeName);
            item.GetComponent<ShopItemScript>().ChangeCost(upgrade.cost);


            item.GetComponentInChildren<Button>().onClick.AddListener(() => {               
                BuyUpgrade(upgrade);

            });

        }
    }

    private void Start()
    {
        shopDialogue.GetComponent<ShopDialogue>().onOpenShopWindow += ToggleShop;
        MoneyHandler.onUpdateMoney += CheckPrices;
        MoneyHandler.onTransact += ApplyUpgrades;
        HandleShopContent(baitUpgrades, shopContentBait);
        HandleShopContent(bucketUpgrades, shopContentBucket);
        HandleShopContent(staminaUpgrades, shopContentStamina);
        HandleShopContent(rodUpgrades, shopContentRod);

    }
    public void BuyUpgrade(UpgradeScriptableObj upgrade)
    {
        if (!upgrade.isUnlocked)
        {
            onBuyUpgrade?.Invoke(upgrade);
        }


    }

    private void ApplyUpgrades(UpgradeScriptableObj upgrade)
    {
        Instantiate(upgrade.applyUpgrade);
        upgrade.itemRef.GetComponent<ShopItemScript>().UnlockOverlay();
        upgrade.itemRef.GetComponentInChildren<Button>().interactable = false;
        //add to save system
        upgrade.isUnlocked = true;
    }



    public void OpenRodTab()
    {
        baitTab.SetActive(false);
        rodTab.SetActive(true);
        staminaTab.SetActive(false);
        bucketTab.SetActive(false);
    }
    public void OpenBaitTab()
    {
        baitTab.SetActive(true);
        rodTab.SetActive(false);
        staminaTab.SetActive(false);
        bucketTab.SetActive(false);

    }

    public void OpenBucketTab()
    {
        baitTab.SetActive(false);
        rodTab.SetActive(false);
        staminaTab.SetActive(false);
        bucketTab.SetActive(true);
    }

    public void OpenStaminaTab()
    {
        baitTab.SetActive(false);
        rodTab.SetActive(false);
        staminaTab.SetActive(true);
        bucketTab.SetActive(false);
    }
    public void ToggleShop(object sender, bool canOpen)    
    {
        OpenRodTab();
        shopDialogue.SetActive(!canOpen);
        shopWindow.SetActive(canOpen);
    }

    public void CloseShopWindow()
    {
        shopDialogue.SetActive(true);
        shopWindow.SetActive(false);
    }

    //update items to reflect if you have enough money or not
    private void CheckPrices(int totalMoney)
    {
        CheckUpgradeButtons(totalMoney, baitUpgrades);
        CheckUpgradeButtons(totalMoney, rodUpgrades);
        CheckUpgradeButtons(totalMoney, bucketUpgrades);
        CheckUpgradeButtons(totalMoney, staminaUpgrades);
    }

    private void CheckUpgradeButtons(int _totalMoney, UpgradeScriptableObj[] upgrades )
    {
        foreach (UpgradeScriptableObj upgrade in upgrades)
        {
            if (_totalMoney <= upgrade.cost)
            {
                upgrade.itemRef.GetComponentInChildren<Button>().interactable = false;
            }
            else
            {
                upgrade.itemRef.GetComponentInChildren<Button>().interactable = true;
            }

        }
    }

    private void OnDestroy()
    {
        if(shopDialogue != null)
            shopDialogue.GetComponent<ShopDialogue>().onOpenShopWindow -= ToggleShop;
        MoneyHandler.onUpdateMoney -= CheckPrices;
        MoneyHandler.onTransact -= ApplyUpgrades;
    }
}

