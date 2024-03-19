
using System;
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
    private float timer;

    private bool holdingSpace;
    [SerializeField] private float hookPower = 0.5f;
    [SerializeField] private float hookSize;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed = 10f; // Maximum speed of movement
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private float currentProgress;
    [SerializeField] private float maxProgress;
    [SerializeField] private float progressIncreaseSpeed = 1f;
    [SerializeField] private float progressDecreaseSpeed = 1f;

    [SerializeField] private float timeBeforeFail = 2.5f;
    private float failTimer;


    void Start()
    {
        
        NewFishDestination();
        hookSize = hookIndicator.GetComponent<SpriteRenderer>().bounds.size.x;
        leftPivot = fishBar.transform.position.x - fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        rightPivot = fishBar.transform.position.x + fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        currentProgress = 0;
        hookIndicator.transform.localPosition = Vector2.zero;
        fishIndicator.transform.localPosition = Vector2.zero;
    
        //calledOnce = false;
    }

    private void OnEnable()
    {
        NewFishDestination();
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
        currentDestination = new Vector2(UnityEngine.Random.Range(leftPivot, rightPivot), fishIndicator.transform.position.y);
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

        
        currentSpeed = holdingSpace ? currentSpeed + hookPower * Time.deltaTime : currentSpeed - hookPower * Time.deltaTime;
        // Move the hookIndicator based on the current speed
        Vector3 movement = Vector3.right * currentSpeed * Time.deltaTime;
        Vector3 newPosition = hookIndicator.transform.position + movement;

        // Clamp the position within pivot limits
        newPosition.x = Mathf.Clamp(newPosition.x, leftPivot + hookSize / 2, rightPivot - hookSize / 2);
        /*
        if (newPosition.x <= (leftPivot + hookSize / 2) || newPosition.x >= (rightPivot - hookSize / 2))
        {
            Debug.Log("current spsed = 0?");
            currentSpeed = 0f;
        }
        */
        if (Mathf.Approximately(newPosition.x, (leftPivot + hookSize / 2)) || Mathf.Approximately(newPosition.x,(rightPivot - hookSize / 2)))
        {
            
            currentSpeed = 0f;
        }
        //Debug.Log("hookPosition" + newPosition.x + "<=" + "left edge: " + (leftPivot + hookSize / 2));
        // Update the position of the hookIndicator
        hookIndicator.transform.position = newPosition;
        
        


    }


        private void ProgressBar()
        {
        RaycastHit2D hit = Physics2D.BoxCast(hookIndicator.transform.position, new Vector2(hookSize, hookSize), 0, hookIndicator.transform.position, 0, targetLayer);
        
        if (hit.collider != null)
        {
        
            failTimer = 0;
            currentProgress += Time.deltaTime * progressIncreaseSpeed;
            if (currentProgress >= maxProgress)
            {
                currentProgress = maxProgress;
                onCompletedProgress?.Invoke();
                gameObject.SetActive(false);
            }
                
        }
        else if(hit.collider == null)
        {
            currentProgress -= Time.deltaTime * progressDecreaseSpeed;
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
            timer = UnityEngine.Random.value * timeMultiplier;

        }
        float xPos = Mathf.Lerp(fishIndicator.transform.position.x, currentDestination.x, newTargetSpeed * Time.deltaTime);
        fishIndicator.transform.position = new Vector2(xPos, fishIndicator.transform.position.y);
    }
    
    void Update()
    {

        ProgressBar();
        leftPivot = fishBar.transform.position.x - fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        rightPivot = fishBar.transform.position.x + fishBar.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        HookIndicatorMovement();
        FishIndicatorMovement();


    }
}
