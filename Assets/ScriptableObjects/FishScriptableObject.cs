using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fishes")]
public class FishScriptableObject : ScriptableObject
{
    public GameObject fishPrefab;
    public string fishName;
    public float speed;
    public float strength;
    public float weight;
    public BaitType baitNeeded;
}
