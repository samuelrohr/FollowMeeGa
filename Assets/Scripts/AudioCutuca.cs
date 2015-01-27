using UnityEngine;
using System.Collections;

public class AudioCutuca : MonoBehaviour {

	public AudioClip cutuca;
	private AudioSource source;
	
	void Start () {
		
		source = GetComponent<AudioSource>();
	}
	

	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
		{
			
			source.Play (0);
			
		}
		
		if(Input.GetKeyUp(KeyCode.E))
		{
			source.Stop ();
			
		}
	}
}
