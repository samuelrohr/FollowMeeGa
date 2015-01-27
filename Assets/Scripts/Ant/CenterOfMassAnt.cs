using UnityEngine;
using System.Collections;

public class CenterOfMassAnt : MonoBehaviour {
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0, -10, 0);
	}
}
