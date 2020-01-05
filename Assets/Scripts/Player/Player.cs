using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector2 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;

    [HideInInspector]
    public bool facingLeft = false;
    [HideInInspector]
    public bool grounded;

    [HideInInspector]
    public bool jumping;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        jumping = false;
    }

    void FixedUpdate()
    {
        CalculateVelocity();

        controller.Move((velocity * Time.deltaTime)); //+ (controller.collisions.residualMovement ?? Vector2.zero));

        //if (controller.collisions.residualMovement.HasValue)
        //{
        //    Debug.Log(controller.collisions.residualMovement.Value.x + "," + controller.collisions.residualMovement.Value.y);
        //}

        grounded = controller.IsGrounded();

        if (grounded && velocity.y <= 0)
        {
            velocity.y = 0;
            jumping = false;
        }
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
        if (grounded && !jumping)
        {
            velocity.y = maxJumpVelocity;
            jumping = true;
        }
    }

    public void OnJumpInputUp()
    {
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below.HasValue) ? accelerationTimeGrounded : accelerationTimeAirborne);
        if (!grounded)
            velocity.y += gravity * Time.deltaTime;
    }
}
