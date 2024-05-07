using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemScript : MonoBehaviour
{
    [SerializeField] private Image upgradeImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private GameObject unlockedOverlay;

    public void ChangeUpgradeSprite(Sprite sprite)
    {
        upgradeImage.sprite = sprite;

    }

    public void ChangeName(string _name)
    {
        nameText.text = string.Format(_name);
    }

    public void ChangeCost(float cost)
    {
        costText.text = string.Format(cost.ToString()); 
    }

    public void UnlockOverlay()
    {
        unlockedOverlay.SetActive(true);
    }
}
