using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public LayerMask groundLayer;

    public float maxJumpHeight = 4;
    public float jumpSpeed = 4;
    float timeToJumpApex;

    public float fallingGravityMultiplier = 4;

    public bool allowAirControl = true;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    float velocityXSmoothing;

    float jumpTime;
    float maxJumpTime;

    float jumpForce;

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

        float gravityAcceleration = Physics2D.gravity.magnitude * rigidbody.gravityScale;
        float jumpHeightScaled = maxJumpHeight * collider.bounds.extents.y * 2;

        maxJumpTime = jumpHeightScaled / jumpSpeed;

        // Based on the set jump height and jump time the force can be calculated:
        jumpForce = ((rigidbody.mass * gravityAcceleration) / 2) * (
            1 + Mathf.Sqrt(1 + ((8 * jumpHeightScaled) / (gravityAcceleration * maxJumpTime * maxJumpTime))));
    }

    void FixedUpdate() {
        if (directionalInput.HasValue) {
            float targetVelocityX = directionalInput.Value.x * moveSpeed;
            float forceMagnitude = rigidbody.mass * (targetVelocityX - rigidbody.velocity.x) / Time.fixedDeltaTime;
            forceMagnitude = Mathf.Min(Mathf.Abs(forceMagnitude), rigidbody.mass * moveSpeed / Time.fixedDeltaTime) * Mathf.Sign(forceMagnitude);
            rigidbody.AddForce(forceMagnitude * Vector2.right);
        }

        if (jumping) {
            if (jumpTime < maxJumpTime - Mathf.Abs(jumpSpeed/Physics2D.gravity.magnitude)) {
              rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
            }
            //rigidbody.addforce(jumpforce * vector2.up);
            jumpTime += Time.fixedDeltaTime;
        }

        if (jumpTime >= maxJumpTime) {
            jumping = false;
            //rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            jumpTime = 0;
        }

        // Additional acceleration for falling for a snappier feel
        //if (!grounded && rigidbody.velocity.y <= 0) {
        //    rigidbody.AddForce(rigidbody.mass * Physics2D.gravity * fallingGravityMultiplier);
        //}

        // Raycast from center of the collider downwards to check if grounded
        grounded = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y * 1.1f, groundLayer).collider != null;

        //if (!grounded && !jumping) {
        //    rigidbody.velocity = new Vector2(rigidbody.velocity.x, -2 * jumpSpeed);
        //}
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
          rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y/3);
        }
    }
}