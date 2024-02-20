using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaitType
{
    Bait1
}

[CreateAssetMenu(fileName = "New Bait", menuName = "Bait")]
public class BaitScriptableObject : ScriptableObject
{
    public BaitType baitType;
    public float cost;

}
