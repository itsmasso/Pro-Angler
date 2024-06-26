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
    public override void EnterState(FishingRodBaseScript rod)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.reelingSFXSource, "ReelingSFX", true);
        timer = 0;
        reachedHookLimit = false;
        currentDescendSpeed = rod.descendSpeed;
        currentMoveSpeed = rod.rodScriptableObj.moveSpeed;
        hookRb = rod.hookRb;
        hookTransform = rod.hook.transform;
        hookRb.isKinematic = true;

        List<Transform> points = new List<Transform>();
        points.Add(rod.fishingRodPoint);
        points.Add(rod.fishingLineConnectorPoint);
        points.Add(rod.fishingLineAttatchmentPoint);
        rod.SetUpFishingLine(points);

    }

    public override void FixedUpdateState(FishingRodBaseScript rod)
    {
       
        if (reachedHookLimit)
        {
            Vector2 targetPosition = rod.hook.transform.position + (Vector3)rod.direction * currentMoveSpeed * Time.fixedDeltaTime;
            targetPosition.y -= currentDescendSpeed * Time.fixedDeltaTime;
            currentDescendSpeed *= rod.slowDownRate;
            currentMoveSpeed *= rod.slowDownRate;
            rod.hookRb.MovePosition(targetPosition);
        }
        else
        {

            targetPosition = hookTransform.position + (Vector3)rod.direction * rod.rodScriptableObj.moveSpeed * Time.fixedDeltaTime;
            if (rod.reelEarly)
            {
                hookTransform.position = Vector2.MoveTowards(hookTransform.position, new Vector3(rod.fishingRodPoint.position.x, rod.fishingRodPoint.position.y - rod.fishingRodPointOffset, 0), rod.rodScriptableObj.reelSpeed * Time.deltaTime);
                if (hookTransform.position.y >= rod.waterLinePointY)
                {
                    rod.SwitchState(rod.hookMechanicState);
                }
            }
            else
            {
                targetPosition.y -= rod.descendSpeed * Time.fixedDeltaTime;
            }
            
            targetPosition = new Vector2(Mathf.Clamp(targetPosition.x, rod.minXBounds, rod.maxXBounds), //subtract 1 to provide some buffer 
                Mathf.Clamp(targetPosition.y, rod.fishingRodPoint.position.y - rod.rodScriptableObj.fishingLineLength - 1, rod.fishingRodPoint.position.y));
            hookRb.MovePosition(targetPosition);
        }
    }

    public override void UpdateState(FishingRodBaseScript rod)
    {
        
        rod.fishingLineConnectorPoint.position = new Vector2(rod.hook.transform.position.x, rod.waterLinePointY);

        RaycastHit2D hit = Physics2D.CircleCast(hookTransform.position, rod.hookRadius, hookTransform.position.normalized, 0, rod.fishLayer);
        if (hookTransform.position.y <= rod.fishingRodPoint.position.y - rod.rodScriptableObj.fishingLineLength
            || (hit.collider != null && hit.collider.transform.GetComponent<FishBaseScript>().fishScriptableObj.baitsUsed == rod.baitHolder.currentBait))
        {
            reachedHookLimit = true;
        }

        
        if (reachedHookLimit)
        {
            timer += Time.deltaTime;
            if(timer >= rod.timeBeforeReel || rod.fishCaught)
            {
                rod.SwitchState(rod.hookMechanicState);
            }
            
        }
    }
}
