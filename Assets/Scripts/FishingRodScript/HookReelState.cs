using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class HookReelState : HookBaseState
{
    private GameObject hookObj;
    private float timer;
    private float spawnMechanicInterval = 2f;
    private bool pointAtMiddle;

    public override void EnterState(FishingRodBaseScript rod)
    {
        //Debug.Log("reel state");
        pointAtMiddle = false;
        rod.CallReelStateEvent(true);
        rod.fishingMechanicsScreen.SetActive(false);
        rod.hookRb.velocity = Vector2.zero;
        hookObj = rod.hook;


    }

    public override void FixedUpdateState(FishingRodBaseScript rod)
    {

    }

    public override void UpdateState(FishingRodBaseScript rod)
    {
      
        float reelSpeed; 
        reelSpeed = rod.rodScriptableObj.reelSpeed * 2;

        Vector3 direction = rod.fishingLineAttatchmentPoint.position - rod.fishingRodPoint.position;
        Vector2 middlePointPosition = rod.fishingRodPoint.position + direction / 2f;
        rod.fishingLineConnectorPoint.position = Vector2.MoveTowards(rod.fishingLineConnectorPoint.position, middlePointPosition, rod.rodScriptableObj.reelSpeed * 2 * Time.deltaTime);

        List<Transform> points = new List<Transform>();
        points.Add(rod.fishingRodPoint);
        points.Add(rod.fishingLineAttatchmentPoint);
        rod.SetUpFishingLine(points);

        float adjustedReelSpeed = Mathf.Clamp(reelSpeed / (rod.hookWeight / rod.referenceWeight), 0.5f, reelSpeed); 
        //Debug.Log(adjustedReelSpeed);
        hookObj.transform.position = Vector2.MoveTowards(hookObj.transform.position, new Vector3(rod.fishingRodPoint.position.x, rod.fishingRodPoint.position.y - rod.fishingRodPointOffset, 0), adjustedReelSpeed * Time.deltaTime);


        if (Vector2.Distance(hookObj.transform.position, new Vector2(rod.fishingRodPoint.position.x, rod.fishingRodPoint.position.y - rod.fishingRodPointOffset)) <= 0.1f)
        {
            if (rod.fishCaught)
            {
                //deleting fish object
                rod.CallOnFinishCaughtFishEvent();
            }
            rod.SwitchState(rod.hookIdleState);

        }



    }
}
