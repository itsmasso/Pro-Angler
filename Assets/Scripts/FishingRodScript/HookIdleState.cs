using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HookIdleState : HookBaseState
{
    public override void EnterState(FishingRodBaseScript rod)
    {
        rod.CallStartedFishing(false);
        rod.hookRb.isKinematic = true;
    }

    public override void FixedUpdateState(FishingRodBaseScript rod)
    {
        
    }

    public override void UpdateState(FishingRodBaseScript rod)
    {
        if(Mathf.Abs(rod.playerTransform.position.x - rod.canFishPosition.position.x) <= 0.3f)
        {
            rod.interactButtonUI.SetActive(true);
            
            if (rod.pressedInteract)
            {
                if (!rod.playerStam.outOfStamina)
                {
                    rod.interactButtonUI.SetActive(false);
                    rod.SwitchState(rod.hookThrowState);
                }
                else
                {
                    Debug.Log("Out of stamina!");
                }
                
            }
        }
        else
        {
            rod.interactButtonUI.SetActive(false);
        }
    }
}
