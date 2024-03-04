using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingMechanicScript : MonoBehaviour
{
    public static event Action<bool> onPlayerHit;
    public static event Action<float> onFinishedMechanic;

    public int numberOfPoints;
    [SerializeField] private float radius;
    [SerializeField] private GameObject barPrefab;
    //private List<Vector2> points = new List<Vector2>();
    [SerializeField] private List<GameObject> barIndicators = new List<GameObject>();

    public float barSizeMultiplier;
    [SerializeField] private float barSizeReducer = 0.75f;

    private Coroutine hitRoutine;
    private bool playerHit;
    //[SerializeField] private float hitDuration = 0.01f;
    private bool holdBar = false;

    private int previousIndex;

    private float numberOfHits;
    [SerializeField] private float numberOfBarsToHit; //how many bars will be spawned
    private int currentBarsSpawned;
    private int missedHitsStreak;
    void Start()
    {
        BarIndicatorScript.onSuccessfulHit += OnHittingBar;
        BarIndicatorScript.onMissedHit += OnMissingBar;

    }
    private void OnEnable()
    {
        //need to come up with some calculation to handle number of hits according to fish rarity/strength and number of fish you caught
        //TODO: a way to do it is have fish getting caught call an event that adds up a number here. that number can be used for calculation. can do same with indicator speed
        numberOfHits = 0;
        currentBarsSpawned = 0;
        missedHitsStreak = 0;
        
        float spacing = 360f / numberOfPoints;

        // Spawn points around the circle
        for (int i = 0; i < numberOfPoints; i++)
        {
            // Calculate the angle for the current point
            float angle = i * spacing;
            float radians = angle * Mathf.Deg2Rad;
            // Calculate the position of the point using trigonometry
            float x = transform.position.x + radius * Mathf.Cos(radians);
            float y = transform.position.y + radius * Mathf.Sin(radians);

            Vector2 pointPosition = new Vector2(x, y);
            //points.Add(pointPosition);
            GameObject bar = Instantiate(barPrefab, pointPosition, Quaternion.identity);
            bar.transform.parent = gameObject.transform;
            bar.transform.localScale = new Vector2(bar.transform.localScale.x * barSizeMultiplier, bar.transform.localScale.y);
            bar.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            barIndicators.Add(bar);
            bar.SetActive(false);

        }

        PickRandomBar();
    }


    private IEnumerator HitRoutine()
    {
        playerHit = true;
        onPlayerHit?.Invoke(playerHit);
        yield return new WaitForEndOfFrame();
        playerHit = false;
        onPlayerHit?.Invoke(playerHit);
        hitRoutine = null;
    }

    public void OnFishingMechanicHit(InputAction.CallbackContext ctx)
    {
        if (!holdBar)
        {
            if (ctx.performed)
            {
               
                if (hitRoutine != null)
                {
                    StopCoroutine(HitRoutine());
                }

                hitRoutine = StartCoroutine(HitRoutine());
            }
        }
        else
        {
            //hold mechanic
        }
    }

    private void OnHittingBar()
    {
        //successfully hit a bar
        numberOfHits++;
        foreach(GameObject bar in barIndicators)
        {
            bar.transform.localScale = new Vector2(bar.transform.localScale.x * barSizeReducer, bar.transform.localScale.y); //reducing size by half
        }
        missedHitsStreak = 0;
        PickRandomBar();
    }

    private void OnMissingBar()
    {
        //missed a hit
        missedHitsStreak++;
        if(missedHitsStreak >= 3)
        {
            //call event with score of 0
            onFinishedMechanic?.Invoke(0);
            gameObject.SetActive(false);
        }
        PickRandomBar();
    }

    private void PickRandomBar()
    {
        //Debug.Log("picking bar");
        int randIndex = UnityEngine.Random.Range(0, barIndicators.Count);
        if(previousIndex == randIndex)
        {
            randIndex++;
            if (randIndex >= barIndicators.Count)
                randIndex = 0;
        }
        previousIndex = randIndex;
        if (randIndex >= 0 && randIndex < barIndicators.Count)
        {
            GameObject randomBar = barIndicators[randIndex];
            randomBar.SetActive(true);
            currentBarsSpawned++;
        }
        


    }

   
    void Update()
    {
        //number of bars to hit needs to be determined by fish quantity and rarity. each fish will have a rarity score and it will add up 
        if(currentBarsSpawned > numberOfBarsToHit)
        {
            onFinishedMechanic?.Invoke(numberOfHits /numberOfBarsToHit);
            //Debug.Log("totalBars: " + numberOfBarsToHit + " numberOfHits: " + numberOfHits + " numHits/total = " + numberOfHits/numberOfBarsToHit);
        }
    }

    private void OnDestroy()
    {
        BarIndicatorScript.onSuccessfulHit -= OnHittingBar;
        BarIndicatorScript.onMissedHit -= OnMissingBar;
    }

    private void OnDisable()
    {
        foreach(GameObject bar in barIndicators)
        {
            Destroy(bar);
        }
        barIndicators.Clear();
    }
}
