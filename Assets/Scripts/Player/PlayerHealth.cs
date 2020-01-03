using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHealth : MonoBehaviour
{
    HealthController HealthController;
    void Start() {

    }

    void Update () {

    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            HealthController.updateHealth -= 1;
            //Debug.Log("collision");
            } 
    }
}
