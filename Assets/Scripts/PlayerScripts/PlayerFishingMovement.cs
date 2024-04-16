using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishingMovement : PlayerMovement
{
    [SerializeField] private PlayerMovement playerMovementScript;
    private Vector2 negativeReflect, positiveReflect;
    [Header("Sprite Properties")]
    [SerializeField] protected GameObject hook;
    protected override void Start()
    {
        base.Start();
        positiveReflect = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        negativeReflect = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        FishingRodBaseScript.onFishing += DisablePlayerMovement;
    }

    private void DisablePlayerMovement(bool isFishing)
    {
        playerMovementScript.enabled = !isFishing;
    }
    private void FlipSprite()
    {
        if (velocity.x < 0)
        {

            transform.localScale = positiveReflect;
            hook.transform.localScale = positiveReflect;

        }
        else if (velocity.x > 0)
        {

            transform.localScale = negativeReflect;
            hook.transform.localScale = negativeReflect;
        }

    }
    protected override void Update()
    {
        base.Update();
        FlipSprite();
    }

    private void OnDestroy()
    {
        FishingRodBaseScript.onFishing -= DisablePlayerMovement;
    }
}
