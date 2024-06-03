using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockFlashLightUpgrade : MonoBehaviour
{
    public static event Action onUnlockFlashLight;
    void Start()
    {
        onUnlockFlashLight?.Invoke();
        Destroy(gameObject);
    }
}
