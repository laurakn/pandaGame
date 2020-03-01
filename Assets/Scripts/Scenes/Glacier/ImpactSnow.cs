using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSnow : MonoBehaviour {

    public GameObject snowObject;
    // Start is called before the first frame update
    void Start() { 
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.collider.CompareTag ("Player") && collision.contacts[0].normal == new Vector2(0, -1)) {
            Vector2 position = collision.GetContact(0).point; 
            snowPuff(position);
        }
    }
    void snowPuff (Vector2 position) {
    GameObject clone = Instantiate(snowObject, new Vector3(position.x, position.y, 1), Quaternion.identity);
    clone.SetActive(true);
    }
}
