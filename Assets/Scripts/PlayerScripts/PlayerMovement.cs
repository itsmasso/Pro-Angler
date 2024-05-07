using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;

    [Header("Movement Properties")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float jumpForce = 10f;
    protected Vector2 velocity;

    [Header("GroundCheck")]
    //when a character jumps, you need a "ground check". You need to check if player is on the ground first before inputting another jump command or moving
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected bool isGrounded;

    [SerializeField]
    protected bool enableJump = false;

    protected virtual void Start()
    {
 

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }


    public virtual void OnMove(InputAction.CallbackContext ctx)
    {
        velocity = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded && enableJump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    //manipulating physics should be done in FixedUpdate
    protected virtual void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity.x * moveSpeed, rb.velocity.y);
    }

    protected virtual void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    

    }


}
