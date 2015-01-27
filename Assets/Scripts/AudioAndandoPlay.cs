using UnityEngine;
using System.Collections;

public class AudioAndandoPlay : MonoBehaviour {

	public AudioClip andando;
	private AudioSource source;
    private PlayerAntMovement antMoveScript;

	void Start () {
	
		source = GetComponent<AudioSource>();
        antMoveScript = GetComponent<PlayerAntMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && antMoveScript.canMove)
		{
			source.Play (0);

		}
        else{
			source.Stop ();
        }
    }
}
