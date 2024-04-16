using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMechanicState : HookBaseState
{
    private bool pointAtMiddle;
    private FishScriptableObject fishScriptable;
    

    private BarFishingMechanics fishingMechanicScript;
    private Vector2 startPosition;
    public override void EnterState(FishingRodBaseScript rod)
    {

        startPosition = rod.transform.position;
        pointAtMiddle = false;
        fishingMechanicScript = rod.fishingMechanicsScreen.GetComponent<BarFishingMechanics>();

        rod.CallReelStateEvent(true);
        rod.hookRb.velocity = Vector2.zero;
        if(!rod.fishCaught)
        {
            rod.SwitchState(rod.hookReelState);
        }
        else
        {
            //TODO: make mechanic scale

            //retrieving fish scriptable object from caught fish object
            RaycastHit2D hit = Physics2D.CircleCast(rod.hook.transform.position, rod.hookRadius, rod.hook.transform.position.normalized, 0, rod.fishLayer);
            if (hit.collider != null)
            {
                fishScriptable = hit.collider.gameObject.GetComponent<FishBaseScript>().fishScriptableObj;
            }
            rod.fishingMechanicsScreen.GetComponent<BarFishingMechanics>().fishScriptableObject = fishScriptable;
            rod.fishingMechanicsScreen.SetActive(true);
        }

        List<Transform> points = new List<Transform>();
        points.Add(rod.fishingRodPoint);
        points.Add(rod.fishingLineConnectorPoint);
        points.Add(rod.fishingLineAttatchmentPoint);
        rod.SetUpFishingLine(points);

        

    }



    public override void FixedUpdateState(FishingRodBaseScript rod)
    {
        
    }

    public override void UpdateState(FishingRodBaseScript rod)
    {
        rod.playerStam.DrainStamina((fishScriptable.strength * rod.staminaDrainScaler) * Time.deltaTime);

        Vector3 direction = rod.fishingLineAttatchmentPoint.position - rod.fishingRodPoint.position;
        Vector2 middlePointPosition = rod.fishingRodPoint.position + direction / 2f;
        rod.fishingLineConnectorPoint.position = Vector2.MoveTowards(rod.fishingLineConnectorPoint.position, middlePointPosition, rod.slowedReelSpeed * Time.deltaTime);
        List<Transform> points = new List<Transform>();
        points.Add(rod.fishingRodPoint);
        points.Add(rod.fishingLineAttatchmentPoint);
        rod.SetUpFishingLine(points);

        
        if (rod.fishingMechanicsScreen.GetComponent<BarFishingMechanics>().gainingProgress)
        {
            Vector2 targetPosition = Vector2.MoveTowards(rod.hook.transform.position, rod.fishingRodPoint.position, rod.slowedReelSpeed * Time.deltaTime);
            rod.hook.transform.position = targetPosition;
            
        }
        else
        {
            Vector2 targetPosition = Vector2.MoveTowards(rod.hook.transform.position, rod.fishingRodPoint.position, -rod.slowedReelSpeed * Time.deltaTime);
            rod.hook.transform.position = targetPosition;
        }

        rod.hook.transform.position = new Vector2(rod.hook.transform.position.x, Mathf.Clamp(rod.hook.transform.position.y, -80f, rod.waterLinePointY));
    }
}
