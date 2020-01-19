using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseClass : MonoBehaviour {

  public float triggerRaycastDistance;
  public LayerMask playerLayer; 
  public float rayLength = 1f;

  [HideInInspector]
  public bool facingLeft; 
  [HideInInspector]
  public bool checkRaycastSight;
  [HideInInspector]
  public bool engage;
  [HideInInspector]
  public GameObject mainPlayer;
  [HideInInspector]
  public float distanceCheckWait = 1.0f, distanceTime = 0.0f;
  [HideInInspector]
  public float playerSpeed;
  

  // Only check raycast every quater second
  private float raycastCheckWait = .25f, raycastTime = 0.0f;
  private int nRays = 5;
  

  // Start is called before the first frame update
  public void playerDistance() {
    distanceTime += Time.deltaTime;
    // wait to check only when possibly close eough to see.
    if (distanceTime > distanceCheckWait) {
      // reset timer
      distanceTime = 0;
      float distance = Vector2.Distance(transform.position, mainPlayer.transform.position) - triggerRaycastDistance;
      // if far away, update wait time for next check
      if (distance > 0) {
        distanceCheckWait = distance / playerSpeed;
        // if very far away, enemy now ignores player if they were engaged.
        if (distance > 100) checkRaycastSight = false;
      }
      // if close to player, check if enemy can "see" player.
      else {
        checkRaycastSight = true;
        distanceCheckWait = 1;
      }
    }
  }

  public void raycastSight(){
    raycastTime += Time.deltaTime;
    if (raycastTime > raycastCheckWait){
      float h = facingLeft ? 1 : -1;
      // arc of nRays from 3pi/4 to 5pi/4, since facing left is h=1.
      for (int i = 0; i < nRays; i++) {
        float x = Mathf.Cos(3*Mathf.PI/4 + i*Mathf.PI/2/nRays);
        float y = Mathf.Sin(3*Mathf.PI/4 + i*Mathf.PI/2/nRays);
        RaycastHit2D seePlayer = Physics2D.Raycast(transform.position, new Vector2(x, y)*h, rayLength, playerLayer);
        Debug.DrawRay(transform.position, new Vector2(x, y)*h*rayLength, Color.magenta);

        if (seePlayer) {
          engage = true;
        }
      }
    }
  }
}
