using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBait : MonoBehaviour
{
    [SerializeField] private int baitID;
    public static event Action<int> onUnlockBait;
    void Start()
    {
        onUnlockBait?.Invoke(baitID);
        Destroy(gameObject);
    }
}
