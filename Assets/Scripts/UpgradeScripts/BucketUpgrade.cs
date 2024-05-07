using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketUpgrade : MonoBehaviour
{
    [SerializeField] private int increaseBucketAmount;
    public static event Action<int> onIncreaseBucketSize;
    void Start()
    {
        onIncreaseBucketSize?.Invoke(increaseBucketAmount);
        Destroy(gameObject);
    }

}
