using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
	//Colliders de cada roda
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelBR;
	public WheelCollider wheelBL;

	//Torque maximo aplicado pelo motor
	public float maxTorque;

	//Variaveis para controle da velocidade
	public float frition;
	public float topSpeed;
	public float topBackwardSpeed;
	public float currSpeed;  //On Km/h

	//Variaveis para controle da cuvartura da roda em uma curva
	public float lowSpeedSteerAngle;
	public float highSpeedSteerAngle;
	private float lowestSteerAngleAtSpeed;
	
	//Botoes usados para contolar o carro
	public KeyCode forward;
	public KeyCode backward;
	public KeyCode left;
	public KeyCode right;

	//Transforms de cada roda
	public Transform wheelFLTransform;
	public Transform wheelFRTransform;
	public Transform wheelBLTransform;
	public Transform wheelBRTransform;

	public float centerOfMassY;

	// Pontos iniciais de vida
	public float healthPoints;

	// Use this for initialization
	void Start () {
		if (centerOfMassY > 0)
			centerOfMassY = centerOfMassY * -1;

		if(topBackwardSpeed > 0)
			topBackwardSpeed = topBackwardSpeed * -1;
		rigidbody.centerOfMass = new Vector3(rigidbody.centerOfMass.x, centerOfMassY, rigidbody.centerOfMass.z);

		lowestSteerAngleAtSpeed = topSpeed / 2;
	}
	
	// Update is called once per frame
	void Update () {

		float speedFactor = rigidbody.velocity.magnitude/lowestSteerAngleAtSpeed;
		//A abertura da curva feita pelo carro depende da velocidade com que esse esta se movendo
		//Interpolamos esse valor entre o minimo de SteerAngle, o maximo de SteerAngle ponderando pelo speedFactor
		float currSteerAngle = Mathf.Lerp(lowSpeedSteerAngle, highSpeedSteerAngle, speedFactor);

		//Removemos qualquer brake aplicado ao torque
		wheelBR.brakeTorque = 0;
		wheelBL.brakeTorque = 0;
		wheelFL.brakeTorque = 0;
		wheelFR.brakeTorque = 0;

		currSpeed = Mathf.Round(2 * 3.14f * wheelFL.radius * wheelFL.rpm * 60 / 1000);

		//Verifica input do teclado
		if(Input.GetKey(forward)){
			wheelBR.motorTorque = maxTorque;
			wheelBL.motorTorque = maxTorque;
		}
		else if(Input.GetKey(backward)){
			wheelBR.motorTorque = -maxTorque;
			wheelBL.motorTorque = -maxTorque;
		}
		else{
			wheelBR.motorTorque = 0;
			wheelBL.motorTorque = 0;
			wheelBR.brakeTorque = frition;
			wheelBL.brakeTorque = frition;
			wheelFR.brakeTorque = frition;
			wheelFL.brakeTorque = frition;
		}
		if(currSpeed >= topSpeed || currSpeed <= topBackwardSpeed){
			wheelBL.motorTorque = 0;
			wheelBR.motorTorque = 0;
		}

		if(Input.GetKey(left)){
			wheelFL.steerAngle = -currSteerAngle;
			wheelFR.steerAngle = -currSteerAngle;
		}
		else if(Input.GetKey(right)){
			wheelFL.steerAngle = currSteerAngle;
			wheelFR.steerAngle = currSteerAngle;
		}
		else{
			wheelFL.steerAngle = 0;
			wheelFR.steerAngle = 0;
		}

		//Anima as rodas para girarem no seu proprio eixo
		makeWheelsSpin();

		//Anima as rodas da frente para girarem na direçao da curva feita
		makeFrontWheelsSteer();
	}

	void makeWheelsSpin(){
		float direction = -1;
		if(wheelFL.motorTorque < 0)
			direction = 1;
		wheelFLTransform.Rotate(0, 0, direction * wheelFL.rpm/60*360*Time.deltaTime);
		wheelFRTransform.Rotate(0, 0, direction * wheelFR.rpm/60*360*Time.deltaTime);
		wheelBLTransform.Rotate(0, 0, direction * wheelBL.rpm/60*360*Time.deltaTime);
		wheelBRTransform.Rotate(0, 0, direction * wheelBR.rpm/60*360*Time.deltaTime);	
	}

	void makeFrontWheelsSteer(){
		Vector3 aux;
		aux = wheelFLTransform.localEulerAngles;
		wheelFLTransform.localEulerAngles = new Vector3(aux.x, wheelFL.steerAngle, aux.z);
		aux = wheelFLTransform.localEulerAngles;
		wheelFRTransform.localEulerAngles = new Vector3(aux.x, wheelFR.steerAngle, aux.z);
	}

	public void setControlKeys(KeyCode forward, KeyCode backward, KeyCode left, KeyCode right){
		this.forward = forward;
		this.backward = backward;
		this.left = left;
		this.right = right;
	}
}
