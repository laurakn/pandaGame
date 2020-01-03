using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float moveSpeedIncrement = 1f;
    public float maxSpeed = 6f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    [HideInInspector]
    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    [HideInInspector]
    public bool facingLeft = false;
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool jumping;

    private Controller2D.Collisions collisions = new Controller2D.Collisions();

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        if (Time.deltaTime == 0)
        {
            return;
        }

        HandleHorizontalInput();

        velocity.y += gravity * Time.deltaTime;

        collisions = controller.Move(velocity * Time.deltaTime);

        if (collisions.bottom.HasValue)
        {
            // Check if hit ground
            grounded = collisions.bottom.Value.collider.tag.Equals("Ground");
        }
        else
        {
            grounded = false;
        }

        if (grounded && velocity.y < 0)
        {
            jumping = false;
            velocity.y = 0;
        }
    }

    public void HandleHorizontalInput()
    {
        if (directionalInput.x == 0 || !InputMatchesHeading())
        {
            velocity.x = 0;
        }
        else
        {
            velocity.x += directionalInput.x * moveSpeedIncrement;
            if (Mathf.Abs(velocity.x) > maxSpeed)
            {
                velocity.x = Mathf.Sign(velocity.x) * maxSpeed;
            }
        }
    }

    public bool InputMatchesHeading()
    {
        return directionalInput.x < 0 == facingLeft;
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
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (grounded)
        {
            jumping = true;
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp()
    {
        // if (velocity.y < minJumpVelocity)
        // {
        //     velocity.y = minJumpVelocity;
        // }
        // else if (velocity.y > maxJumpVelocity)
        // {
        //     velocity.y = maxJumpVelocity;
        // }
    }
}
