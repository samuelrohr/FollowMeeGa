using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeetleBehavior : MonoBehaviour {
	public float minZ, maxZ;
	public float minX, maxX;
	public float moveSpeed;
	public float turnSpeed;
	public float difficulty;
	public bool walking = true;
	int countRotate = 0;
	float frozenTime = 0;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (walking) {
			transform.Translate (Vector3.up * moveSpeed * Time.deltaTime * (difficulty/10.0f));
			if (countRotate > 30) {
				transform.Rotate(Vector3.forward, Random.Range(-2,3) * turnSpeed * Time.deltaTime);
			}
			rigidbody.position = new Vector3 
			(
				Mathf.Clamp (rigidbody.position.x, minX, maxX), 
				0.45f, 
				Mathf.Clamp (rigidbody.position.z, minZ, maxZ)
			);
			countRotate++;
		} else {
			frozenTime -= Time.deltaTime;
			if (frozenTime <= 0) {
				walking = true;
				frozenTime = 0;
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals(ColliderTags.antNPCTag) && walking) {
			QuantdFormigas.RemoveAnt(other.gameObject);
			Destroy(other.gameObject);
		}
	}


	public void Freeze(float freezeTime) {
		walking = false;
		frozenTime += freezeTime;
	}
}
