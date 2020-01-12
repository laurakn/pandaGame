using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLoader : MonoBehaviour {
    public GameObject healthPrefab;
    public GameObject mainCamera;
    
    public int xShift = 5;
    public int xStart = 39;
    public int yStart = 20;

    void Awake() {
        int numberOfLives = GameManager.health;
        //Instantiate(healthPrefab, new Vector3(xStart + mainCamera.transform.position.x, yStart + mainCamera.transform.position.y, 1), Quaternion.identity, gameObject.transform);
        IDictionary<int, GameObject> healthDict = new Dictionary<int, GameObject>();
        for (int n=0; n<numberOfLives; ++n) {
            healthDict.Add(n, healthPrefab);
            Instantiate(healthDict[n], new Vector3(xStart+xShift*n + mainCamera.transform.position.x, yStart+mainCamera.transform.position.y, 0), Quaternion.identity, gameObject.transform); 
        }       
    }
}