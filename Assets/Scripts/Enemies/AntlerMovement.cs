using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AntlerMovement : MonoBehaviour {
  public float speed;
  public LayerMask wallLayer;
  public LayerMask mainPlayer;
  private bool facingLeft = true;
  private bool atWall = false;

  private Rigidbody2D rigidbody;
  private Collider2D collider;

  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
    collider = GetComponent<Collider2D>();
    //animator = GetComponent<Animator>();
  }

  void FixedUpdate() {
    float h = facingLeft ? -1 : 1;

    RaycastHit2D hit = Physics2D.Raycast(
      new Vector2(facingLeft ? collider.bounds.min.x : collider.bounds.max.x, collider.bounds.center.y),
      facingLeft ? Vector2.left : Vector2.right, speed * Time.fixedDeltaTime, wallLayer);

    atWall = hit.collider != null;
    if (atWall) { //makes sprite turn
      turn();
    }

    rigidbody.velocity = new Vector2(h * speed, rigidbody.velocity.y);
  }

  void turn() {
    Vector2 theScale = transform.localScale;
    theScale.x *= -1;
    transform.localScale = theScale;
    facingLeft = !facingLeft;
  }
}