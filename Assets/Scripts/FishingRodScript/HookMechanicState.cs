using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMechanicState : HookBaseState
{
    private int followingFish;
    public override void EnterState(FishingRodBaseScript hook)
    {
        hook.CallReelStateEvent(true);
        followingFish = hook.followingFishCount;
        if(followingFish == 0)
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
        hook.hookRb.velocity = Vector2.zero;
    }

    public override void UpdateState(FishingRodBaseScript hook)
    {
       
    }
}
