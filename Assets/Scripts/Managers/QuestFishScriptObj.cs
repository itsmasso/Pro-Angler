using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New QuestFish", menuName = "QuestFish")]
public class QFishScriptableObj : ScriptableObject
{
    
    public int bonus;
    public string FishName;
    public Sprite FishIcon;
    public GameObject itemref;
}