using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour {
    Player player;
    Collider2D col;
    public LayerMask groundLayer;
    public float raycastLength = .2f;
    private bool grounded;
    private float groundCheckExtentx;
    private float groundCheckExtenty;

    void Start() {
        player = transform.parent.GetComponent<Player>();
        col = GetComponent<Collider2D>();
        groundCheckExtentx = col.bounds.extents.x;
        groundCheckExtenty = col.bounds.extents.y;
    }

    void OnTriggerEnter2D(Collider2D other) {
        grounded = CheckGround();
        player.Grounded(grounded);
    }

    void OneTriggerStay2D(ColliderDistance2D other) {
        grounded = CheckGround();
        player.Grounded(grounded);
    }

    void OnTriggerExit2D(Collider2D other) {
        player.Grounded(false);
    }

    bool CheckGround() {
        Vector3 right = col.bounds.center + new Vector3 (groundCheckExtentx, groundCheckExtenty, 0);
        Vector3 left = col.bounds.center + new Vector3 (-groundCheckExtentx, groundCheckExtenty, 0);
        bool checkLeft = Physics2D.Raycast(left, Vector2.down, raycastLength, groundLayer).collider != null;  
        bool checkRight = Physics2D.Raycast(right, Vector2.down, raycastLength, groundLayer).collider != null; 

        Debug.DrawRay(right, Vector2.down*raycastLength, Color.red, 1);
        Debug.DrawRay(left, Vector2.down*raycastLength, Color.red, 1);
         
        return checkLeft || checkRight;
    }
}
