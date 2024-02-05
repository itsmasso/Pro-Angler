using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class HookBaseScript : MonoBehaviour
{
    [SerializeField] private float descendSpeed; //how fast hook descends in the water
    [SerializeField] private Transform fishingRodPoint; //point of where the string of the fishing rod is attatched
    private Vector2 direction;
    private Vector2 targetPosition;
    private bool isReeling;
    [SerializeField] private bool descending;
    public RodScriptableObject rodScriptableObj;
    [SerializeField] private Rigidbody2D rb;


    void Start()
    {
        descending = true;
    }

    public void OnHookMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        
    }

    public void OnReelIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            isReeling = true;
        if (ctx.canceled)
            isReeling = false;
        
    }

    public void OnIncreaseFishingLine(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (descending)
            {
                descending = false;
            }
            else
            {
                descending = true;
            }
        }
           
        

    }


    void Update()
    {
        if (descending)
        {
            direction.y = -descendSpeed;
        }
        else
        {
            direction.y = 0;
        }
        targetPosition = (Vector2)transform.position + direction * rodScriptableObj.reelSpeed * Time.fixedDeltaTime;
        targetPosition = new Vector2(
            Mathf.Clamp(targetPosition.x, fishingRodPoint.position.x - rodScriptableObj.fishingLineLength, fishingRodPoint.position.x + rodScriptableObj.fishingLineLength),
            Mathf.Clamp(targetPosition.y, fishingRodPoint.position.y - rodScriptableObj.fishingLineLength, fishingRodPoint.position.y + rodScriptableObj.fishingLineLength));

        if (isReeling)
        {
            transform.position = Vector2.MoveTowards(transform.position, fishingRodPoint.position, rodScriptableObj.reelSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(targetPosition);
    }
}
