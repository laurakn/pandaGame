using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSnow : MonoBehaviour {

    public GameObject snowObject;
    private int count = 0;
    private Transform[] allChildren;
    void Start() { 
        allChildren = snowObject.GetComponentsInChildren<Transform>(true);
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.collider.CompareTag ("Player") && collision.contacts[0].normal == new Vector2(0, -1)) {
            Vector2 position = collision.GetContact(0).point; 
            snowPuff(position);
        }
    }
    void snowPuff (Vector2 position) {
        count  %=  allChildren.Length - 1;
        count += 1;
        allChildren[count].transform.localPosition = position;
        allChildren[count].gameObject.SetActive(true);
    }
}
