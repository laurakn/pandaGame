using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    Animator anim;
    // Start is called before the first frame update
    void Start() {
    	anim = GetComponent<Animator>();
    }

    public void HandleAnimations(bool moving, bool grounded, bool jumping) {
        if (moving && grounded)
            Run();

        else if (jumping)
            Jump();

        else if (!moving && grounded)
            Idle();
    }

    void Idle() {
        anim.Play("idle", 0);
    }

    void Run() {
        anim.Play("run", 0);
    }

    void Jump() {
        anim.Play("run", 0);
    }
}
