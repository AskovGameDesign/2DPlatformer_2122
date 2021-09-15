using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DMovement : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Vector2 groundCheckBox;
    public Transform groundCheckCenter;
    public Color gizmoColor;  
    
    private Rigidbody2D rb2d;
    private float moveInput;
    private bool isGrounded;
    private bool isFacingRight;
    private Animator animator;
    
    private bool isJumping = false;
    private float currentVelocity, lastVelocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapBox(groundCheckCenter.position, groundCheckBox, 0, groundLayer);

        animator.SetFloat("Walk", Mathf.Abs(moveInput));

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.velocity = Vector2.up * jumpForce;
            isJumping = true;
            animator.SetBool("Jump", isJumping);
        }

        if(moveInput < 0f && !isFacingRight)
        {
            FlipPlayer();
        }
        else if(moveInput > 0f && isFacingRight)
        {
            FlipPlayer();
        }
        currentVelocity = rb2d.velocity.y;

        if (isGrounded && isJumping && currentVelocity < lastVelocity)
        {
            isJumping = false;
            animator.SetBool("Jump", isJumping);
        }
        
        lastVelocity = rb2d.velocity.y;
    }

    void FlipPlayer()
    {
        isFacingRight = !isFacingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(moveInput * speed, rb2d.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(groundCheckCenter.position, new Vector3(groundCheckBox.x, groundCheckBox.y, 0));
    }
}
