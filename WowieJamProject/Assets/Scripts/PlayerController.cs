using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Moving")]
    Rigidbody2D rb2d;
    Vector2 input;
    [SerializeField] int Speed;

    [Header("GroundCheck")]
    [SerializeField] Transform feet;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float radius;
    [SerializeField] bool PlayerIsGrounded;

    [Header("Jumping")]
    [SerializeField] float CoyoteTime;
    [SerializeField] int JumpForce;
    [SerializeField] float JumpCooldown;
    bool canJump = true;
    public bool PlayerIsJumping;

    [Header("Animation")]
    SpriteRenderer spriteRenderer;
    Animator animator;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void ChangeDirection(InputAction.CallbackContext _input)
    {
        input = _input.ReadValue<Vector2>();

        animator.SetBool("Walk", input.x != 0);
        spriteRenderer.flipX = input.x < 0;

    }

    public void Jump(InputAction.CallbackContext _input)
    {
        if (!_input.canceled)
            PlayerIsJumping = true;
        else if (_input.canceled)
            PlayerIsJumping = false;
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void PerformJump()
    {
        rb2d.AddForce(JumpForce * transform.up);
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(input.x * Speed, rb2d.velocity.y);
        if (PlayerIsJumping && canJump && PlayerIsGrounded)
        {
            canJump = false;
            Invoke("ResetJump", JumpCooldown);
            PerformJump();
        }

        GroundCheck();
    }

    private void GroundCheck()
    {
        bool grounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
        animator.SetBool("Jump", !grounded);

        if (grounded)
            PlayerIsGrounded = true;
        else
            Invoke("ResetGroundCheck", CoyoteTime);
    }

    void ResetGroundCheck()
    {
        PlayerIsGrounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
    }
}
