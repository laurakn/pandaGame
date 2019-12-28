using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject objectToFollow;

    private Vector3 offset;            //Private variable to store the offset distance between the player and camera

    void Start ()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        //offset = transform.position - objectToFollow.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate ()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        this.transform.position = objectToFollow.transform.position;
    }

}
