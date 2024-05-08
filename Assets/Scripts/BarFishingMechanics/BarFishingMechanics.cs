
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BarFishingMechanics : MonoBehaviour
{
    public static event Action onCompletedProgress;
    public static event Action onFailed;

    public FishScriptableObject fishScriptableObject;
    public RodScriptableObject rodScriptableObject;

    private float leftPivot, rightPivot;

    [Header("Fish Indicator Properties")]   
    [SerializeField] private GameObject fishBar;
    [SerializeField] private float fishSpeed;
    [SerializeField] private float fishSpeedScaler;
    [SerializeField] private GameObject fishIndicator;  
    [SerializeField] private float timeMultiplier; //controls how often fish goes to a new destination. low means more difficult and high means easier
    [SerializeField] private float timeMultiplierScaler;
    private float fishSize;
    [SerializeField] private float fishSizeMultplier;
    [SerializeField] private float fishSizeScaler = 0.05f;

    private Vector2 currentDestination;
    private float newTargetSpeed;
    private float timer;
    private bool holdingSpace;
    

    [Header("Hook Indicator Properties")]
    [SerializeField] private GameObject hookIndicator;
    [SerializeField] private float hookSize;
    [SerializeField] private float hookPower = 0.5f;
    private float currentHookPower;  
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed = 10f; // Maximum speed of movement
    [SerializeField] private LayerMask targetLayer;

    [Header("Progress Properties")]
    [SerializeField] private Slider progressBarSlider;
    [SerializeField] private float currentProgress;
    [SerializeField] private float maxProgress;
    [SerializeField] private float progressIncreaseSpeed = 1f;
    [SerializeField] private float progressDecreaseSpeed = 1f;
    public bool gainingProgress;
    public bool losingProgress;
    [SerializeField] private float timeBeforeFail = 2.5f;
    private float failTimer;

    [Header("Buff Properties")]
    //[SerializeField] private float chanceToActivateScaler = 0.5f;
    //[SerializeField] private float checkBuffInterval;
    //[SerializeField] private float currentChanceToActivate;
    //[SerializeField] private float baseChanceToActivate;
    [SerializeField] private float progressStageToActivate;
    [SerializeField] private float buffActivateScaler = 1f;
    [SerializeField] private float chanceToDeactivate;
    [SerializeField] private float buffCooldown;
    //[SerializeField] private float buffDuration; //make it a range
    private float buffedTimeMultiplier;
    private float buffedSpeed;
    //private float activateTimer;
    private bool buffIsActive;
    private bool onBuffCooldown;
    //private float buffTimer;

    [Header("QTE Properties")]
    [SerializeField] private GameObject QTEobject;
    private float activateQTETimer;
    private bool hitQTE;
    [SerializeField] private float startQTEchance;
    [SerializeField] private float QTEcooldown;
    private float QTEtimer;
    private bool onQTEcooldown;
    [SerializeField] private float QTEduration;
    private bool currentlyInQTE;
    private bool activateQTE;

    private float QTEScaler(float strength)
    {
        float duration = 1;
        if(strength >= 0 && strength < 10)
        {
            duration = 3f;
        }else if (strength >= 10 && strength < 20)
        {
            duration = 2.25f;
        }
        else if (strength >= 20 && strength < 30)
        {
            duration = 2f;
        }
        else if (strength >= 30 && strength < 40)
        {
            duration = 1.75f;
        }
        else if (strength >= 40 && strength < 50)
        {
            duration = 1.3f;
        }
        else if (strength >= 50 && strength < 60)
        {
            duration = 1f;
        }
        else if (strength >= 60 && strength < 70)
        {
            duration = 0.75f;
        }
        else if (strength >= 70 && strength < 80)
        {
            duration = 0.5f;
        }
        else if (strength >= 80 && strength < 90)
        {
            duration = 0.3f;
        }
        else if(strength >= 90)
        {
            duration = 0.25f;
        }
        return duration;
    }

    private void DetermineFishSpeed()
    {
        if (rodScriptableObject.canSlow)
        {
            fishSpeed = fishScriptableObject.strength * fishSpeedScaler * (1-(rodScriptableObject.percentSlowAmount/100f));
        }
        else
        {
            fishSpeed = fishScriptableObject.strength * fishSpeedScaler;
        }
    }

    private void OnEnable()
    {
        //scaling the mechanics according to fish strength
        if (fishScriptableObject != null)
        {
            float strength = Mathf.Clamp((fishScriptableObject.strength - rodScriptableObject.rodStrength), 5, 100);
            DetermineFishSpeed();

            timeMultiplier = (100 - strength) * timeMultiplierScaler;
            maxProgress = strength / 2;
            //baseChanceToActivate = fishSciptableObject.strength * chanceToActivateScaler;
            progressStageToActivate = Mathf.Clamp((100 - strength) * buffActivateScaler, 20f, 75f);
            QTEduration = QTEScaler(strength);
            fishSizeMultplier = (100 - strength) * fishSizeScaler;
        }

        currentHookPower = hookPower;

        //Setting bools
        onBuffCooldown = false;
        buffIsActive = false;
        hitQTE = false;
        currentlyInQTE = false;
        onQTEcooldown = false;
        QTEobject.SetActive(false);
        activateQTE = false;
        //currentChanceToActivate = baseChanceToActivate;
        NewFishDestination();

        //Setting sizes and positions
        fishSize = fishSizeMultplier;
        fishIndicator.transform.localScale = new Vector2(fishSize, fishIndicator.transform.localScale.y);
        hookSize = hookIndicator.GetComponent<SpriteRenderer>().bounds.size.x;
        leftPivot = fishBar.transform.localPosition.x - fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        rightPivot = fishBar.transform.localPosition.x + fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;

        //Resetting numbers
        currentProgress = 0;
        currentSpeed = 0;
        failTimer = 0;
        timer = 0;
        QTEtimer = 0;
        hookIndicator.transform.localPosition = Vector2.zero;
        fishIndicator.transform.localPosition = Vector2.zero;
    }

    private void NewFishDestination()
    {
        newTargetSpeed = UnityEngine.Random.Range(fishSpeed - fishSpeed / 2, fishSpeed + fishSpeed / 2); //adjust range if needed
        currentDestination = new Vector2(UnityEngine.Random.Range(leftPivot, rightPivot), fishIndicator.transform.position.y); //TODO: change left and right pivot to accomodate for fish indicator size
    }
    
    public void HookIndicatorController(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            holdingSpace = true;
        if (ctx.canceled)
            holdingSpace = false;
    }

    private void HookIndicatorMovement()
    {
        //Code for hookindicator movement with no smoothing
        /*
        hookTargetPos = holdingSpace ? hookIndicator.transform.position + transform.right * hookPower * Time.deltaTime : 
            hookIndicator.transform.position + transform.right * -hookPower * Time.deltaTime;

        hookTargetPos.x = Mathf.Clamp(hookTargetPos.x, leftPivot + hookSize / 2, rightPivot - hookSize / 2);

        hookIndicator.transform.position = hookTargetPos;
        */

        
        currentSpeed = holdingSpace ? currentSpeed + currentHookPower * Time.deltaTime : currentSpeed - currentHookPower * Time.deltaTime;
        // Move the hookIndicator based on the current speed
        Vector3 movement = Vector3.right * currentSpeed * Time.deltaTime;
        Vector3 newPosition = hookIndicator.transform.localPosition + movement;

        // Clamp the position within pivot limits
        newPosition.x = Mathf.Clamp(newPosition.x, leftPivot + hookSize / 2, rightPivot - hookSize / 2);
        /*
        if (newPosition.x <= (leftPivot + hookSize / 2) || newPosition.x >= (rightPivot - hookSize / 2))
        {
            Debug.Log("current spsed = 0?");
            currentSpeed = 0f;
        }
        */

        if (Mathf.Approximately(newPosition.x, (leftPivot + hookSize/2)) || Mathf.Approximately(newPosition.x,(rightPivot - hookSize/2)))
        {
            
            currentSpeed = 0f;
        }
        //Debug.Log("hookPosition" + newPosition.x + "<=" + "left edge: " + (leftPivot + hookSize / 2));
        // Update the position of the hookIndicator
        hookIndicator.transform.localPosition = newPosition;
        
        


    }

        private void ProgressBar()
        {
        RaycastHit2D hit = Physics2D.BoxCast(fishIndicator.transform.position, new Vector2(fishSize, fishSize), 0, Vector2.zero, fishSize/2, targetLayer);
        Visualizer.BoxCast(fishIndicator.transform.position, new Vector2(fishSize, fishSize), 0, Vector2.zero, fishSize / 2, targetLayer);
        if (hit.collider != null)
        {
       
            failTimer = 0;
            gainingProgress = true;
            losingProgress = false;
            currentProgress += Time.deltaTime * progressIncreaseSpeed;
            //currentChanceToActivate = Mathf.Clamp(currentChanceToActivate++, baseChanceToActivate, 90f); //adjust if needed
            if (currentProgress >= maxProgress)
            {
                currentProgress = maxProgress;
                onCompletedProgress?.Invoke();
                gameObject.SetActive(false);
            }
                
        }
        else if(hit.collider == null)
        {
            gainingProgress = false;
            losingProgress = true;
            currentProgress -= Time.deltaTime * progressDecreaseSpeed;
            //currentChanceToActivate = Mathf.Clamp(currentChanceToActivate--, baseChanceToActivate, 90f); //adjust if needed
            if (currentProgress <= 0)
            {
                currentProgress = 0;
                failTimer += Time.deltaTime;
                if(failTimer >= timeBeforeFail)
                {
                    onFailed?.Invoke();
                    gameObject.SetActive(false);
                }

            }
                
        }

        progressBarSlider.value = currentProgress / maxProgress;
    }

    private void FishIndicatorMovement()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            NewFishDestination();
            timer = buffIsActive ? UnityEngine.Random.value * buffedTimeMultiplier: UnityEngine.Random.value * timeMultiplier;

        }
        float xPos = buffIsActive ? Mathf.Lerp(fishIndicator.transform.localPosition.x, currentDestination.x, buffedSpeed * Time.deltaTime) : Mathf.Lerp(fishIndicator.transform.localPosition.x, currentDestination.x, newTargetSpeed * Time.deltaTime);
        xPos = Mathf.Clamp(xPos, leftPivot + fishSize/2, rightPivot - fishSize / 2);
        fishIndicator.transform.localPosition = new Vector2(xPos, fishIndicator.transform.localPosition.y);
        
    }

    public void Counter(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            hitQTE = true;
        }
    }

    private void BuffEvent()
    {
        /*
        buffTimer += Time.deltaTime;
        if(buffTimer >= buffDuration && !currentlyInQTE)
        {
            buffIsActive = false;
            buffTimer = 0;
            
        }
        */

        if (onBuffCooldown && !currentlyInQTE )
        {
            buffIsActive = false;
            //buffTimer = 0;

        }

        if (!onQTEcooldown)
        {
            if (!currentlyInQTE)
            {
                activateQTETimer += Time.deltaTime;
                if (activateQTETimer >= 1) //checking to do QTE every however seconds
                {
                    float rand = UnityEngine.Random.value;
                    if (rand < startQTEchance)
                    {
                        activateQTE = true;

                    }
                    activateQTETimer = 0;
                }
            }

        }
    }

    private IEnumerator StunHook()
    {
        float stunDuration = 3 * (fishScriptableObject.strength / 100);
        currentHookPower = 0;
        yield return new WaitForSeconds(stunDuration);
        currentHookPower = hookPower;
    }

    private void QTE()
    {
        currentlyInQTE = true;
        QTEobject.SetActive(true);
        QTEtimer += Time.deltaTime;
        QTEobject.GetComponent<Slider>().value = QTEtimer / QTEduration;


        if (QTEtimer >= QTEduration && !hitQTE)
        {
            //fail counter
            //figure out correct interaction according to type of fish
            Debug.Log("Failed To Counter");
            if(fishScriptableObject.strength >= 30)
            {
                StartCoroutine(StunHook());
            }
            currentProgress -= currentProgress*0.2f; //reducing current progress by 20%
            buffIsActive = true;
            //buffTimer = 0;
            QTEobject.SetActive(false);
            StartCoroutine(ActivateQTECooldown());
            currentlyInQTE = false;
            activateQTE = false;
            QTEtimer = 0;

            if (currentProgress / maxProgress <= 0.10f) //if current progress is at 10% and you fail counter, you lose fish
            {
                onFailed?.Invoke();
                gameObject.SetActive(false);
            }

        }
        else if (hitQTE && QTEtimer < QTEduration)
        {
            //successful counter
            //figure out correct interaction according to type of rod
            Debug.Log("Successful Counter");
            //buffTimer = 0;
            if (rodScriptableObject.canStun)
            {
                StartCoroutine(StunFish());
            }
            QTEobject.SetActive(false);
            StartCoroutine(ActivateQTECooldown());
            currentlyInQTE = false;
            float rand = UnityEngine.Random.value;
            if(rand < chanceToDeactivate)
            {
                buffIsActive = false;
                StartCoroutine(ActivateBuffCooldown());
            }
            activateQTE = false;
            hitQTE = false;
            QTEtimer = 0;
        }
    }

    private IEnumerator StunFish()
    {
        fishSpeed = 0;
        yield return new WaitForSeconds(rodScriptableObject.stunDuration);
        DetermineFishSpeed();

    }

    private IEnumerator ActivateQTECooldown()
    {
        onQTEcooldown = true;
        Debug.Log("cooldown");
        yield return new WaitForSeconds(QTEcooldown);
        Debug.Log(" off cooldown");
        onQTEcooldown = false;
    }

    private IEnumerator ActivateBuffCooldown()
    {
        onBuffCooldown = true;
        yield return new WaitForSeconds(buffCooldown);
        onBuffCooldown = false;

    }
    
    
    void Update()
    {
        if(fishScriptableObject.strength >= 50)
        {
            progressDecreaseSpeed = 3 * (fishScriptableObject.strength / 100);
        }
        else
        {
            progressDecreaseSpeed = 1f;
        }
        buffedSpeed = newTargetSpeed * 2;
        buffedTimeMultiplier = timeMultiplier /2;
        if (!buffIsActive)
        {
            if((currentProgress/maxProgress * 100) >= progressStageToActivate && !onBuffCooldown)
            {
                buffIsActive = true;
            }


            /* CODE FOR ACTIVATING BUFF BASED ON RANDOM CHANCE
            currentChanceToActivate = baseChanceToActivate;
            activateTimer += Time.deltaTime;
            if(activateTimer >= checkBuffInterval)
            {
                float rand = UnityEngine.Random.value;
                if(rand < currentChanceToActivate)
                {
                    buffIsActive = true;
                }

            }
            */
        }
        else
        {
            //activateTimer = 0;
            BuffEvent();
        }

        if (activateQTE)
        {
            QTE();
        }

        if (buffIsActive)
        {
            fishIndicator.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            fishIndicator.GetComponent<SpriteRenderer>().color = Color.blue;
        }
      
        ProgressBar();
        HookIndicatorMovement();
        FishIndicatorMovement();
        leftPivot = fishBar.transform.localPosition.x - fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        rightPivot = fishBar.transform.localPosition.x + fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        


    }
}
