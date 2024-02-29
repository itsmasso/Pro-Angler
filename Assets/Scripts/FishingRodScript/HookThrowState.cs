using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookThrowState : HookBaseState
{
    private Rigidbody2D hookRb;
    private float throwForce;
    private float trajectoryTimeStep; //determining the time interval between consecutive points along the trajectory line
    private int numPoints; // Number of points in the trajectory line
    private Vector2 velocity, startMousePos, currentMousePos;
    private float waterLinePointY;
    private bool hookThrown;
    public override void EnterState(FishingRodBaseScript hook)
    {
        //resetting fish counts
        hook.followingFishCount = 0;
        hook.caughtFishCount = 0;

        //initializing variables (constructor kinda)
        hookRb = hook.hookRb;
        throwForce = hook.throwForce;
        trajectoryTimeStep = hook.trajectoryTimeStep;
        numPoints = hook.numPoints;
        waterLinePointY = hook.waterLinePointY;

        //setting booleans and setting rigidbody to dynamic
        hookThrown = false;
        hookRb.isKinematic = false;

        //setting gravity to 0 so that hook stays in place until thrown;
        hookRb.gravityScale = 0;

        //setting trajectory line back to 0 to make it not visible
        hook.lineRenderer.positionCount = 0;

        hook.CallReelStateEvent(false); //calling this event to set all bools in fish on reelstate to false so fish doesn't ignore hook
    }

    public override void FixedUpdateState(FishingRodBaseScript hook)
    {
        
    }

    public override void UpdateState(FishingRodBaseScript hook)
    {
        //change this code to use player input system later
        if (!hookThrown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //setting trajectory line back to 2 to make it visible
                hook.lineRenderer.positionCount = 2;
                startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                velocity = (startMousePos - currentMousePos) * throwForce;
            }

            if (Input.GetMouseButtonUp(0))
            {
                hookRb.gravityScale = 1;
                hookRb.velocity = velocity;
                hook.lineRenderer.positionCount = 0;
                hookThrown = true;
            }

           //calculating trajectory
            Vector3[] positions = new Vector3[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                float t = i * trajectoryTimeStep;
                Vector3 pos = (Vector2)hook.fishingRodPoint.position + velocity * t + 0.5f * Physics2D.gravity * t * t;
                positions[i] = pos;
            }

            hook.lineRenderer.SetPositions(positions);
        }

        if (hook.hook.transform.position.y < waterLinePointY)
        {
            velocity = Vector2.zero;
            hook.SwitchState(hook.hookDescendState);
        }
    }
}
