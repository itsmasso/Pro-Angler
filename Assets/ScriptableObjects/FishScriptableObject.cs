using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fishes")]
public class FishScriptableObject : ScriptableObject
{
    public GameObject fishPrefab;
    public float speed;
    public float strength;
    public BaitType baitNeeded;
}
