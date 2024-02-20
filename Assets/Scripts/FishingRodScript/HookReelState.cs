using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HookReelState : HookBaseState
{
    private GameObject hookObj;
    public override void EnterState(FishingRodBaseScript hook)
    {
        hook.CallReelStateEvent(true);
        hook.hookRb.velocity = Vector2.zero;
        hookObj = hook.hook;
        Debug.Log("Reel state");
    }

    public override void FixedUpdateState(FishingRodBaseScript hook)
    {
       
    }

    public override void UpdateState(FishingRodBaseScript hook)
    {
        //possibly add a check that automatically reels in anyway after a certain time in the case a bug happens
        if (hook.caughtFishCount == hook.followingFishCount)
        {
            hookObj.transform.position = Vector2.MoveTowards(hookObj.transform.position, hook.fishingRodPoint.position, hook.rodScriptableObj.reelSpeed * Time.deltaTime);
        }

        if(Vector2.Distance(hookObj.transform.position, hook.fishingRodPoint.position) <= 0.1f)
        {
            //add fish to collected fish
            //deleting fish object
            hook.CallDestroyCaughtFishEvent();
            hook.SwitchState(hook.hookThrowState);

        }
     
    }
}
