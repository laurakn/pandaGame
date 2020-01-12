using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene : MonoBehaviour {
    #if UNITY_EDITOR 
    public static bool loaded = false;
    public void Start() {
        if (LoadingSceneIntegration.otherScene > 0) {
            loaded = true;
            Debug.Log("Returning again to the scene: " + LoadingSceneIntegration.otherScene);
            SceneManager.LoadScene(LoadingSceneIntegration.otherScene);
        }
        else  SceneManager.LoadScene("MainMenu");
    }
#endif
}
