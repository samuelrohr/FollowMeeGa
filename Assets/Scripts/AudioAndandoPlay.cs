using UnityEngine;
using System.Collections;

public class AudioAndandoPlay : MonoBehaviour {

	public AudioClip andando;
	private AudioSource source;

	void Start () {
	
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W))
		{
			source.Play (0);

		}
	
		if(Input.GetKeyUp(KeyCode.W))
		{
			source.Stop ();

	}
}
}
