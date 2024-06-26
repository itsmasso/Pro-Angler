using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishInfo
{
    public int weight;
    public int worth;
    public string fishName;
    public int stamRestored;
    public Sprite icon;
}
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
    public static event Action<FishScriptableObject> onCaught;
    public static event Action<FishInfo> onDestroyFish;

    [SerializeField] protected GameObject alertPrefab;
    private GameObject alert;

    [SerializeField] protected float fishRadius = 0.2f;
    [SerializeField] protected float hookDetectionRadius = 3f;
    [SerializeField] protected LayerMask hookLayer;
    protected FishState currentState;
    public List<Vector2> wayPoints = new List<Vector2>();
    protected Vector2 currentWayPoint;
    public FishScriptableObject fishScriptableObj;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Sprite fishIcon;
    [SerializeField] protected Animator anim;
    [SerializeField] private float speed;

    [Header("Movement")]
    private Vector2 direction;
    [SerializeField] private float minCD = 3f, maxCD = 5f;
    [SerializeField] private float changeDirectionCooldown;
    [SerializeField] private float rotateSpeed = 3f;
    public Vector2 minMapBounds, maxMapBounds;
    private Vector2 targetDirection;
    private float timer;
    private bool hasChangedDirection;

    //flags for calling things once
    protected bool calledEnterEvent = false; //bool to ensure we call event once
    protected bool calledExitEvent = false; //bool to ensure we call event once
    protected bool callCaughtEvent = false; //call once
    protected bool onReelState = false; //checking to see if fishing rod is in its reeling state
    protected bool isCaught = false;


    [Header("Fish Info")]
    private int weight;
    private int sellValue;
    private int stamRestored;
    private string fullName;
    public bool isVisible = true;
    [SerializeField] private float fadeDuration = 2.5f;

    private float EvaluateRandomValue(float randomValue, float minValue, float maxValue)
    {
        float score = 1 - ((maxValue - randomValue) / (maxValue - minValue));
        return Mathf.Clamp(score, 0.1f, 1f);
    }

    protected virtual void Start()
    {

        alert = Instantiate(alertPrefab, transform.position, Quaternion.identity);
        alert.transform.SetParent(gameObject.transform);
        float minValue = fishScriptableObj.weight - fishScriptableObj.weight * 0.4f;
        float maxValue = fishScriptableObj.weight + fishScriptableObj.weight * 0.4f;
        float randomWeight = UnityEngine.Random.Range(minValue, maxValue);
        float evaluatedScore = EvaluateRandomValue(randomWeight, minValue, maxValue);

        //can be small, medium, large, huge depending on weight. divided into four sections

        if (evaluatedScore <= 0.25f)
        {
            fullName = string.Format("Small {0}", fishScriptableObj.fishName);
        }
        else if (evaluatedScore > 0.25f && evaluatedScore <= 0.5f)
        {
            fullName = string.Format("Normal {0}", fishScriptableObj.fishName);
        }
        else if (evaluatedScore > 0.5f && evaluatedScore <= 0.75f)
        {
            fullName = string.Format("Large {0}", fishScriptableObj.fishName);
        }
        else if (evaluatedScore > 0.75f && evaluatedScore <= 1f)
        {
            fullName = string.Format("Huge {0}", fishScriptableObj.fishName);
        }
        else
        {
            //bugged fish
            fullName = "??? " + fishScriptableObj.fishName;
        }

        weight = Mathf.FloorToInt(randomWeight);
        sellValue = Mathf.FloorToInt(fishScriptableObj.sellValue * evaluatedScore * 2f);
        stamRestored = Mathf.FloorToInt(fishScriptableObj.staminaRestoreAmount * evaluatedScore * 2f);
        //add icon change

        changeDirectionCooldown = UnityEngine.Random.Range(minCD, maxCD);
        targetDirection = UnityEngine.Random.insideUnitCircle.normalized;
        if (fishScriptableObj != null)
        {
            speed = UnityEngine.Random.Range(fishScriptableObj.speed - fishScriptableObj.speed/2, fishScriptableObj.speed + fishScriptableObj.speed/2);
        }
        FishingRodBaseScript.onReelState += CheckReelState;
        FishingRodBaseScript.onFinishCaughtFish += SaveFish;
        FishingRodBaseScript.onFishEscape += Escape;

        WorldTime.onTimeChange += OnTimeChange;
        callCaughtEvent = false;
        currentState = FishState.Patrolling;

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

    private void OnTimeChange(DayPeriod dayPeriod)
    {
        if (!isCaught)
        {
            DeleteFish();
        }
    }

    protected void CheckReelState(bool isReelState)
    {
        onReelState = isReelState;
    }

    protected void DeleteFish()
    {
        StartCoroutine(FadeFishAndDestroy());
    }

    private IEnumerator FadeFishAndDestroy()
    {
        Color startColor = sprite.color;
        Color targetColor = startColor;
        targetColor.a = 0f;
        float timer = 0f;
        while(timer < fadeDuration)
        {
            timer += Time.deltaTime;
            Color currentColor = Color.Lerp(startColor, targetColor, timer / fadeDuration);
            sprite.color = currentColor;
            yield return null;
        }

        sprite.color = targetColor;
        Destroy(gameObject);
    }

    protected void SaveFish()
    {
        if (isCaught)
        {
            //TEMPORARILY COMMENT OUT
            //fish popup animation
            //GameObject iconPopup = Instantiate(fishScriptableObj.fishIconPopup, transform.position, Quaternion.identity);
            //iconPopup.GetComponent<FishIconPopup>().spriteRenderer.sprite = fishIcon;
            FishInfo fishInfo = new FishInfo();
            fishInfo.weight = weight;
            fishInfo.worth = sellValue;
            fishInfo.fishName = fullName;
            fishInfo.stamRestored = stamRestored;
            fishInfo.icon = fishScriptableObj.icon;
            onDestroyFish?.Invoke(fishInfo);

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

    //not using this method right now
    protected void RotateSprite()
    {

        
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


    private void HandleEnemyOutOfBounds()
    {
        if (transform.position.x <= minMapBounds.x || transform.position.x >= maxMapBounds.x ||
            transform.position.y <= minMapBounds.y || transform.position.y >= maxMapBounds.y)
        {
            if (!hasChangedDirection)
            {
                // Change direction

                direction = -direction;
                
                hasChangedDirection = true;
            }
        }
        else
        {
            // Reset the flag when the object is not touching the border
            hasChangedDirection = false;
        }

    }



    protected virtual void Update()
    {

        if(alert != null)
        {
            // Calculate the direction from the target to this object
            Vector2 directionToTarget = transform.position - alert.transform.position;

            // Calculate the angle between the direction to the target and the upward direction (Vector2.up)
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Rotate the object towards the target
            alert.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Orbit around the target
            alert.transform.RotateAround(transform.position, Vector3.forward, 10f * Time.deltaTime);
        }

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, hookDetectionRadius, transform.position.normalized, 0, hookLayer);     
        if(anim != null)
        {
            if (currentState == FishState.Caught)
            {
                //anim.SetBool("IsCaught", true);
            }
            else
            {
                //anim.SetBool("IsCaught", false);
            }
        }
            

        switch (currentState)
        {
            case FishState.Patrolling:
                if (alert != null) alert.SetActive(false);
                if (hit.collider != null && (hit.collider.gameObject.GetComponent<BaitHolder>().currentBait == fishScriptableObj.baitsUsed || hit.collider.gameObject.GetComponent<BaitHolder>().currentBait == BaitType.OmniBait) && !onReelState)
                {
                    if (isVisible || (!isVisible && hit.collider.gameObject.GetComponent<FishingRodBaseScript>().flashLightUnlocked == true))
                    {
                        if (!calledEnterEvent)
                        {
                            onFollowingHook?.Invoke();
                            currentState = FishState.Chasing;
                            calledExitEvent = false;
                            calledEnterEvent = true;
                        }
                    }

                    
                }
              
                timer += Time.deltaTime;

                if (timer >= changeDirectionCooldown)
                {
                    // Generate a random direction vector
                    targetDirection = UnityEngine.Random.insideUnitCircle.normalized;
                    //changeDirectionCooldown = UnityEngine.Random.Range(minCD, maxCD);
                    timer = 0;

                }

                //turning directions
                direction = Vector3.Lerp(direction, targetDirection, rotateSpeed * Time.deltaTime);
                direction = direction.normalized;

                HandleEnemyOutOfBounds();

                //random movement
                Vector3 movement = direction * speed * Time.deltaTime;
                transform.position += movement;
                
                RotateSprite();
        
                break;
            case FishState.Chasing:
                if (alert != null) alert.SetActive(true);

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
                    
                    HandleEnemyOutOfBounds();
                    transform.position = Vector2.MoveTowards(transform.position, hit.collider.gameObject.transform.position, speed * Time.deltaTime);
                    targetDirection = hit.collider.gameObject.transform.position - transform.position;
                    direction = Vector3.Lerp(direction, targetDirection, rotateSpeed * Time.deltaTime);
                    direction = direction.normalized;
                    RotateSprite();
                }
                
                if(hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= fishRadius && !onReelState)
                {
                    currentState = FishState.Caught;
                }
                break;
            case FishState.Caught:
                if (alert != null) alert.SetActive(false);
                if (!callCaughtEvent)
                {
                    onCaught?.Invoke(fishScriptableObj);
                    callCaughtEvent = true;
                }
                isCaught = true;
                
                if(hit.collider != null)
                {
                    transform.position = hit.collider.gameObject.transform.position;
                }
                break;
            default:
                break;

        }

    }




    protected virtual void OnDestroy()
    {
        FishingRodBaseScript.onReelState -= CheckReelState;
        FishingRodBaseScript.onFinishCaughtFish -= SaveFish;
        FishingRodBaseScript.onFishEscape -= Escape;
        WorldTime.onTimeChange -= OnTimeChange;

    }


}
