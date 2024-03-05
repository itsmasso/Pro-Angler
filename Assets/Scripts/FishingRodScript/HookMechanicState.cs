using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMechanicState : HookBaseState
{
    private float slowedReelSpeed = 0.75f;
    public override void EnterState(FishingRodBaseScript hook)
    {
        hook.CallReelStateEvent(true);
        hook.hookRb.velocity = Vector2.zero;
        if(hook.followingFishCount == 0 && hook.caughtFishCount == 0)
        {
            hook.SwitchState(hook.hookReelState);
        }
        else
        {
            hook.fishingMechanicsScreen.SetActive(true);
        }

        

    }



    public override void FixedUpdateState(FishingRodBaseScript hook)
    {
        
    }

    public override void UpdateState(FishingRodBaseScript hook)
    {
        hook.hook.transform.position = Vector2.MoveTowards(hook.hook.transform.position, hook.fishingRodPoint.position, slowedReelSpeed * Time.deltaTime);
    }
}
