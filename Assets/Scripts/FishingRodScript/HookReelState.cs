using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HookReelState : HookBaseState
{
    private GameObject hookObj;
    private bool isReelingFish;
    private float currentDescendSpeed;
    private float currentMoveSpeed;
    private float timer;
    public override void EnterState(FishingRodBaseScript hook)
    {
        timer = 0;
        isReelingFish = false;
        currentDescendSpeed = hook.descendSpeed;
        currentMoveSpeed = hook.rodScriptableObj.moveSpeed;
        hook.CallReelStateEvent(true);
        hook.hookRb.velocity = Vector2.zero;
        hookObj = hook.hook;

    }

    public override void FixedUpdateState(FishingRodBaseScript hook)
    {
        if (!isReelingFish)
        {
            Vector2 targetPosition = hookObj.transform.position + (Vector3)hook.direction * currentMoveSpeed * Time.fixedDeltaTime;
            targetPosition.y -= currentDescendSpeed * Time.fixedDeltaTime;
            currentDescendSpeed *= hook.slowDownRate;
            currentMoveSpeed *= hook.slowDownRate;
            hook.hookRb.MovePosition(targetPosition);
        }
    }

    public override void UpdateState(FishingRodBaseScript hook)
    {
        timer += Time.deltaTime;

        //possibly add a check that automatically reels in anyway after a certain time in the case a bug happens
        if(timer >= hook.timeBeforeReel)
        {
            if (hook.caughtFishCount == hook.followingFishCount)
            {
                isReelingFish = true;
                hookObj.transform.position = Vector2.MoveTowards(hookObj.transform.position, hook.fishingRodPoint.position, hook.rodScriptableObj.reelSpeed * Time.deltaTime);
            }

            if (Vector2.Distance(hookObj.transform.position, hook.fishingRodPoint.position) <= 0.1f)
            {
                //add fish to collected fish
                hook.CallReplenishFishEvent();
                //deleting fish object
                hook.CallDestroyCaughtFishEvent();
                hook.SwitchState(hook.hookThrowState);

            }
        }
     
    }
}
