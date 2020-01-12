using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHealth : MonoBehaviour
{
    private float timer = 0.0f;
    public float recoveryTime = 2.0f;
    HealthController HealthController;

    void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag == "Enemy") {
            timer += Time.deltaTime;
            if (timer > recoveryTime) {
                timer = 0.0f;
            HealthController.updateHealth -= 1;
            //Debug.Log("collision");
            } 
        }
    }
}
