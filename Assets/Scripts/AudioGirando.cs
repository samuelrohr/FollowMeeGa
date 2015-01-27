using UnityEngine;
using System.Collections;

public class AudioGirando : MonoBehaviour {

	public AudioClip girando;
	private AudioSource source;
	
	void Start () {
		
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D))
		{
			
			source.Play (0);
			
		}
		
		if(Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.D))
		{
			source.Stop ();
			
		}
	}
}
