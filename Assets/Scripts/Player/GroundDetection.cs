using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour {
    Player player;
    Collider2D collider;
    public LayerMask groundLayer;
    public float raycastLength = .2f;
    private bool grounded;
    private float groundCheckExtent;

    void Start() {
        player = transform.parent.GetComponent<Player>();
        collider = GetComponent<Collider2D>();
        groundCheckExtent = collider.bounds.extents.x;

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag ("Ground"))
            grounded = (Physics2D.Raycast(
            collider.bounds.center + new Vector3 (groundCheckExtent,0,0), Vector2.down, raycastLength, groundLayer).collider != null)
            || (Physics2D.Raycast(
            collider.bounds.center - new Vector3 (groundCheckExtent,0,0), Vector2.down, raycastLength, groundLayer).collider != null);

            Debug.DrawRay(collider.bounds.center + new Vector3 (groundCheckExtent,0,0), Vector2.down*raycastLength, Color.red, 1);
            Debug.DrawRay(collider.bounds.center - new Vector3 (groundCheckExtent,0,0), Vector2.down*raycastLength, Color.red, 1);
        
        
        player.Grounded(grounded);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag ("Ground"))
            player.Grounded(false);
    }
}
