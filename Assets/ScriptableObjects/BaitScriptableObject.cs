using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaitType
{
    Tier1Bait,
    Tier2Bait,
    Tier3Bait,
    Tier4Bait
}

[CreateAssetMenu(fileName = "New Bait", menuName = "Bait")]
public class BaitScriptableObject : ScriptableObject
{
    public BaitType baitType;
    public float cost;

}
