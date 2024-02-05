using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fishing Rod", menuName = "Fishing Rod")]
public class RodScriptableObject : ScriptableObject
{
    public float reelSpeed;
    public float rodStrength;
    public float fishingLineLength;
}



