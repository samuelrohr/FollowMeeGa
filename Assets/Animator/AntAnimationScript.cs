using UnityEngine;
using System.Collections;

public class AntAnimationScript : MonoBehaviour {



    private Animator anim;
	private PlayerAntMovement antMoveScript;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
		antMoveScript = GetComponent<PlayerAntMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("canMove", antMoveScript.canMove);

		if (((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))) && antMoveScript.canMove)
        {
            anim.SetBool("walk", true);

        } else
        {
            anim.SetBool("walk", false);
        }
		if (Input.GetKeyUp(KeyCode.W)|| Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetBool("walk",false);
		
        }
		if(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetBool("left", true);
	
        }
		if (Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetBool("left",false);

        }
		if(Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetBool("right", true);

        }
		if (Input.GetKeyUp(KeyCode.D)|| Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetBool("right",false);

        }

	}
}
