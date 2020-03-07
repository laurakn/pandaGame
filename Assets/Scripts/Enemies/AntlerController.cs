using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(CharacterController))]
public class AntlerController : EnemyBaseClass {
    
  public float distancePerSecond;
  public LayerMask wallLayer;
  private Rigidbody2D rb;
  private float attackSpeed;

  void Start () {
    //animator = GetComponent<Animator>();
    facingLeft = 1; // 1 true, -1 false
    mainPlayer = GameObject.FindWithTag("Player");
    playerSpeed = mainPlayer.GetComponent<Player>().moveSpeed;
    rb = GetComponent<Rigidbody2D>();
    engage = false;
    checkRaycastSight = false;
    rayLength = triggerRaycastDistance;
    attackSpeed = 2*distancePerSecond;

  }

  // Update is called once per frame
  void FixedUpdate() {
    if (!engage) move();
    // check if close to mainPlayer
    playerDistance();
    // if player close, check if within sight
    if (checkRaycastSight) {
      float hitDistance = raycastSight();
      if (hitDistance < rayLength) {
        engage = false;
        attack(hitDistance);
      }
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    print("collision");
    if (collision.collider.tag == "Wall") {
        turn();
        print("wall");
    }
  }

  public void move() {
    // movement when not engaged with player
    //float targetVelocityX = -facingLeft * distancePerSecond;
    //float forceMagnitude = rb.mass * (targetVelocityX - rb.velocity.x) / Time.fixedDeltaTime;
    //rb.AddForce(forceMagnitude * Vector2.right);
    //transform.position = new Vector2 (transform.position.x + facingLeft*distancePerSecond*Time.deltaTime, transform.position.y);
    rb.MovePosition(transform.position - facingLeft*Vector3.right*distancePerSecond * Time.fixedDeltaTime);
  }
  void turn() {
      facingLeft *= -1;
      Vector2 theScale = transform.localScale;
      theScale.x *= -1;
      transform.localScale = theScale;
      //transform.eulerAngles = new Vector3(0, 0, 30*facingLeft);
  }

  void attack(float distance) {
    // attack animation
    distancePerSecond = attackSpeed;
  }

  void chase() {

  }
}
