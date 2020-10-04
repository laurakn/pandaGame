using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer
/// </summary>
public class Parallax : MonoBehaviour
{
    public Vector2 speed = new Vector2(1, 1);
    public Vector2 direction = new Vector2(-1, 0);
    void FixedUpdate () {
        Vector3 movement = new Vector3(
        Camera.main.velocity.x * direction.x,
        Camera.main.velocity.y * direction.y,
        0);

        movement *= Time.deltaTime;
        Debug.Log(Camera.main.velocity);
        transform.Translate(movement);
    }
}
