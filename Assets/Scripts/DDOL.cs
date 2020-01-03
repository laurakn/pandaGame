using System.Collections;
using UnityEngine;

public class DDOL : MonoBehaviour {
    void Awake() {
        print("DDOLAwake");
        DontDestroyOnLoad(this.gameObject);
    }
}
