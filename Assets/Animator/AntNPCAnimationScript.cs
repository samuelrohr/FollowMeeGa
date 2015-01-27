using UnityEngine;
using System.Collections;

public class AntNPCAnimationScript : MonoBehaviour {

    private Animator anim;
    private SecondaryFollow followScript;
    private NPCAntJobScript jobScript;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        followScript = GetComponent<SecondaryFollow>();
        jobScript = GetComponent<NPCAntJobScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("walk", true);
        }
		if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetBool("walk",false);
        }
        if (followScript.following)
        {
            anim.SetBool("following", true);
        }
        if (!followScript.following)
        {
            anim.SetBool("following", false);
        }
        if (jobScript.isWorking)
        {
            anim.SetBool("isWorking", true);
        }
        if (!jobScript.isWorking)
        {
            anim.SetBool("isWorking", false);
        }

	}
}
