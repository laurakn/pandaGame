using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    /** Inspector variables */
    public float maxJumpHeight = 4;
    public float jumpSpeed = 4;
    public bool allowAirControl = true;
    public float moveSpeed = 6;
    public float fallVelocityScale = 1;
    public float jumpMomentumCutoff = .8f;

    /** Calculated variables */
    float maxJumpTime;
    float jumpHeightScaled;

    /** Runtime variables */
    float jumpTime;

    Vector2? directionalInput = null;

    [HideInInspector]
    public bool facingLeft = false;

    [HideInInspector]
    public bool grounded = false;

    [HideInInspector] 
    public bool atWall = false;

    [HideInInspector]
    public bool jumping = false;

    Rigidbody2D rigidbody;

    Collider2D collider;

    SpriteRenderer spriteRenderer;

    Collision2D currentCollision = null;

    List<ContactPoint2D> contactPoints;

    private bool slowingDown = false;


    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        contactPoints = new List<ContactPoint2D>();

        jumpHeightScaled = maxJumpHeight * spriteRenderer.bounds.extents.y * 2;
        maxJumpTime = jumpHeightScaled / jumpSpeed;
    }

    void FixedUpdate() {
        handleHorizontalMovement();

        handleJumpMechanics();
    }

    private void handleHorizontalMovement() {
        if (directionalInput.HasValue) {
            float targetVelocityX = directionalInput.Value.x * moveSpeed;
            setHorizontalVelocity(targetVelocityX);
        }
    }

    private void handleJumpMechanics() {
        if (jumpTime >= jumpMomentumCutoff * maxJumpTime && !slowingDown) {
            slowingDown = true;
            setVerticalVelocity(
                Mathf.Sqrt(
                    2 * rigidbody.gravityScale * Physics2D.gravity.magnitude * jumpHeightScaled * (1 - jumpMomentumCutoff)));
        }

        if (jumping) {
            if (!slowingDown) {
                setVerticalVelocity(jumpSpeed);
            }
            jumpTime += Time.fixedDeltaTime;
        } else if (hasJumpMomentum() && !slowingDown) {
            scaleVerticalVelocity(.75f);
        }

        if (jumpTime >= maxJumpTime) {
            jumping = false;
            jumpTime = 0;
        }
    }

    private void scaleVerticalVelocity(float factor) {
        setVerticalVelocity(factor * rigidbody.velocity.y);
    }

    private bool hasJumpMomentum() {
        return jumpTime < maxJumpTime && rigidbody.velocity.y > 0;
    }

    private void setHorizontalVelocity(float speed) {
        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y); 
    }

    private void setVerticalVelocity(float speed) {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed);
    }

    public void Grounded(bool b) {
        grounded = b;
    }

    public void AtWall(bool b) {
        atWall = b;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //currentCollision = collision;
        //int numContacts = collision.GetContacts(contactPoints);
        //if (numContacts > 0)
        //    Debug.Log(contactPoints[0]);
    }

    void OnCollisionExit2D(Collision2D collision) {
        currentCollision = null;
    }
    public void turn() {
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingLeft = !facingLeft;
    }

    public void SetDirectionalInput(Vector2 input) {
        if (allowAirControl || grounded)
            directionalInput = input;
        else
            directionalInput = null;
    }

    public void OnJumpInputDown() {
        if (!jumping && (grounded || atWall)) {
            jumping = true;
            jumpTime = 0f;
            slowingDown = false;
        }
    }

    public void OnJumpInputUp() {
        if (!grounded && rigidbody.velocity.y > 0) {
            jumping = false;
        }
    }
}