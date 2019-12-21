using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed;
    public Vector2 jumpHeight;
    public LayerMask groundLayer;
    private bool facingLeft = false;

    Animator animator;

    void Start () {
      animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

      float h = Input.GetAxisRaw("Horizontal");
      float v = Input.GetAxisRaw("Vertical");
      bool j = Input.GetKeyDown(KeyCode.Space);

      animator.SetBool("idle", h==0);
      animator.SetBool("jumping", !isGrounded());

      gameObject.transform.position = new Vector2 (transform.position.x + h * speed, transform.position.y);

      if (j) { //makes player jump
        jump();
      }

      if (h<0 != facingLeft && h != 0) { //makes player faceLeft when moving left
        faceLeft();
      }
    }

    void faceLeft(){
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        facingLeft = !facingLeft;
    }

    void jump(){
      if (!isGrounded()) {return;}
      else {
          GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
      }
    }

    bool isGrounded() {
      Vector2 position = transform.position;
      Vector2 direction = Vector2.down;
      float distance = 2.0f;

      RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

      if (hit.collider != null) {
          return true;
      }
      return false;
    }
}
