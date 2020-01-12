using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public LayerMask groundLayer;

    public float maxJumpHeight = 4;
    public float timeToJumpApex = .4f;

    public float fallingGravityMultiplier = 4;

    public bool allowAirControl = true;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    float maxJumpVelocity;
    float velocityXSmoothing;

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

    void Start()
    {
        maxJumpVelocity = maxJumpHeight / timeToJumpApex;

        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        contactPoints = new List<ContactPoint2D>();
    }

    void FixedUpdate()
    {
        if (directionalInput.HasValue)
        {
            float targetVelocityX = directionalInput.Value.x * moveSpeed;
            float forceMagnitude = rigidbody.mass * (targetVelocityX - rigidbody.velocity.x) / Time.fixedDeltaTime;
            rigidbody.AddForce(forceMagnitude * Vector2.right);
        }

        if (jumping)
        {
            float jumpAcceleration = (maxJumpVelocity - rigidbody.velocity.y) / Time.fixedDeltaTime;
            rigidbody.AddForce(jumpAcceleration * rigidbody.mass * Vector2.up);

            jumpTime += Time.fixedDeltaTime;
        }

        if (jumpTime >= timeToJumpApex)
        {
            jumping = false;
        }

        // Additional acceleration for falling for a snappier feel
        if (!grounded && rigidbody.velocity.y <= 0)
        {
            rigidbody.AddForce(rigidbody.mass * Physics2D.gravity * fallingGravityMultiplier);
        }

        // Raycast from center of the collider downwards to check if grounded
        grounded = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y * 1.1f, groundLayer).collider != null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //currentCollision = collision;
        //int numContacts = collision.GetContacts(contactPoints);
        //if (numContacts > 0)
        //    Debug.Log(contactPoints[0]);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentCollision = null;
    }

    void Update()
    {
    }

    public void turn()
    {
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingLeft = !facingLeft;
    }

    public void SetDirectionalInput(Vector2 input)
    {
        if (allowAirControl || grounded)
            directionalInput = input;
        else
            directionalInput = null;
    }

    public void OnJumpInputDown()
    {
        if (!jumping && grounded)
        {
            jumping = true;
            jumpTime = 0f;
        }
    }

    public void OnJumpInputUp()
    {
        jumping = false;
    }
}