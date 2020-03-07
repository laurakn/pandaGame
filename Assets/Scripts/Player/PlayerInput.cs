using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {

	Player player;
  	PlayerAnimation anim;

	bool moving = false;

	void Start () {
		player = GetComponent<Player> ();
    	anim = GetComponent<PlayerAnimation>();
	}

	void Update () {
		if (Input.GetKey(KeyCode.Escape)) {
		        Application.Quit();
		    }

		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		player.directionalInput = directionalInput;

    	moving = directionalInput.x != 0;

		if ((directionalInput.x < 0) != player.facingLeft && moving) {
		player.turn();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			player.OnJumpInputDown ();
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			player.OnJumpInputUp ();
		}

		anim.HandleAnimations(moving, player.grounded, player.jumping);
	}
}
