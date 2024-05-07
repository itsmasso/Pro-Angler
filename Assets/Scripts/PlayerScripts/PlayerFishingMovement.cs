using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFishingMovement : PlayerMovement
{
    [SerializeField] private PlayerMovement playerMovementScript;
    private Vector2 negativeReflect, positiveReflect;
    [Header("Sprite Properties")]
    [SerializeField] protected GameObject hook;
    [SerializeField] protected Animator anim;
    [SerializeField] protected FishingRodBaseScript rodScript;
    [SerializeField] protected Animator hookAnim;

    protected bool canMove;
    protected override void Start()
    {
        base.Start();
        canMove = true;
        ScreenManager.onScreenOpened += CanMove;
        positiveReflect = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        negativeReflect = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        FishingRodBaseScript.onFishing += CanMove;
    }

    protected void CanMove(bool _canMove)
    {
        canMove = !_canMove;
    }
    public override void OnMove(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            velocity = ctx.ReadValue<Vector2>();
        }
        else
        {
            velocity = Vector2.zero;
        }
        if (velocity != Vector2.zero)
        {
            hookAnim.SetBool("isMoving", true);
            rodScript.currentAnim.SetBool("isMoving", true);
            anim.SetBool("IsMoving", true);
        }
        else
        {
            hookAnim.SetBool("isMoving", false);
            rodScript.currentAnim.SetBool("isMoving", false);
            anim.SetBool("IsMoving", false);
        }
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
        FishingRodBaseScript.onFishing -= CanMove;
        ScreenManager.onScreenOpened -= CanMove;
    }
}
