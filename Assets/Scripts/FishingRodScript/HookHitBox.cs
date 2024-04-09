using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHitBox : MonoBehaviour
{
    [SerializeField] private FishingRodBaseScript rodScript;
    public void DamageHook(int amount)
    {
        rodScript.DamageHook(amount);
    }
}
