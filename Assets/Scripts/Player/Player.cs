using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    /** Inspector variables */
    public LayerMask groundLayer;
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

        rigidbody.gravityScale = fallVelocityScale * fallVelocityScale * jumpSpeed * jumpSpeed / (Physics2D.gravity.magnitude * jumpHeightScaled);

        Debug.Log(spriteRenderer.bounds.extents.y * 2);
    }

    void FixedUpdate() {
        handleHorizontalMovement();

        handleJumpMechanics();

        checkGround();
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

    private void checkGround() {
        // Raycast from center of the collider downwards to check if grounded
        grounded = Physics2D.Raycast(
            collider.bounds.center, Vector2.down, collider.bounds.extents.y * 1.2f, groundLayer).collider != null;
    }

    private void setHorizontalVelocity(float speed) {
        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    }

    private void setVerticalVelocity(float speed) {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed);
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

    void Update() { }

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
        if (!jumping && grounded) {
            jumping = true;
            jumpTime = 0f;
            slowingDown = false;

            Debug.DrawRay(spriteRenderer.bounds.min, Vector2.left * 20, Color.red, maxJumpTime * 2);
            Debug.DrawRay(spriteRenderer.bounds.min + new Vector3(0, jumpHeightScaled, 0), Vector2.left * 20, Color.red, maxJumpTime * 2);
        }
    }

    public void OnJumpInputUp() {
        if (!grounded && rigidbody.velocity.y > 0) {
            jumping = false;
        }
    }
}