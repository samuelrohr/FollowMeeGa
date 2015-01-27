using UnityEngine;
using System.Collections;

public class WheelMove : MonoBehaviour {
	public KeyCode right;
	public KeyCode left;
	public int angleLimit;
	public int angleStep;
	//Zero e centralizado
	private int interations;
	private Vector3 startp;

	// Use this for initialization
	void Start () {
		interations = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(left)){
			if(interations > -angleLimit){
				interations -= angleStep;
				transform.Rotate(0, angleStep, 0);
			}
		}
		else if(Input.GetKey(right)){
			if(interations < angleLimit){
				interations += angleStep;
				transform.Rotate(0, -angleStep, 0);
			}
		}
		else{
			if(interations < 0){
				interations += angleStep;
				transform.Rotate(0, -angleStep, 0);;
			}
			else if(interations > 0){
				interations -= angleStep;
				transform.Rotate(0, angleStep, 0);
			}
		}

	}
}
