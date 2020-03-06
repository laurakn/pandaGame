using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour {
    public int timeToDeactive = 5;
    float timer;
    void OnEnable() {
        timer = Time.time;
    }
    void FixedUpdate() {
        if (Time.time - timer > timeToDeactive) {
            gameObject.SetActive(false);
        }
    }
}
