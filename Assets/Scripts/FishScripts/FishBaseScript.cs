using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public enum FishState
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
    public static event Action<float, float> onCaught;

    [SerializeField] protected float fishRadius = 0.2f;
    [SerializeField] protected float hookDetectionRadius = 3f;
    [SerializeField] protected LayerMask hookLayer;
    protected FishState currentState;
    public List<Vector2> wayPoints = new List<Vector2>();
    protected Vector2 currentWayPoint;
    [SerializeField] protected FishScriptableObject fishScriptableObj;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Sprite fishIcon;
    [SerializeField] protected Animator anim;
    private float speed;

    //flags for calling things once
    protected bool calledEnterEvent = false; //bool to ensure we call event once
    protected bool calledExitEvent = false; //bool to ensure we call event once
    protected bool callCaughtEvent = false; //call once
    protected bool onReelState = false; //checking to see if fishing rod is in its reeling state
    protected bool isCaught = false;
    
    protected virtual void Start()
    {
        if(fishScriptableObj != null)
        {
            speed = UnityEngine.Random.Range(fishScriptableObj.speed - fishScriptableObj.speed/2, fishScriptableObj.speed + fishScriptableObj.speed/2);
        }
        FishingRodBaseScript.onReelState += CheckReelState;
        FishingRodBaseScript.onFinishCaughtFish += DestroyFish;
        FishingRodBaseScript.onFishEscape += Escape;
        callCaughtEvent = false;
        currentState = FishState.Patrolling;
        if (wayPoints.Count > 0)
        {
            currentWayPoint = wayPoints[UnityEngine.Random.Range(0, wayPoints.Count)];
        }
    }
    /*
    protected virtual void OnEnable()
    {
        
        callCaughtEvent = false;
        currentState = FishState.Patrolling;
        if (wayPoints.Count > 0)
        {
            currentWayPoint = wayPoints[UnityEngine.Random.Range(0, wayPoints.Count)];
        }
    }
    */
    protected void CheckReelState(bool isReelState)
    {
        onReelState = isReelState;
    }

    protected void DestroyFish()
    {
        if (isCaught)
        {
            GameObject iconPopup = Instantiate(fishScriptableObj.fishIconPopup, transform.position, Quaternion.identity);
            iconPopup.GetComponent<FishIconPopup>().spriteRenderer.sprite = fishIcon;
            Destroy(gameObject);
        }
    }

    protected void Escape()
    {
        //make fish run away
        currentState = FishState.Patrolling;
        isCaught = false;
        callCaughtEvent = false;
    }

    protected void RotateSprite(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        float zRotation = transform.rotation.eulerAngles.z;
        if (zRotation > 180f)
        {
            zRotation -= 360f;
        }

        if (zRotation <= -90 || zRotation >= 90)
        {

            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }

    }

    protected virtual void Update()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, hookDetectionRadius, transform.position.normalized, 0, hookLayer);     
        if(anim != null)
        {
            if (currentState == FishState.Caught)
            {
                anim.SetBool("IsCaught", true);
            }
            else
            {
                anim.SetBool("IsCaught", false);
            }
        }
            

        switch (currentState)
        {
            case FishState.Patrolling:
                if (hit.collider != null && hit.collider.gameObject.GetComponent<BaitHolder>().currentBait == fishScriptableObj.baitNeeded && !onReelState)
                {                
                    if (!calledEnterEvent)
                    {
                        onFollowingHook?.Invoke();
                        currentState = FishState.Chasing;
                        calledExitEvent = false;
                        calledEnterEvent = true;
                    }
                    
                }
                
                RotateSprite(currentWayPoint);
                FishMovement();
                break;
            case FishState.Chasing:
                
                if (hit.collider == null || onReelState)
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
                    RotateSprite(hit.collider.gameObject.transform.position);
                    transform.position = Vector2.MoveTowards(transform.position, hit.collider.gameObject.transform.position, speed * Time.deltaTime);
                }
                
                if(hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= fishRadius /*&& onReelState*/)
                {
                    currentState = FishState.Caught;
                }
                break;
            case FishState.Caught:
                if (!callCaughtEvent)
                {
                    onCaught?.Invoke(fishScriptableObj.strength, fishScriptableObj.weight);
                    callCaughtEvent = true;
                }
                isCaught = true;
                
                transform.position = hit.collider.gameObject.transform.position;
                break;
            default:
                break;

        }

    }

    //random waypoint fish movement
    protected virtual void FishMovement()
    {
        
        if (wayPoints.Count > 0)
        {

            transform.position = Vector2.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentWayPoint) < 0.1f)
            {
                currentWayPoint = wayPoints[UnityEngine.Random.Range(0, wayPoints.Count)];
            }

        }

    }



    protected virtual void OnDestroy()
    {
        FishingRodBaseScript.onReelState -= CheckReelState;
        FishingRodBaseScript.onFinishCaughtFish -= DestroyFish;
        FishingRodBaseScript.onFishEscape -= Escape;
    }


}
