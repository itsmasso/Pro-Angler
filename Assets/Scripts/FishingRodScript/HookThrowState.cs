using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookThrowState : HookBaseState
{
    private Rigidbody2D hookRb;
    private float throwForce;
    private float trajectoryTimeStep; //determining the time interval between consecutive points along the trajectory line
    private int numPoints; // Number of points in the trajectory line
    private Vector2 velocity, startMousePos;
    private float waterLinePointY;
    private bool hookThrown;
    private bool canThrow;
    public override void EnterState(FishingRodBaseScript rod)
    {
        rod.CallStartedFishing(true);

        //resetting fish counts
        rod.fishCaught = false;
        rod.currentFishStrength = 0;
        rod.hookWeight = 0;

    //initializing variables (constructor kinda)
    hookRb = rod.hookRb;
        throwForce = rod.throwForce;
        trajectoryTimeStep = rod.trajectoryTimeStep;
        numPoints = rod.numPoints;
        waterLinePointY = rod.waterLinePointY;

        //setting booleans and setting rigidbody to dynamic
        hookThrown = false;
        hookRb.isKinematic = false;
        canThrow = false;

        //setting gravity to 0 so that hook stays in place until thrown;
        hookRb.gravityScale = 0;

        //resetting line renderers
        rod.trajectoryLine.positionCount = 0;
        List<Transform> points = new List<Transform>();
        points.Add(rod.fishingRodPoint);
        points.Add(rod.fishingLineAttatchmentPoint);
        rod.SetUpFishingLine(points);

        rod.CallReelStateEvent(false); //calling this event to set all bools in fish on reelstate to false so fish doesn't ignore hook
        
    }

    public override void FixedUpdateState(FishingRodBaseScript rod)
    {
        
    }

    public override void UpdateState(FishingRodBaseScript rod)
    {
        
        
        //change this code to use player input system later
        if (!hookThrown)
        {
            if(Input.GetMouseButtonDown(0) && Vector2.Distance(rod.mousePosition, rod.hook.transform.position) <= 0.5f)
            {
                canThrow = true;
            }

            if (canThrow)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //setting trajectory line back to 2 to make it visible
                    rod.trajectoryLine.positionCount = 2;
                    startMousePos = rod.mousePosition;
                }

                if (Input.GetMouseButton(0))
                {
                                                       
                    float dragDistance = Vector3.Distance(startMousePos, rod.mousePosition);
                    if (dragDistance > rod.maxThrowRadius)
                    {
                        // Calculate the direction from startMousePos to hookMousePos
                        Vector2 direction = (rod.mousePosition - startMousePos).normalized;

                        // Clamp the hookMousePos to maxDragDistance from startMousePos
                        rod.mousePosition = startMousePos + direction * rod.maxThrowRadius;
                    }
                    velocity = (startMousePos - rod.mousePosition) * throwForce;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    hookRb.gravityScale = 1;
                    hookRb.velocity = velocity;
                    rod.trajectoryLine.positionCount = 0;
                    hookThrown = true;
                    canThrow = false;
                }
            }


            //calculating trajectory
            Vector3[] positions = new Vector3[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                float t = i * trajectoryTimeStep;
                Vector3 pos = (Vector2)rod.hook.transform.position + velocity * t + 0.5f * Physics2D.gravity * t * t;
                positions[i] = pos;
            }

            rod.trajectoryLine.SetPositions(positions);
        }

        if (rod.hook.transform.position.y < waterLinePointY)
        {
            velocity = Vector2.zero; //causing bug?
            rod.SwitchState(rod.hookDescendState);
        }
    }
}
