using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Math;

public class Player : MonoBehaviour {
    /** Inspector variables */
    public float maxJumpHeight = 4;
    public float jumpSpeed = 4;
    public float moveSpeed = 6;
    public float fallVelocityScale = 1;
    public float jumpMomentumCutoff = .8f;

    /** Calculated variables */
    float maxJumpTime;
    float jumpHeightScaled;

    /** Runtime variables */
    float jumpTime;

    [HideInInspector]
    public Vector2? directionalInput = null;

    [HideInInspector]
    public bool facingLeft = false;

    [HideInInspector]
    public bool grounded = false;

    [HideInInspector] 
    public bool atWall = false;

    [HideInInspector]
    public bool jumping = false;

    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;

    private bool slowingDown = false;

    GroundDetection groundDetection;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        groundDetection = GetComponentInChildren<GroundDetection>();

        jumpHeightScaled = maxJumpHeight * spriteRenderer.bounds.extents.y * 2;
        maxJumpTime = jumpHeightScaled / jumpSpeed;

        rb.gravityScale = fallVelocityScale * fallVelocityScale * jumpSpeed * jumpSpeed / (Physics2D.gravity.magnitude * jumpHeightScaled);

        Debug.Log(spriteRenderer.bounds.extents.y * 2);
    }

    void FixedUpdate() {
        handleHorizontalMovement();

        handleJumpMechanics();

        if (!grounded && Mathf.Abs(rb.velocity.y) < 0.1) {
            groundDetection.CheckGrounded();
        }
    }

    private void handleHorizontalMovement() {
        if (directionalInput.HasValue) {
            float targetVelocityX = directionalInput.Value.x * moveSpeed;
            setHorizontalVelocity(targetVelocityX);
        }
    }

    private void handleJumpMechanics() {
        if (jumpTime >= jumpMomentumCutoff * maxJumpTime && !slowingDown) {
            Debug.DrawRay(spriteRenderer.bounds.min, Vector2.left * 20, Color.green, maxJumpTime * 2);
            slowingDown = true;
            setVerticalVelocity(
                Mathf.Sqrt(
                    2 * rb.gravityScale * Physics2D.gravity.magnitude * jumpHeightScaled * (1 - jumpMomentumCutoff)));

            jumping = false;
            jumpTime = 0;
        }

        if (jumping) {
            if (!slowingDown) {
                setVerticalVelocity(jumpSpeed);
            }
            jumpTime += Time.fixedDeltaTime;
        } else if (hasJumpMomentum() && !slowingDown) {
            scaleVerticalVelocity(.75f);
        }

        //if (jumpTime >= maxJumpTime) {
        //    jumping = false;
        //    jumpTime = 0;
        //}
    }

    private void scaleVerticalVelocity(float factor) {
        setVerticalVelocity(factor * rb.velocity.y);
    }

    private bool hasJumpMomentum() {
        return jumpTime < maxJumpTime && rb.velocity.y > 0;
    }

    private void setHorizontalVelocity(float speed) {
        rb.velocity = new Vector2(speed, rb.velocity.y); 
    }

    private void setVerticalVelocity(float speed) {
        rb.velocity = new Vector2(rb.velocity.x, speed);
    }

    public void AtWall(bool b) {
        atWall = b;
    }

    public void turn() {
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingLeft = !facingLeft;
    }

    public void OnJumpInputDown() {
        if (!jumping && (grounded || atWall)) {
            jumping = true;
            jumpTime = 0f;
            slowingDown = false;
            grounded = false;

            Debug.DrawRay(spriteRenderer.bounds.min, Vector2.left * 20, Color.red, maxJumpTime * 2);
            Debug.DrawRay(spriteRenderer.bounds.min + new Vector3(0, jumpHeightScaled, 0), Vector2.left * 20, Color.red, maxJumpTime * 2);
        }
    }

    public void OnJumpInputUp() {
        if (!grounded && rb.velocity.y > 0) {
            jumping = false;
        }
    }
}