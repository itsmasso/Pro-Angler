using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fishing Rod", menuName = "Fishing Rod")]
public class RodScriptableObject : ScriptableObject
{
    public string rodName;
    public Sprite rodSprite;
    public float reelSpeed;
    public float moveSpeed;
    public float rodStrength;
    public float fishingLineLength;
    //public float hookHealth;

    public bool canSlow; //has slow ability
    public float percentSlowAmount;

    public bool canStun; //can stun on counter
    public float stunDuration;
}



