using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public int maxStamina;
    public int money;
}