using System;
using UnityEngine;

public class Enemy : Entity {

    private bool playerDetected;

    protected override void Update() {

        HandleCollisions();
        HandleAnimation();
        HandleFlip();
        HandleMovement(facingDir);
        HandleAttack(playerDetected);

    }

    protected override void HandleCollisions() {
        base.HandleCollisions();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }

    protected override void HandleAnimation() {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }

}
