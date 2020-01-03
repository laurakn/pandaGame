using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    Animator[] anims;
      
    [HideInInspector]
    private int health;
    public static int updateHealth;
    public GameObject objectToFollow;

    void Start () {
        health = 3;
        updateHealth = 3;
        anims = GetComponentsInChildren<Animator> ();
    }

    void Update () {
        // Follow camera
        float posX = objectToFollow.transform.position.x - 7;
        float posY = objectToFollow.transform.position.y + 3;
        transform.position = new Vector2 (posX, posY);

        // Animation updates
        if (health > updateHealth) {
            health = updateHealth;
            foreach (Animator anim in anims) {
                anim.SetBool("damage", true);
                anim.SetInteger("health", health);
                
            }
            print("damage");
        } 
        else if (health < updateHealth) {
            health = updateHealth;
            foreach (Animator anim in anims){
                anim.SetBool("healthGain", true);
                anim.SetInteger("health", health);
            }
            print("health");
        }
        else {
            foreach (Animator anim in anims){
                anim.SetBool("healthGain", false);
                anim.SetBool("damage", false);
            }
        }
    }
}
