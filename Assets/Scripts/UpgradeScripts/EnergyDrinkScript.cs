using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkScript : MonoBehaviour
{
    public static event Action<int> onIncreaseStaminaCap;
    [SerializeField] private int increaseAmount;
    void Start()
    {
        onIncreaseStaminaCap?.Invoke(increaseAmount);
        Destroy(gameObject);
    }

}
