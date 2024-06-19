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
    protected bool isWalking;

    [SerializeField] protected Transform groundTypeCheck;
    [SerializeField] protected string[] groundTypeLayerArr;
    protected override void Start()
    {
        base.Start();
        canMove = true;
        isWalking = false;
        WorldTime.onResetDay += DisableMovement;
        QoutaSystem.onContinueToNextDay += EnableMovement;
        ScreenManager.onScreenOpened += CanMove;
        positiveReflect = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        negativeReflect = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        FishingRodBaseScript.onFishing += CanMove;
        PauseMenu.onPaused += CanMove;
    }


    protected void DisableMovement()
    {
        canMove = false;
    }

    protected void EnableMovement()
    {
        canMove = true;
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

    protected void PlayWalkingSFX(string name)
    {

        if (velocity != Vector2.zero && !isWalking)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.walkingSFXSource, name, true);
            isWalking = true;

        }
        else if (isWalking && velocity == Vector2.zero)
        {
            AudioManager.Instance.StopAudioSource(AudioManager.Instance.walkingSFXSource);
            isWalking = false;
        }
    }

    protected void DetermineGroundType()
    {
        int excludedLayer = LayerMask.NameToLayer("Player");
        int layerMask = ~(1 << excludedLayer);
        Collider2D collider = Physics2D.OverlapCircle(groundTypeCheck.position, 0.1f, layerMask);
        string layerName = LayerMask.LayerToName(collider.gameObject.layer);
    
        switch (layerName)
        {
            case "Sand":
                PlayWalkingSFX("WalkingOnSandSFX");
                break;
            case "Wood":
                PlayWalkingSFX("WalkingOnWoodSFX");
                break;
            default:
                break;

        }
        
    }
    protected override void Update()
    {
        base.Update();
        FlipSprite();
        DetermineGroundType();
        

    }


    private void OnDestroy()
    {
        QoutaSystem.onContinueToNextDay -= EnableMovement;
        WorldTime.onResetDay -= DisableMovement;
        FishingRodBaseScript.onFishing -= CanMove;
        ScreenManager.onScreenOpened -= CanMove;
    }
}
