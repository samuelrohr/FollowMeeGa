using UnityEngine;
using System.Collections;

public class AudioAndandoPlay : MonoBehaviour {

	public AudioClip andando;
	private AudioSource source;
    private PlayerAntMovement antMoveScript;
    private bool keyUp = true;
    private bool playing = false;

	void Start () {
	
		source = GetComponent<AudioSource>();
        antMoveScript = GetComponent<PlayerAntMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.W))
            keyUp = false;


        if(!keyUp && antMoveScript.canMove && !playing)
        {   
            playing = true;
            source.Play (0);           
        }
        if(!antMoveScript.canMove)
        {
            playing = false;
            source.Stop();
        }
        if(Input.GetKeyUp(KeyCode.W)|| Input.GetKeyUp(KeyCode.UpArrow))
        {
            keyUp = true;
            source.Stop();
        }

    }
}
