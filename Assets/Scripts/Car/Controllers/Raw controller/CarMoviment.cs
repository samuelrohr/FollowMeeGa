using UnityEngine;
using System.Collections;

public class CarMoviment : MonoBehaviour{
	public float MaxEnginePower;
	private float enginePower;
	private Vector3 wheelDirection;
	private Vector3 carDirection;
	public KeyCode forward;
	public KeyCode backward;
	public KeyCode brake;
	public KeyCode right;
	public KeyCode left;
	public int angleLimit;
	public int angleStep;
	//Zero e centralizado
	private int interations;
	private Vector3 startp;
	public float acceleration;
	public float friction;
	public float brakePower;
	private bool isOnTheGround;
	
	
	// Use this for initialization
	void Start () {
		Debug.DrawLine(new Vector3(0,0,0), carDirection, Color.green, 1);
		carDirection.Normalize();
		
		enginePower = 0;
		interations = 0;
		wheelDirection = new Vector3(1, 0, 0);
		carDirection = wheelDirection;
		startp = new Vector3(0, 0.2f, 0);
		Debug.DrawLine(startp, wheelDirection, Color.yellow, 1);

		isOnTheGround = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(left) || Input.GetKey(right)){
			if(Input.GetKey(left)){
				if(interations > -angleLimit){
					interations -= angleStep;
					wheelDirection = Quaternion.AngleAxis(-angleStep, Vector3.up) * wheelDirection;
				}
			}
			else{
				if(interations < angleLimit){
					interations += angleStep;
					wheelDirection = Quaternion.AngleAxis(angleStep, Vector3.up) * wheelDirection;
				}
			}
			if (enginePower > 0){
				float angle = Utilities.getAngle(carDirection, wheelDirection);
				if(Input.GetKey(left))
					angle = angle * -1;
				
				//achar o angulo q difere e rodar o carro desse angulo
				carDirection = Quaternion.AngleAxis(angle, Vector3.up) * carDirection;
				transform.Rotate(0, angle, 0);
				
				wheelDirection = carDirection;
				interations = 0;
			}
			else if(enginePower < 0){
				float angle = Utilities.getAngle(carDirection, wheelDirection);
				if(Input.GetKey(right))
					angle = angle * -1;
				
				//achar o angulo q difere e rodar o carro desse angulo
				carDirection = Quaternion.AngleAxis(angle, Vector3.up) * carDirection;
				transform.Rotate(0, angle, 0);
				
				wheelDirection = carDirection;
				interations = 0;	
			}
		}
		else{
			if(interations < 0){
				interations += angleStep;
				wheelDirection = Quaternion.AngleAxis(angleStep, Vector3.up) * wheelDirection;
				Debug.DrawLine(startp, wheelDirection, Color.yellow, 1);
			}
			else if(interations > 0){
				interations -= angleStep;
				wheelDirection = Quaternion.AngleAxis(-angleStep, Vector3.up) * wheelDirection;
				Debug.DrawLine(startp, wheelDirection, Color.yellow, 1);
			}
		}
		
		if(Input.GetKey(brake)){
			loseSpeed(brakePower);
		}
		
		if(Input.GetKey(forward)){
			if(enginePower < MaxEnginePower)
				enginePower += acceleration;
			
		}
		else if (Input.GetKey(backward)){
			if(enginePower > -MaxEnginePower)
				enginePower -= acceleration/2;
		}
		else{
			loseSpeed(friction);
		}
		
		//O carro anda se tem engine power
		Debug.DrawLine(new Vector3(0,0,0), carDirection, Color.green, 1);
		if(isOnTheGround)
			transform.position = transform.position + (carDirection * enginePower);
	}
	
	private void loseSpeed(float lossFactor){
		if(enginePower < 0){
			enginePower += lossFactor;
			if(enginePower > 0)
				enginePower = 0;
		}
		else{
			enginePower -= lossFactor;
			if(enginePower < 0)
				enginePower = 0;
		}
	}
	void OnCollisionEnter(Collision collision){
		renderer.material.color = Color.yellow;
	}

	void OnCollisionStay(Collision collision){
		isOnTheGround = true;
	}

	void OnCollisionExit(Collision collision){
		isOnTheGround = false;
	}
	
}
