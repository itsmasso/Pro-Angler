using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class UpgradeScriptableObj : ScriptableObject
{
    public int quantity;
    public int cost;
    public string upgradeName;
    public Sprite upgradeIcon;
    public GameObject itemRef;
}
