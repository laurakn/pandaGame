using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLoader : MonoBehaviour {
    public GameObject healthPrefab;
    public Camera mainCamera;

    void Awake() {
        int numberOfLives = GameManager.health;
        float zStart = gameObject.transform.position.z;

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect; 

        float edgePadding = .05f * cameraHeight;
        print(edgePadding);
        print(cameraWidth);
        print(cameraHeight);
        float spriteWidth = healthPrefab.GetComponent<SpriteRenderer>().bounds.extents.x * 2;
        float xShift = (cameraWidth/3 - edgePadding - 3*spriteWidth)/2;
        float xStart = mainCamera.transform.position.x - .5f*cameraWidth + edgePadding;
        float yStart = mainCamera.transform.position.y + .5f*cameraHeight - edgePadding;

        
        //Instantiate(healthPrefab, new Vector3(xStart + mainCamera.transform.position.x, yStart + mainCamera.transform.position.y, 1), Quaternion.identity, gameObject.transform);
        IDictionary<int, GameObject> healthDict = new Dictionary<int, GameObject>();
        for (int n=0; n<numberOfLives; ++n) {
            healthDict.Add(n, healthPrefab);
            Instantiate(healthDict[n], new Vector3(xStart+xShift*n, yStart, zStart), Quaternion.identity, gameObject.transform); 
        }    
    }
}