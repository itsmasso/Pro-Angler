using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fishes")]
public class FishScriptableObject : ScriptableObject
{
    public GameObject fishPrefab;
    public GameObject fishIconPopup;
    public Sprite icon;
    public string fishName;
    public float speed;
    public float strength;
    public int weight;
    public int sellValue;
    public int staminaRestoreAmount;
    //public int tier;
    public BaitType baitsUsed;

}
