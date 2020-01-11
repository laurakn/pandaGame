using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int health = 3;
    // Start is called before the first frame update
    void Awake() {
        Debug.Log("GameManagerAwake");
    }
    void Start() {
        Debug.Log("GameManagerStart");
    }

    void Update(){
        if (health == 0){
            health = 3;
            SceneManager.LoadScene("MainMenu");

        }
    }
}
