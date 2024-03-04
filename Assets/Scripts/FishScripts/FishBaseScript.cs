using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

enum FishState
{
    Patrolling,
    Chasing,
    Caught
}
//Note: in the future possibly change state machine from switch statements back to objects like fishing rod state machine
public abstract class FishBaseScript : MonoBehaviour
{
    public static event Action onFollowingHook;
    public static event Action onExitHook;
    public static event Action onCaught;

    [SerializeField] protected float fishRadius = 0.5f;
    [SerializeField] protected float hookDetectionRadius = 3f;
    [SerializeField] protected LayerMask hookLayer;
    private FishState currentState;
    public List<Vector2> wayPoints = new List<Vector2>();
    private int currentIndex;
    [SerializeField] protected FishScriptableObject fishScriptableObj;

    //flags for calling things once
    private bool calledEnterEvent = false; //bool to ensure we call event once
    private bool calledExitEvent = false; //bool to ensure we call event once
    private bool callCaughtEvent = false; //call once
    private bool onReelState = false; //checking to see if fishing rod is in its reeling state
    private bool isCaught = false;

    void Start()
    {
        FishingRodBaseScript.onReelState += CheckReelState;
        FishingRodBaseScript.onDestroyCaughtFish += DestroyFish;
        FishingRodBaseScript.onFishEscape += Escape;
        currentState = FishState.Patrolling;
        if (wayPoints.Count > 0)
        {
            currentIndex = UnityEngine.Random.Range(0, wayPoints.Count);
        }
    }
    private void OnEnable()
    {
        currentState = FishState.Patrolling;
        if (wayPoints.Count > 0)
        {
            currentIndex = UnityEngine.Random.Range(0, wayPoints.Count);
        }
    }

    private void CheckReelState(bool isReelState)
    {
        onReelState = isReelState;
    }

    private void DestroyFish()
    {
        if (isCaught)
        {
            Destroy(gameObject);
        }
    }

    private void Escape()
    {
        //make fish run away
        currentState = FishState.Patrolling;
        isCaught = false;
        callCaughtEvent = false;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, hookDetectionRadius, transform.position.normalized, 0, hookLayer);     
       
        switch (currentState)
        {
            case FishState.Patrolling:
                if (hit.collider != null && hit.collider.gameObject.GetComponent<BaitHolder>().currentBait == fishScriptableObj.baitNeeded && !onReelState)
                {                
                    if (!calledEnterEvent)
                    {
                        onFollowingHook?.Invoke();
                        calledExitEvent = false;
                        calledEnterEvent = true;
                    }
                    currentState = FishState.Chasing;
                }
                FishMovement();
                break;
            case FishState.Chasing:
                if (hit.collider == null)
                {                  
                    if (!calledExitEvent)
                    {
                        onExitHook?.Invoke();
                        calledEnterEvent = false;
                        calledExitEvent = true;
                    }
                    currentState = FishState.Patrolling;
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, hit.collider.gameObject.transform.position, fishScriptableObj.speed * Time.deltaTime);
                }
                
                if(hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= fishRadius && onReelState)
                {
                    currentState = FishState.Caught;
                }
                break;
            case FishState.Caught:
                if (!callCaughtEvent)
                {
                    onCaught?.Invoke();
                    callCaughtEvent = true;
                }
                isCaught = true;
                transform.position = hit.collider.gameObject.transform.position;
                break;
            default:
                break;

        }

    }

    protected virtual void FishMovement()
    {
        if(wayPoints.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentIndex], fishScriptableObj.speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, wayPoints[currentIndex]) < 0.1f)
            {
                currentIndex = UnityEngine.Random.Range(0, wayPoints.Count);
            }
        }

    }

    private void OnDestroy()
    {
        FishingRodBaseScript.onReelState -= CheckReelState;
        FishingRodBaseScript.onDestroyCaughtFish -= DestroyFish;
        FishingRodBaseScript.onFishEscape -= Escape;
    }


}
