using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HookReelState : HookBaseState
{
    private GameObject hookObj;
    private float timer;
    private float spawnMechanicInterval = 2f;

    public override void EnterState(FishingRodBaseScript hook)
    {
        //Debug.Log("reel state");
        hook.CallReelStateEvent(true);
        hook.fishingMechanicsScreen.SetActive(false);
        hook.hookRb.velocity = Vector2.zero;
        hookObj = hook.hook;

    }

    public override void FixedUpdateState(FishingRodBaseScript hook)
    {

    }

    public override void UpdateState(FishingRodBaseScript hook)
    {
        //possibly add a check that automatically reels in anyway after a certain time in the case a bug happens
        float reelSpeed; //influence this speed by weight of how many fish there are too
        if (hook.currentMechanicChance == 0)
        {
            //if we end up not reeling any fish or we ended up getting a perfect reel score
            reelSpeed = hook.rodScriptableObj.reelSpeed * 2;
        }
        else
        {
            reelSpeed = hook.rodScriptableObj.reelSpeed;
        }

        hookObj.transform.position = Vector2.MoveTowards(hookObj.transform.position, hook.fishingRodPoint.position, reelSpeed * Time.deltaTime);



        if (hookObj.transform.position.y <= hook.mechanicBoundaryLine.position.y)
        {
            timer += Time.deltaTime;
            //will try to spawn the mechanic every 2 seconds
            if (timer >= spawnMechanicInterval)
            {
                float rand = Random.value;
                if (rand < hook.currentMechanicChance)
                {
                    hook.SwitchState(hook.hookMechanicState);
                }
                timer = 0;
            }
        }

        if (Vector2.Distance(hookObj.transform.position, hook.fishingRodPoint.position) <= 0.1f)
        {
            if (hook.caughtFishCount != 0)
            {
                //deleting fish object
                hook.CallDestroyCaughtFishEvent();
            }
            hook.SwitchState(hook.hookThrowState);

        }



    }
}
