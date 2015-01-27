using UnityEngine;
using System.Collections;

public class BoxInfo : MonoBehaviour {
	private int boxType;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -100)
			Destroy (this);
	}

	void OnCollisionEnter(Collision collision){
		foreach (ContactPoint contact in collision.contacts) {
			// Checamos se a colisao foi com uma caixa
			if (contact.otherCollider.name.Equals ("bottom")) {
				Destroy (this);
			}
		}
	}

	public void setBoxType(int type){
		boxType = type;
	}

	public int getBoxType(){
		return boxType;
	}
}
