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

    /** Calculated variables */
    float maxJumpTime;

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

    Collision2D currentCollision = null;

    List<ContactPoint2D> contactPoints;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        contactPoints = new List<ContactPoint2D>();

        float jumpHeightScaled = maxJumpHeight * collider.bounds.extents.y * 2;
        maxJumpTime = jumpHeightScaled / jumpSpeed;
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
        if (jumping) {
            setVerticalVelocity(jumpSpeed);
            jumpTime += Time.fixedDeltaTime;
        }

        if (jumpTime >= maxJumpTime) {
            jumping = false;
            jumpTime = 0;
            setVerticalVelocity(rigidbody.velocity.y/3);
        }
    }

    private void checkGround() {
        // Raycast from center of the collider downwards to check if grounded
        grounded = Physics2D.Raycast(
            collider.bounds.center, Vector2.down, collider.bounds.extents.y * 1.1f, groundLayer).collider != null;
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
        }
    }

    public void OnJumpInputUp() {
        if (!grounded && rigidbody.velocity.y > 0) {
            jumping = false;
            setVerticalVelocity(rigidbody.velocity.y/3);
        }
    }
}