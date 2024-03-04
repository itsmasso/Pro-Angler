using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HookDescendState : HookBaseState
{
    private Vector2 targetPosition;
    private Rigidbody2D hookRb;
    private Transform hookTransform;
    private bool reachedHookLimit;

    private float currentDescendSpeed;
    private float currentMoveSpeed;
    private float timer;
    public override void EnterState(FishingRodBaseScript hook)
    {
        timer = 0;
        reachedHookLimit = false;
        currentDescendSpeed = hook.descendSpeed;
        currentMoveSpeed = hook.rodScriptableObj.moveSpeed;
        hookRb = hook.hookRb;
        hookTransform = hook.hook.transform;
        hookRb.isKinematic = true;

    }

    public override void FixedUpdateState(FishingRodBaseScript hook)
    {
       
        if (reachedHookLimit)
        {
            Vector2 targetPosition = hook.hook.transform.position + (Vector3)hook.direction * currentMoveSpeed * Time.fixedDeltaTime;
            targetPosition.y -= currentDescendSpeed * Time.fixedDeltaTime;
            currentDescendSpeed *= hook.slowDownRate;
            currentMoveSpeed *= hook.slowDownRate;
            hook.hookRb.MovePosition(targetPosition);
        }
        else
        {
            targetPosition = hookTransform.position + (Vector3)hook.direction * hook.rodScriptableObj.moveSpeed * Time.fixedDeltaTime;
            targetPosition.y -= hook.descendSpeed * Time.fixedDeltaTime;
            targetPosition = new Vector2(Mathf.Clamp(targetPosition.x, hook.minXBounds, hook.maxXBounds),
                Mathf.Clamp(targetPosition.y, hook.fishingRodPoint.position.y - hook.rodScriptableObj.fishingLineLength, hook.fishingRodPoint.position.y));
            hookRb.MovePosition(targetPosition);
        }
    }

    public override void UpdateState(FishingRodBaseScript hook)
    {
        RaycastHit2D hit = Physics2D.CircleCast(hookTransform.position, hook.hookRadius, hookTransform.position.normalized, 0, hook.fishLayer);
        if(hookTransform.position.y <= hook.fishingRodPoint.position.y - hook.rodScriptableObj.fishingLineLength || hit.collider != null)
        {
            reachedHookLimit = true;
        }

        if (reachedHookLimit)
        {
            timer += Time.deltaTime;
            if(timer >= hook.timeBeforeReel)
            {
                hook.SwitchState(hook.hookMechanicState);
            }
            
        }
    }
}
