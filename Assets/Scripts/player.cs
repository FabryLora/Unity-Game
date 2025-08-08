using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    private string playerName = "Player 1";
    private int age = 22;
    [SerializeField] private float speed = 5f;
    private float xInput;

    [SerializeField] private float jumpForce = 7f;

    [SerializeField] private bool facingRight = true;


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2f; // Adjust gravity scale for better jump feel
        anim = GetComponentInChildren<Animator>();
    }

    public void Start()
    {

    }

    public void Update()
    {
        HandleMovement();
        HandleInput();
        HandleAnimation();
        HandleFlip();

    }

    private void HandleAnimation()
    {
        bool isWalking = rb.linearVelocity.x != 0;
        anim.SetBool("isWalking", isWalking);
    }

    public void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) HandleJump();
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
    }

    public void HandleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }


}
