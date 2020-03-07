using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    GameObject[] healthLanterns;

    Animator[] animList;
    public GameObject objectToFollow;
      
    [HideInInspector]
    public static int updateHealth;

    void Start () {
        updateHealth = GameManager.health;
        animList = GetComponentsInChildren<Animator> ();
        for (int i = 0; i < animList.Length; i++){
            animList[i].enabled = false;
        }
    }

    void Update () {
        
        followCamera();

        int health = GameManager.health;
        // Animation updates
        if (GameManager.health > updateHealth) {
            takeDamage();
            GameManager.health = updateHealth;
            print("damage");
        } 
        else if (GameManager.health < updateHealth) {
            gainHealth();
            GameManager.health = updateHealth;
            print("health");
        }
    }

    void takeDamage() {
        animList[updateHealth].enabled = true;
        print(updateHealth);
    }

    void gainHealth() {

    }

    void followCamera() {
        float posX = objectToFollow.transform.position.x;
        float posY = objectToFollow.transform.position.y;
        transform.position = new Vector2 (posX, posY);
    }
}
