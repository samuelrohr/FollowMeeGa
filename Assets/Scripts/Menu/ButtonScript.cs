using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public GameObject parent;


	void OnMouseEnter(){
		renderer.material.color = Color.red;
	}
	
	void OnMouseExit(){
		renderer.material.color = Color.white;
	}
	
	void OnMouseDown(){
		parent.GetComponent<MenuBehaviour>().TreatClickEvent(transform.name, transform);
	}
}
