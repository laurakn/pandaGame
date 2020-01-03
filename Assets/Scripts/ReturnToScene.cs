using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene : MonoBehaviour {
    #if UNITY_EDITOR 
    private void Start() {
        print("ReturnToSceneStart");
        if (LoadingSceneIntegration.otherScene > 0) {
            Debug.Log("Returning again to the scene: " + LoadingSceneIntegration.otherScene);
            SceneManager.LoadScene(LoadingSceneIntegration.otherScene);
        }
    }
#endif
}
