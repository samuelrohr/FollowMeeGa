using UnityEngine;
using System.Collections;

public class CarRotationSelection : MonoBehaviour {
	private float lastMouseX;

	void OnMouseDown(){
		lastMouseX = Input.mousePosition.x;
	}

	void OnMouseDrag(){
		float delta = Input.mousePosition.x - lastMouseX;
		transform.eulerAngles = new Vector3(0, delta, 0);
	}
}
