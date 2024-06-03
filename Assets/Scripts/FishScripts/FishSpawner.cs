using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private List<FishScriptableObject> commonFishList = new List<FishScriptableObject>();
    [SerializeField] private List<FishScriptableObject> uncommonFishList = new List<FishScriptableObject>();
    [SerializeField] private List<FishScriptableObject> rareFishList = new List<FishScriptableObject>();

    [SerializeField] protected int maxFishCount;

    //use event to subtract fish count when fish caught

    //[SerializeField] private float spacing = 25f; //spacing is the radius around a point
    //[SerializeField] private int rejectionSamples = 30; //numSamplesBeforeRejection indicates num of times before we exit a loop
    public List<GameObject> currentSpawnedFishes;
    //[SerializeField] private float wayPointSpacing = 15f;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private bool enableFishInvis;
    void Start()
    {
        //QoutaSystem.onContinueToNextDay += ResetSpawns;
    }

    private void OnEnable()
    {
        InitialSpawn();
    }

    private void InitialSpawn()
    {
        /*
        List<Vector2> fishSpawnPoints = PoissonDiscSampling.GeneratePoints(spacing, regionSize, rejectionSamples);
        foreach (Vector2 point in fishSpawnPoints)
        {
            Vector2 spawnPoint = new Vector2(point.x - boxCollider.size.x/2, point.y - boxCollider.size.y);
            SpawnFish(spawnPoint);
        }
        */

        //maxFishCount = currentSpawnedFishes.Count;

        for(int i = 0; i < maxFishCount; i++)
        {
            AddFish();
        }
    }

    private void ChangeFishVisibility(GameObject newFish)
    {
        if (enableFishInvis)
        {
            newFish.GetComponent<FishBaseScript>().isVisible = false;
        }
        else
        {
            newFish.GetComponent<FishBaseScript>().isVisible = true;
        }
    }

    private void SpawnFish(Vector2 spawnPosition)
    {
        float randNum = Random.value;
        if(randNum <= 0.75)
        {
            //common fish
            GameObject newFish = Instantiate(commonFishList[Random.Range(0, commonFishList.Count)].fishPrefab, spawnPosition, Quaternion.identity);
            newFish.GetComponent<FishBaseScript>().minMapBounds = new Vector2(transform.position.x - (boxCollider.size.x / 2) + boxCollider.offset.x, transform.position.y - (boxCollider.size.y / 2) + boxCollider.offset.y);
            newFish.GetComponent<FishBaseScript>().maxMapBounds = new Vector2(transform.position.x + (boxCollider.size.x / 2) + boxCollider.offset.x, transform.position.y + (boxCollider.size.y / 2) + boxCollider.offset.y);
            ChangeFishVisibility(newFish);
            currentSpawnedFishes.Add(newFish);

        }
        else if(randNum > 0.75 && randNum <= 0.95)
        {
            //uncommon fish
            GameObject newFish = Instantiate(uncommonFishList[Random.Range(0, uncommonFishList.Count)].fishPrefab, spawnPosition, Quaternion.identity);
            newFish.GetComponent<FishBaseScript>().minMapBounds = new Vector2(transform.position.x - (boxCollider.size.x / 2) + boxCollider.offset.x, transform.position.y - (boxCollider.size.y / 2) + boxCollider.offset.y);
            newFish.GetComponent<FishBaseScript>().maxMapBounds = new Vector2(transform.position.x + (boxCollider.size.x / 2) + boxCollider.offset.x, transform.position.y + (boxCollider.size.y / 2) + boxCollider.offset.y);
            ChangeFishVisibility(newFish);
            currentSpawnedFishes.Add(newFish);

        }
        else if(randNum > 0.95)
        {
            //rare fish
            GameObject newFish = Instantiate(rareFishList[Random.Range(0, rareFishList.Count)].fishPrefab, spawnPosition, Quaternion.identity);
            newFish.GetComponent<FishBaseScript>().minMapBounds = new Vector2(transform.position.x - (boxCollider.size.x / 2) + boxCollider.offset.x, transform.position.y - (boxCollider.size.y / 2) + boxCollider.offset.y);
            newFish.GetComponent<FishBaseScript>().maxMapBounds = new Vector2(transform.position.x + (boxCollider.size.x / 2) + boxCollider.offset.x, transform.position.y + (boxCollider.size.y / 2) + boxCollider.offset.y);
            ChangeFishVisibility(newFish);
            currentSpawnedFishes.Add(newFish);
        }

        
        

    }


    private void AddFish()
    {
        
        // Generate random x and y coordinates within the specified range
        float randomX = Random.Range(-(boxCollider.size.x / 2) + boxCollider.offset.x, (boxCollider.size.x / 2) + boxCollider.offset.x);
        float randomY = Random.Range(-(boxCollider.size.y / 2) + boxCollider.offset.y, (boxCollider.size.y / 2) + boxCollider.offset.y);

        // Create a Vector2 representing the random position
        Vector2 spawnPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);

        // Spawn the object at the random position
        SpawnFish(spawnPosition);
    }

    private void Update()
    {

        
    }

    private void OnDestroy()
    {
        //QoutaSystem.onContinueToNextDay -= ResetSpawns;
    }

}
