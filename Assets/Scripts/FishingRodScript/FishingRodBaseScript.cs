using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class FishingRodBaseScript : MonoBehaviour
{
    public static event Action<bool> onReelState; //event to signal others that fishing rod is currently reeling in 
    public static event Action onDestroyCaughtFish;//destroy caught fish objects
    public static event Action onReplenishFish;
    public static event Action onFishEscape;

    public int followingFishCount;
    public int caughtFishCount;
    public LayerMask fishLayer;
    public float hookRadius = 0.5f;

    [Header("Reeling Properties")]
    public float slowDownRate = 0.97f;
    public float timeBeforeReel = 1.5f;
    public GameObject fishingMechanicsScreen;
    public float currentMechanicChance;
    public Transform mechanicBoundaryLine;

    public float descendSpeed; //how fast hook descends in the water
    public Vector2 direction;
    public RodScriptableObject rodScriptableObj;
    public Rigidbody2D hookRb;
    public float waterLinePointY;
    public bool isReeling;

    public GameObject hook;
    public BaitHolder baitHolder;

    public Transform fishingRodPoint; //point of where the string of the fishing rod is attatched
    public LineRenderer lineRenderer;
    public float minXBounds, maxXBounds;

    [Header("Throwing Properties")]
    public float throwForce = 1.5f;
    public float trajectoryTimeStep = 0.05f; //determining the time interval between consecutive points along the trajectory line
    public int numPoints = 15; // Number of points in the trajectory line

    public HookBaseState currentState;
    public HookThrowState hookThrowState = new HookThrowState();
    public HookDescendState hookDescendState = new HookDescendState();
    public HookReelState hookReelState = new HookReelState();
    public HookMechanicState hookMechanicState = new HookMechanicState();

    void Start()
    {
        
        baitHolder = hook.GetComponent<BaitHolder>();
        baitHolder.currentBait = BaitType.Bait1;

        FishBaseScript.onFollowingHook += AddFollowingFish;
        FishBaseScript.onExitHook += SubtractFollowingFish;
        FishBaseScript.onCaught += AddCaughtFish;
        FishingMechanicScript.onFinishedMechanic += FishMechanicCooldown;
        currentState = hookThrowState;
        currentState.EnterState(this);
        fishingMechanicsScreen.SetActive(false);
    }

    //TODO: keep adjusting these rates
    public void FishMechanicCooldown(float score)
    {
        if (score <= 0)
        {
            //TODO: call event to scatter fish
            caughtFishCount = 0;
            followingFishCount = 0;
            onFishEscape?.Invoke();
            currentMechanicChance = 0;
            SwitchState(hookReelState);
        }
        else if (score < 0.33)
        {
            //possibly do calculations to determine how long the cooldown is
            currentMechanicChance = score * 1.5f;
            SwitchState(hookReelState);
        }
        else if (score >= 0.33 && score < 0.66)
        {
            currentMechanicChance = score * 0.5f;
            SwitchState(hookReelState);

        }
        else if (score >= 0.66 && score < 1)
        {
            currentMechanicChance = score * 0.3f;
            SwitchState(hookReelState);
        }
        else if (score >= 1)
        {
            currentMechanicChance = 0f;
            SwitchState(hookReelState);
        }
    }

    public void OnHookMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        
    }

    public void CallReelStateEvent(bool isReelState)
    {
        onReelState?.Invoke(isReelState);
    }

    public void CallReplenishFishEvent()
    {
        onReplenishFish?.Invoke();
    }

    protected void AddFollowingFish()
    {
        followingFishCount++;
    }

    protected void SubtractFollowingFish()
    {
        if(followingFishCount > 0)
        {
            followingFishCount--;
        }
    }

    protected void AddCaughtFish()
    {
        caughtFishCount++;
    }

    public void CallDestroyCaughtFishEvent()
    {
        onDestroyCaughtFish?.Invoke();
    }

    public void SwitchState(HookBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
        
    }
    protected void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    protected void OnDestroy()
    {
        FishBaseScript.onFollowingHook -= AddFollowingFish;
        FishBaseScript.onExitHook -= SubtractFollowingFish;
        FishBaseScript.onCaught -= AddCaughtFish;
        FishingMechanicScript.onFinishedMechanic -= FishMechanicCooldown;
    }
}
