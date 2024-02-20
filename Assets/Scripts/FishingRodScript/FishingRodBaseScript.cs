using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class FishingRodBaseScript : MonoBehaviour
{
    public static event Action<bool> onReelState; //event to signal others that fishing rod is currently reeling in 
    public static event Action onDestroyCaughtFish;//destroy caught fish objects

    public int followingFishCount;
    public int caughtFishCount;

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

    void Start()
    {
        baitHolder = hook.GetComponent<BaitHolder>();
        baitHolder.currentBait = BaitType.Bait1;

        FishBaseScript.onFollowingHook += AddFollowingFish;
        FishBaseScript.onExitHook += SubtractFollowingFish;
        FishBaseScript.onCaught += AddCaughtFish;
        currentState = hookThrowState;
        currentState.EnterState(this);      
    }


    public void OnHookMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        
    }

    public void OnReelIn(InputAction.CallbackContext ctx)
    {
        /*
        if (ctx.performed)
            isReeling = true;
        if (ctx.canceled)
            isReeling = false;
        */
    }

    public void CallReelStateEvent(bool isReelState)
    {
        onReelState?.Invoke(isReelState);
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
    }
}
