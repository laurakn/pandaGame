using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{

    Player player;
    Animator animator;

    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        animator.SetBool("grounded", player.grounded);
        animator.SetBool("jumping", player.jumping);
        animator.SetBool("walking", directionalInput.x != 0);
		animator.SetFloat("vertical_velocity", player.velocity.y);
		animator.SetFloat("horizontal_velocity", player.velocity.x);

        if (directionalInput.x != 0 && (directionalInput.x < 0) != player.facingLeft)
        {
            player.turn();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }
    }
}
