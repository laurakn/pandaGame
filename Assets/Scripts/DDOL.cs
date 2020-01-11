using System.Collections;
using UnityEngine;

public class DDOL : MonoBehaviour {
    void Awake() {
        Debug.Log("DDOLAwake");
        DontDestroyOnLoad(this.gameObject);
    }
}
