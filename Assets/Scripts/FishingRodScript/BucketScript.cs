using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketScript : MonoBehaviour
{
    public int currentFishAmount;
    public int maxBucketSize;
    public int bucketTotalValue;
    public List<FishScriptableObject> listOfFish = new List<FishScriptableObject>();
    [SerializeField] private UI_manager uiManager; //maybe make into event if needed
    void Start()
    {
        EatOrKeepDialogue.onKeepFish += AddCurrentFishToBucket;
    }

    private void AddCurrentFishToBucket(FishScriptableObject fishScriptable)
    {
        listOfFish.Add(fishScriptable);
        bucketTotalValue += fishScriptable.sellValue;
        currentFishAmount++;
    }

    public void SellFish()
    {
        uiManager.totalMoney += bucketTotalValue;
        bucketTotalValue = 0;
        currentFishAmount = 0;
    }
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EatOrKeepDialogue.onKeepFish -= AddCurrentFishToBucket;
    }
}
