using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMechanicState : HookBaseState
{
    private float slowedReelSpeed = 0.75f;
    private bool pointAtMiddle;
    //private float hitIndicatorSpeedFactor = 10f;
    private float numOfBarsScaler = 1.5f;
    private BarFishingMechanics fishingMechanicScript;
    //private HookIndicatorScript hookIndicator;
    public override void EnterState(FishingRodBaseScript rod)
    {
        pointAtMiddle = false;
        fishingMechanicScript = rod.fishingMechanicsScreen.GetComponent<BarFishingMechanics>();
        //hookIndicator = rod.fishingMechanicsScreen.GetComponent<FishingMechanicScript>().hookIndicator.GetComponent<HookIndicatorScript>();
        rod.CallReelStateEvent(true);
        rod.hookRb.velocity = Vector2.zero;
        if(!rod.fishCaught)
        {
            rod.SwitchState(rod.hookReelState);
        }
        else
        {       
            //TODO: make mechanic scale
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
        Vector3 direction = rod.fishingLineAttatchmentPoint.position - rod.fishingRodPoint.position;
        Vector2 middlePointPosition = rod.fishingRodPoint.position + direction / 2f;
        rod.fishingLineConnectorPoint.position = Vector2.MoveTowards(rod.fishingLineConnectorPoint.position, middlePointPosition, slowedReelSpeed * Time.deltaTime);
        if (Vector2.Distance(rod.fishingLineConnectorPoint.position, middlePointPosition) <= 0.1f && !pointAtMiddle)
        {
            List<Transform> points = new List<Transform>();
            points.Add(rod.fishingRodPoint);
            points.Add(rod.fishingLineAttatchmentPoint);
            rod.SetUpFishingLine(points);
            pointAtMiddle = true;
        }

    
        rod.hook.transform.position = Vector2.MoveTowards(rod.hook.transform.position, rod.fishingRodPoint.position, slowedReelSpeed * Time.deltaTime);
    }
}
