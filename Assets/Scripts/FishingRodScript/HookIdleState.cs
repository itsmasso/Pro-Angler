using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HookIdleState : HookBaseState
{
    public override void EnterState(FishingRodBaseScript rod)
    {
        AudioManager.Instance.StopAudioSource(AudioManager.Instance.reelingSFXSource);
        rod.hookAloneSprite.enabled = false;
        rod.fishingLineSprite.enabled = true;
        rod.CallStartedFishing(false);
        rod.hookRb.isKinematic = true;
    }

    public override void FixedUpdateState(FishingRodBaseScript rod)
    {
        
    }

    public override void UpdateState(FishingRodBaseScript rod)
    {
        if(rod.canFishPosition != null)
        {
            if (Mathf.Abs(rod.playerTransform.position.x - rod.canFishPosition.position.x) <= 0.3f)
            {
                rod.interactButtonUI.SetActive(true);

                if (rod.pressedInteract)
                {
                    if (!rod.outOfStamina && !rod.isbucketFull)
                    {
                        rod.interactButtonUI.SetActive(false);
                        rod.SwitchState(rod.hookThrowState);
                    }
                    else if (rod.outOfStamina || (rod.outOfStamina && rod.isbucketFull))
                    {
                        Debug.Log("Out of stamina!");
                    }
                    else
                    {
                        Debug.Log("Bucket is full! Please Sell your fish");
                    }

                }
            }
            else
            {
                rod.interactButtonUI.SetActive(false);
            }
        }
    }
}
