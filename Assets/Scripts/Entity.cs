using System;
using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class Entity : MonoBehaviour {

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;



    [SerializeField] private LayerMask groundLayer;
    public Collider2D[] enemyColliders;

    [Header("Health")]
    [SerializeField] int maxHealth = 1;
    [SerializeField] int currentHealth;
    [SerializeField] private Material damageFeedbackMaterial;
    [SerializeField] private float damageFeedbackDuration = .2f;
    [SerializeField] private Coroutine damageFeedbackCoroutine;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius = 1f;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;


    [Header("Movement details")]
    private float xInput;
    [SerializeField] protected float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] protected float groundCheck = 1.4f;
    protected int facingDir = 1;
    protected bool isGrounded;
    protected bool facingRight = true;
    protected bool canMove = true;
    private bool canJump = true;


    public void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3.5f;
        rb.linearDamping = 0.5f; // Adjust gravity scale for better jump feel
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }



    protected virtual void Update() {

        HandleCollisions();
        HandleMovement(xInput);
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

    private IEnumerator DamageFeedbackCoroutine() {
        Material originalMaterial = sr.material;

        sr.material = damageFeedbackMaterial;

        yield return new WaitForSeconds(damageFeedbackDuration);

        sr.material = originalMaterial;
    }

    private void TakeDamage() {
        currentHealth--;

        if (damageFeedbackCoroutine != null) {
            StopCoroutine(damageFeedbackCoroutine);
        }

        StartCoroutine(DamageFeedbackCoroutine());

        if (currentHealth <= 0) Die();

    }

    protected virtual void Die() {
        anim.enabled = false;
        col.enabled = false;
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
    }

    public void EnableMovementAndJump(bool enable) {
        canMove = enable;
        canJump = enable;
    }

    protected virtual void HandleAnimation() {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    public void HandleInput() {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) TryToJump();

        if (Input.GetKeyDown(KeyCode.Mouse0)) HandleAttack(isGrounded);
    }

    protected virtual void HandleAttack(bool condition) {
        if (condition) {
            anim.SetTrigger("attack");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected virtual void HandleMovement(float xDirection) {
        if (canMove) rb.linearVelocity = new Vector2(xDirection * speed, rb.linearVelocity.y);
        else rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

    }

    protected virtual void HandleCollisions() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheck, groundLayer);
    }

    public void TryToJump() {
        if (isGrounded && canJump) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    protected void HandleFlip() {
        if (rb.linearVelocity.x > 0 && !facingRight) {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight) {
            Flip();
        }
    }

    protected void Flip() {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
        facingDir *= -1;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheck));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


}
