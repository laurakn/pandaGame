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



    void Start () {
      //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

      atWall = GetComponent<BoxCollider2D>().IsTouchingLayers(wallLayer);

      if (atWall) { //makes sprite turn
        turn();
      }
      float h = facingLeft ? -1 : 1;

      transform.position = new Vector2 (transform.position.x + h * speed, transform.position.y);

    }

    void turn(){
      Vector2 theScale = transform.localScale;
      theScale.x *= -1;
      transform.localScale = theScale;
      facingLeft = !facingLeft;

    }
}
