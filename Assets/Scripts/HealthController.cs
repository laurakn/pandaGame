using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    Animator[] anims;
      
    [HideInInspector]
    public static int updateHealth;
    public GameObject objectToFollow;

    void Start () {
        updateHealth = GameManager.health;
        anims = GetComponentsInChildren<Animator> ();
    }

    void Update () {
        // Follow camera
        float posX = objectToFollow.transform.position.x - 30;
        float posY = objectToFollow.transform.position.y + 14;
        transform.position = new Vector2 (posX, posY);

        int health = GameManager.health;
        // Animation updates
        if (GameManager.health > updateHealth) {
            GameManager.health = updateHealth;
            foreach (Animator anim in anims) {
                anim.SetBool("damage", true);
                anim.SetInteger("health", GameManager.health);
                
            }
            print("damage");
        } 
        else if (GameManager.health < updateHealth) {
            GameManager.health = updateHealth;
            foreach (Animator anim in anims){
                anim.SetBool("healthGain", true);
                anim.SetInteger("health", GameManager.health);
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
