
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BarFishingMechanics : MonoBehaviour
{
    public static event Action onCompletedProgress;
    public static event Action onFailed;

    [SerializeField] private GameObject fishIndicator;
    [SerializeField] private GameObject hookIndicator;
    [SerializeField] private GameObject fishBar;
    [SerializeField] private Slider progressBarSlider;
    private float leftPivot, rightPivot;

    private Vector2 currentDestination;
    //private bool calledOnce;
    [SerializeField] private float fishSpeed;
    private float newTargetSpeed;
    [SerializeField] private float timeMultiplier;
    private float buffedTimeMultiplier;
    private float timer;

    private bool holdingSpace;
    [Header("Hook Indicator Properties")]
    [SerializeField] private float hookSize;
    [SerializeField] private float hookPower = 0.5f;
    private float currentHookPower;
    public float fishSize;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed = 10f; // Maximum speed of movement
    [SerializeField] private LayerMask targetLayer;

    [Header("Progress Properties")]
    [SerializeField] private float currentProgress;
    [SerializeField] private float maxProgress;
    [SerializeField] private float progressIncreaseSpeed = 1f;
    [SerializeField] private float progressDecreaseSpeed = 1f;

    [SerializeField] private float timeBeforeFail = 2.5f;
    private float failTimer;

    [Header("Buff Properties")]
    [SerializeField] private float checkBuffInterval;
    [SerializeField] private float currentChanceToActivate;
    [SerializeField] private float baseChanceToActivate;
    [SerializeField] private float chanceToDeactivate;
    [SerializeField] private float buffDuration; //make it a range
    private float buffedSpeed;
    private float activateTimer;
    private bool buffIsActive;
    private float buffTimer;

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

    public bool gainingProgress;
    public bool losingProgress;
    void Start()
    {
        currentHookPower = hookPower;
        hitQTE = false;
        currentlyInQTE = false;
        QTEobject.SetActive(false);
        currentChanceToActivate = baseChanceToActivate;
        NewFishDestination();
        fishSize = fishIndicator.GetComponent<SpriteRenderer>().bounds.size.x;
        hookSize = hookIndicator.GetComponent<SpriteRenderer>().bounds.size.x;
        leftPivot = fishBar.transform.localPosition.x - fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        rightPivot = fishBar.transform.localPosition.x + fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        currentProgress = 0;
        hookIndicator.transform.localPosition = Vector2.zero;
        fishIndicator.transform.localPosition = Vector2.zero;
    
        //calledOnce = false;
    }

    private void OnEnable()
    {
        currentHookPower = hookPower;
        hitQTE = false;
        currentlyInQTE = false;
        QTEobject.SetActive(false);
        currentChanceToActivate = baseChanceToActivate;
        NewFishDestination();
        fishSize = fishIndicator.GetComponent<SpriteRenderer>().bounds.size.x;
        hookSize = hookIndicator.GetComponent<SpriteRenderer>().bounds.size.x;
        leftPivot = fishBar.transform.localPosition.x - fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        rightPivot = fishBar.transform.localPosition.x + fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        currentProgress = 0;
        currentSpeed = 0;
        failTimer = 0;
        timer = 0;
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
            currentChanceToActivate = Mathf.Clamp(currentChanceToActivate++, baseChanceToActivate, 90f); //adjust if needed
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
            currentChanceToActivate = Mathf.Clamp(currentChanceToActivate--, baseChanceToActivate, 90f); //adjust if needed
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
        xPos = Mathf.Clamp(xPos, leftPivot, rightPivot);
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
        buffTimer += Time.deltaTime;
        if(buffTimer >= buffDuration && !currentlyInQTE)
        {
            buffIsActive = false;
            buffTimer = 0;
            
        }

        if (!onQTEcooldown)
        {
            if (!currentlyInQTE)
            {
                activateQTETimer += Time.deltaTime;
                if (activateQTETimer >= 2)
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
            currentProgress -= currentProgress*0.2f; //reducing current progress byt 20%
            buffIsActive = true;
            buffTimer = 0;
            QTEobject.SetActive(false);
            StartCoroutine(ActivateQTECooldown());
            currentlyInQTE = false;
            activateQTE = false;
            QTEtimer = 0;

        }
        else if (hitQTE && QTEtimer < QTEduration)
        {
            //successful counter
            //figure out correct interaction according to type of rod
            Debug.Log("Successful Counter");
            buffTimer = 0;
            QTEobject.SetActive(false);
            StartCoroutine(ActivateQTECooldown());
            currentlyInQTE = false;
            float rand = UnityEngine.Random.value;
            if(rand < chanceToDeactivate)
            {
                buffIsActive = false;
            }
            activateQTE = false;
            hitQTE = false;
            QTEtimer = 0;
        }
    }

    private IEnumerator ActivateQTECooldown()
    {
        onQTEcooldown = true;
        yield return new WaitForSeconds(QTEcooldown);
        onQTEcooldown = false;
    }
    
    void Update()
    {
        
        buffedSpeed = newTargetSpeed * 2;
        buffedTimeMultiplier = timeMultiplier /2;
        if (!buffIsActive)
        {
            currentChanceToActivate = baseChanceToActivate;
            activateTimer += Time.deltaTime;
            if(activateTimer >= checkBuffInterval)
            {
                float rand = UnityEngine.Random.value;
                if(rand < currentChanceToActivate)
                {
                    Debug.Log("buff activated");
                    buffIsActive = true;
                }

            }
        }
        else
        {
            activateTimer = 0;
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
