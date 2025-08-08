using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class Entity : MonoBehaviour {

    protected Animator anim;
    protected Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    public Collider2D[] enemyColliders;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius = 1f;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;


    private float xInput;
    [SerializeField] protected float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] protected float groundCheck = 1.4f;


    protected bool isGrounded;
    protected bool facingRight = true;
    protected bool canMove = true;
    protected bool canJump = true;


    public void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3.5f;
        rb.linearDamping = 0.5f; // Adjust gravity scale for better jump feel
        anim = GetComponentInChildren<Animator>();
    }



    protected virtual void Update() {
        HandleCollisions();
        HandleMovement();
        HandleInput();
        HandleAnimation();
        HandleFlip();

    }

    public void DamageTargets() {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider2D enemy in enemyColliders) {
            enemy.GetComponent<Entity>()?.TakeDamage();

        }
    }

    private void TakeDamage() {
        throw new NotImplementedException();
    }

    public void EnableMovementAndJump(bool enable) {
        canMove = enable;
        canJump = enable;
    }

    protected void HandleAnimation() {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    public void HandleInput() {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) TryToJump();

        if (Input.GetKeyDown(KeyCode.Mouse0)) TryToAttack();
    }

    protected virtual void TryToAttack() {
        if (isGrounded) {
            anim.SetTrigger("attack");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected virtual void HandleMovement() {
        if (canMove) rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);

    }

    protected virtual void HandleCollisions() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheck, groundLayer);
    }

    public void TryToJump() {
        if (isGrounded && canJump) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleFlip() {
        if (rb.linearVelocity.x > 0 && !facingRight) {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight) {
            Flip();
        }
    }

    private void Flip() {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheck));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


}
