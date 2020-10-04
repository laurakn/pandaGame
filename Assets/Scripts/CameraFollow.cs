using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject objectToFollow;
    public float smoothTimeX;
    public float smoothTimeY;
    public bool bounds;
    public Vector2 minCameraPos;
    public Vector2 maxCameraPos;

    private Vector2 velocity;

    // LateUpdate is called after Update each frame
    //void LateUpdate ()  {
    //    // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
    //    this.transform.position = objectToFollow.transform.position;
    //}

    void FixedUpdate () {
        float posX = Mathf.SmoothDamp(transform.position.x, objectToFollow.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, objectToFollow.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3 (posX, posY, transform.position.z);

        if (bounds) {
          transform.position = new Vector2 (Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x), Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y));
        }
    }
}
