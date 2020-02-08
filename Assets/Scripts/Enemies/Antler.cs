using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antler : MonoBehaviour {
    public int moveSpeed;

    public LayerMask wallMask;

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private Vector2 direction = Vector2.left;

    private List<ContactPoint2D> contacts;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        contacts = new List<ContactPoint2D>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        collision.GetContacts(contacts);
        if (collision.collider.CompareTag("Player")) {
            Vector2 pushDirection = (.1f * Vector2.up) + direction;
            pushDirection = pushDirection.normalized;
            Vector2 pushBack = pushDirection * 2000;
            contacts[0].rigidbody.AddForce(pushBack, ForceMode2D.Impulse);
        }
        //for (int i = 0; i < collision.contactCount; i++) {
        //    if (contacts[i].collider.gameObject.CompareTag("Player")) {
        //        Debug.Log("impulse direction: " + contacts[i].normal.x + "," + contacts[i].normal.y);
        //        Debug.Log("other: " + contacts[i].rigidbody.name);
        //        contacts[i].rigidbody.AddForce(contacts[i].normal*5000*-1, ForceMode2D.Impulse);
        //        //rigidbody.AddForce(contacts[i].normalImpulse * contacts[i].normal * 100, ForceMode2D.Impulse);
        //    }
        //}
    }

    // Update is called once per frame
    void FixedUpdate() {
        float targetVelocityX = direction.x * moveSpeed;
        float forceMagnitude = rigidbody.mass * (targetVelocityX - rigidbody.velocity.x) / Time.fixedDeltaTime;
        rigidbody.AddForce(forceMagnitude * Vector2.right);

        if (Physics2D.Raycast(collider.bounds.center, direction, collider.bounds.extents.x * 1.01f, wallMask)) {
            direction = direction * -1;
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}