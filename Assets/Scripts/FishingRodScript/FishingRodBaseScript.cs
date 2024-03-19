using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public abstract class FishingRodBaseScript : MonoBehaviour
{
    //events
    public static event Action<bool> onReelState; //event to signal others that fishing rod is currently reeling in 
    public static event Action onDestroyCaughtFish;//destroy caught fish objects
    public static event Action onFishEscape;

    [Header("Fishing Line Properties")]
    public LineRenderer fishingLine;
    public Transform fishingLineAttatchmentPoint;
    public Transform fishingLineConnectorPoint; //this point will stop at the water line on contact to create a new line going down
    public List<Transform> points = new List<Transform>();

    [Header("Fishing Rod Properties")]
    public Transform fishingRodPoint; //point of where the string of the fishing rod is attatched
    public float fishingRodPointOffset = 2f;
    public BaitHolder baitHolder;
    public RodScriptableObject rodScriptableObj;

    [Header("Fish Info")]
    public LayerMask fishLayer;
    public float currentFishStrength;
    public float hookWeight;
    public float referenceWeight = 6f; //number used to scale the proportation between weight and reel speed

    [Header("Hook Properties")]
    public GameObject hook;
    public Rigidbody2D hookRb;
    public float hookRadius = 0.5f;  

    [Header("Throwing Properties")]
    public LineRenderer trajectoryLine;
    public float throwForce = 1.5f;
    public float trajectoryTimeStep = 0.05f; //determining the time interval between consecutive points along the trajectory line
    public int numPoints = 15; // Number of points in the trajectory line
    public Vector2 mousePosition;
    public float maxThrowRadius = 4;

    [Header("Descending Properties")]
    public float descendSpeed; //how fast hook descends in the water
    public Vector2 direction;

    [Header("Reeling Properties")]
    public bool isReeling;
    public float slowDownRate = 0.97f;
    public float timeBeforeReel = 1.5f;

    [Header("Mechanic Properties")]
    public GameObject fishingMechanicsScreen;
    public Transform mechanicBoundaryLine;
    public bool fishCaught;
    

    [Header("Map/Environment Properties")]
    public float waterLinePointY;
    public float minXBounds, maxXBounds;

    [Header("State Info")]
    public HookBaseState currentState;
    public HookThrowState hookThrowState = new HookThrowState();
    public HookDescendState hookDescendState = new HookDescendState();
    public HookReelState hookReelState = new HookReelState();
    public HookMechanicState hookMechanicState = new HookMechanicState();

    void Start()
    {
        //SetUpFishingLine(points);
        baitHolder = hook.GetComponent<BaitHolder>();
        baitHolder.currentBait = BaitType.Tier1Bait;

        //FishBaseScript.onExitHook += SubtractFollowingFish;
        FishBaseScript.onCaught += CaughtFish;
        BarFishingMechanics.onCompletedProgress += OnSuccess;
        BarFishingMechanics.onFailed += OnFailure;
        currentState = hookThrowState;
        currentState.EnterState(this);
        fishingMechanicsScreen.SetActive(false);
    }

    public void SetUpFishingLine(List<Transform> points)
    {
        fishingLine.positionCount = points.Count;
        this.points = points;
    }

    private void OnSuccess()
    {
        SwitchState(hookReelState);
    }

    private void OnFailure()
    {
        onFishEscape?.Invoke();
        SwitchState(hookReelState);
    }

    public void OnMousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
       

    }
    public void OnHookMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        
    }

    public void CallReelStateEvent(bool isReelState)
    {
        onReelState?.Invoke(isReelState);
    }


    protected void CaughtFish(float strength, float weight)
    {
        fishCaught = true;
        currentFishStrength = strength;
        hookWeight = weight;
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
        for(int i = 0; i < points.Count; i++)
        {
            fishingLine.SetPosition(i, points[i].position);
        }
    }
    protected void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    protected void OnDestroy()
    {
        //FishBaseScript.onExitHook -= SubtractFollowingFish;
        FishBaseScript.onCaught -= CaughtFish;
        BarFishingMechanics.onCompletedProgress -= OnSuccess;
        BarFishingMechanics.onFailed -= OnFailure;
    }
}
