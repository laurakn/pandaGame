﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour {

    Player player;
    Collider2D collider;
    public LayerMask wallLayer;
    public float raycastLength = .2f;
    private bool atWall;
    Vector2 direction;

    void Start() {
        player = transform.parent.GetComponent<Player>();
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        direction = player.facingLeft? Vector2.left : Vector2.right;
        if (other.CompareTag ("Wall"))
            atWall = (Physics2D.Raycast(
            collider.bounds.center, direction, raycastLength, wallLayer).collider != null);
    
            Debug.DrawRay(collider.bounds.center, direction*raycastLength, Color.red, 1);
        player.AtWall(atWall);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag ("Wall"))
            player.AtWall(false);
    }
}