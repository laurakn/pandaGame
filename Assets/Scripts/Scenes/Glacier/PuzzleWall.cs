using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleWall : MonoBehaviour {
    public float moveAmount;
    public float moveSpeed;

    private float destination;
    private float previousDestination;
    private float origin;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        origin = transform.position.y;
        destination = origin;
    }

    public void MoveUp() {
        destination = destination + moveAmount;
    }

    public void MoveDown() {
        destination = destination - moveAmount;
    }

    public void Reset() {
        destination = origin;
    }

    void FixedUpdate() {
        if (previousDestination != destination) {
            rb.velocity = new Vector2(0, Mathf.Sign(destination - transform.position.y)*moveSpeed);
        }

        if ((rb.velocity.y > 0 && transform.position.y >= destination) ||
            (rb.velocity.y < 0 && transform.position.y <= destination)) {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x, destination, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}