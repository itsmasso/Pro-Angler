using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sardine : FishBaseScript
{

    //[SerializeField] protected float razorBladeRadius;
    //[SerializeField] protected LayerMask targetLayer;
    //protected float timer;
    //[SerializeField] protected float hitCooldown = 1.5f;
    //[SerializeField] protected int damageAmount;
    //public bool canRazorBlade;

    protected override void Start()
    {
        base.Start();
        //canRazorBlade = true;
    }


    protected override void Update()
    {
        base.Update();
        /*
        if(currentState != FishState.Caught && canRazorBlade && !onReelState)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, razorBladeRadius, transform.position, 0, targetLayer);
            if (hit.collider != null)
            {
                timer += Time.deltaTime;
                if (timer >= hitCooldown)
                {
                    hit.collider.gameObject.GetComponent<HookHitBox>().DamageHook(damageAmount);
                    timer = 0;
                }
            }
        }
        */
        
    }


}
