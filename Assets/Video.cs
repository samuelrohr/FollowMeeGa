using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class Video : MonoBehaviour {

	public MovieTexture movie;
	float duracaovideo = 75f;

	void Start () {
		renderer.material.mainTexture = movie as MovieTexture;
		movie.Play ();

	
	}
	

	void Update () {
		if (Time.time >= duracaovideo ){
			Application.LoadLevel ("SRScene+");


	
	}
}
}
