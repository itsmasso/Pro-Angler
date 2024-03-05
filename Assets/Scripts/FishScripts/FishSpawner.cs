using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishSpawner : MonoBehaviour
{
    [SerializeField] private List<FishScriptableObject> fishList = new List<FishScriptableObject>();
    [SerializeField] protected int maxFishCount;

    //use event to subtract fish count when fish caught

    [SerializeField] private float spacing = 25f; //spacing is the radius around a point
    [SerializeField] private int rejectionSamples = 30; //numSamplesBeforeRejection indicates num of times before we exit a loop
    [SerializeField] private Vector2 regionSize = new Vector2(50, 12); //sampleRegionSize indicates size of grid in units
    [SerializeField] private Vector2 positionOffset = new Vector2(-25, -6); //spawn point
    public List<GameObject> currentSpawnedFishes;
    [SerializeField] private float minAngle, maxAngle;

    [SerializeField] private float wayPointSpacing = 15f;

    private float timer;
    private float checkInterval = 0.1f;
    void Start()
    {
        InitialSpawn();
    }


    private void InitialSpawn()
    {
        List<Vector2> fishSpawnPoints = PoissonDiscSampling.GeneratePoints(spacing, regionSize, rejectionSamples);
        foreach (Vector2 point in fishSpawnPoints)
        {
            Vector2 spawnPoint = new Vector2(point.x + positionOffset.x, point.y + positionOffset.y);
            SpawnFish(spawnPoint);


        }
        maxFishCount = currentSpawnedFishes.Count;
    }

    private void SpawnFish(Vector2 spawnPosition)
    {
        GameObject newFish = Instantiate(fishList[Random.Range(0, fishList.Count)].fishPrefab, spawnPosition, Quaternion.identity);
 
        newFish.SetActive(false);
        List<Vector2> fishWayPoints = PoissonDiscSampling.GeneratePoints(wayPointSpacing, regionSize, rejectionSamples);
        
        foreach (Vector2 point in fishWayPoints)
        {
            Vector2 wayPoint = new Vector2(point.x + positionOffset.x, point.y + positionOffset.y);
            newFish.GetComponent<FishBaseScript>().wayPoints.Add(wayPoint);
        }
        newFish.SetActive(true);
        currentSpawnedFishes.Add(newFish);
        

    }

    private void AddFish()
    {
        
        // Generate random x and y coordinates within the specified range
        float randomX = Random.Range(-regionSize.x / 2, regionSize.x / 2);
        float randomY = Random.Range(-regionSize.y / 2, regionSize.y / 2);

        // Create a Vector2 representing the random position
        Vector2 spawnPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);

        // Spawn the object at the random position
        SpawnFish(spawnPosition);
    }

    private void Update()
    {
        //disclaimer: if this makes things slow, change it up
        timer += Time.deltaTime;
        if(timer >= checkInterval)
        {
            //deleting null objects in list after destroying them          
            if(currentSpawnedFishes.Exists(item => item == null))
            {
                currentSpawnedFishes.RemoveAll(item => item == null);
            }
            if (currentSpawnedFishes.Count < maxFishCount)
            {
                AddFish();
            }
            timer = 0;
        }
        
    }

    private void OnDestroy()
    {

    }

}
