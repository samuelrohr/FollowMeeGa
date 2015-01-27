using UnityEngine;
using System.Collections;

public class OrtograficCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().enabled = false;
        GetComponent<Camera>().enabled = true;
	}

}
