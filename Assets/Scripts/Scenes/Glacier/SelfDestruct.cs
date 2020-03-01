using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {
    public int timeToDestruction = 5;

    
    void Awake() {
        Destroy(gameObject, timeToDestruction);
    }
}
