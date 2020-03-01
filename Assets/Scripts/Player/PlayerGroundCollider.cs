using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollider : MonoBehaviour {
    Collider2D collider;
    Player player;
    // Start is called before the first frame update
    void Start() {
        collider = GetComponent<Collider2D>();
        player = transform.parent.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ground")) {
            player.OnCollisionEnter2DGround();
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ground")) {
            player.OnCollisionExit2DGround();
            }
    }
}
