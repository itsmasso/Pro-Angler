using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockFishingRodScript : MonoBehaviour
{
    [SerializeField] private int rodID;
    public static event Action<int> onUnlockFishingRod;
    void Start()
    {
        onUnlockFishingRod?.Invoke(rodID);
        Destroy(gameObject);
    }


}
