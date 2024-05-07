using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class BucketScript : MonoBehaviour
{
    private int currentFishWeight;
    [SerializeField] private int defaultBucketSize = 100;
    [SerializeField] private int maxBucketSize;
    [SerializeField] private int bucketTotalValue;
    [SerializeField] private List<FishScriptableObject> listOfFish = new List<FishScriptableObject>();
    public static event Action<int> onSellFish;
    public static event Action<bool> onBucketFull;

    [Header("Text Properties")]
    [SerializeField] private TMP_Text bucketText;
    void Start()
    {
        EatOrKeepDialogue.onKeepFish += AddCurrentFishToBucket;
        BucketUpgrade.onIncreaseBucketSize += IncreaseBucketSize;
        ShopDialogue.onSellFishOption += SellFish;
        maxBucketSize = defaultBucketSize;
        //add to save system
        //maxbucketsize == datapersistence
    }

    private void ChangeBucketTextColor(bool isFull)
    {
        if (isFull)
        {
            bucketText.color = Color.red;
        }
        else
        {
            bucketText.color = Color.white;
        }
    }

    private void IncreaseBucketSize(int sizeIncreaseAmount)
    {
        maxBucketSize = sizeIncreaseAmount;
       

    }

    private void AddCurrentFishToBucket(FishScriptableObject fishScriptable)
    {
        if(currentFishWeight < maxBucketSize)
        {
            listOfFish.Add(fishScriptable);
            bucketTotalValue += fishScriptable.sellValue;
            currentFishWeight += fishScriptable.weight;
            if (currentFishWeight >= maxBucketSize)
            {
                onBucketFull?.Invoke(true);
                ChangeBucketTextColor(true);
            }
            else
            {
                onBucketFull?.Invoke(false);
                ChangeBucketTextColor(false);
            }
                
        }
    }

    public void SellFish()
    {
        onSellFish?.Invoke(bucketTotalValue);
        
        bucketTotalValue = 0;
        currentFishWeight = 0;
    }
    void Update()
    {
        if (maxBucketSize >= 999999999)
        {
            bucketText.text = string.Format("{0} lb", currentFishWeight);
        }
        else
        {
            bucketText.text = string.Format("{0}/{1} lb", currentFishWeight, maxBucketSize);
        }
    }

    private void OnDestroy()
    {
        EatOrKeepDialogue.onKeepFish -= AddCurrentFishToBucket;
        BucketUpgrade.onIncreaseBucketSize -= IncreaseBucketSize;
        ShopDialogue.onSellFishOption -= SellFish;
    }
}
