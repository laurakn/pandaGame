using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
    public int numWalls;
    public List<PuzzleWall> puzzleWalls = new List<PuzzleWall>();
    public List<bool> moveDown = new List<bool>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            for (int i = 0; i < numWalls; i++) {
                puzzleWalls[i].Reset();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) {
            return;
        }

        for (int i = 0; i < numWalls; i++) {
            if (moveDown[i]) {
                puzzleWalls[i].MoveDown();
            } else {
                puzzleWalls[i].MoveUp();
            }
        }
    }
}