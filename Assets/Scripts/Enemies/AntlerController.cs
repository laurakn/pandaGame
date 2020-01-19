using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AntlerController : EnemyBaseClass {
    
  public float distancePerSecond;
  public LayerMask wallLayer;
  private Rigidbody2D rigidbody;

  void Start () {
    //animator = GetComponent<Animator>();
    facingLeft = true;
    mainPlayer = GameObject.FindWithTag("Player");
    playerSpeed = mainPlayer.GetComponent<Player>().moveSpeed;
    rigidbody = GetComponent<Rigidbody2D>();
    engage = false;
    checkRaycastSight = false;
  }

  // Update is called once per frame
  void FixedUpdate() {
    if (!engage) move();
    // check if close to mainPlayer
    playerDistance();
    // if player close, check if within sight
    if (checkRaycastSight) raycastSight();
  }

  void OnCollisionEnter2D(Collision2D collision) {
    print("collision");
    if (collision.collider.tag == "Wall") {
        turn();
        print("wall");
    }
  }

  public void move() {
    float h = facingLeft ? -1 : 1;

    float targetVelocityX = h * distancePerSecond;
    float forceMagnitude = rigidbody.mass * (targetVelocityX - rigidbody.velocity.x) / Time.fixedDeltaTime;
    rigidbody.AddForce(forceMagnitude * Vector2.right);
    //transform.position = new Vector2 (transform.position.x + h*distancePerSecond*Time.deltaTime, transform.position.y);
  }
  void turn() {
      Vector2 theScale = transform.localScale;
      theScale.x *= -1;
      transform.localScale = theScale;
      facingLeft = !facingLeft;
  }
}
