using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {
	private float effectTime;
	private float backup1;
	private int effectType;

	void Start () {
		transform.GetComponent<Effect> ().enabled = false;
	}

	void OnEnable(){
		switch(effectType){
		case 1:
			transform.parent.FindChild("healthGUI").FindChild("healthBar").GetComponent<HealthController>().HealCar(100);
			effectTime = 0;
			print(transform.name + "was healed");
			break;
		case 2:
			backup1 = transform.GetComponent<CarController>().topSpeed;
			transform.GetComponent<CarController>().topSpeed = backup1 + 100;
			effectTime = 500;
			print(transform.name + " SpeedUP");
			break;
		case 3:
			backup1 = transform.GetComponent<CarController>().centerOfMassY;
			transform.GetComponent<CarController>().centerOfMassY = 0.5f;
			effectTime = 300;
			print(transform.name + " centerOfMass change");
			break;
		case 4:
			backup1 = transform.GetComponent<CarController>().topSpeed;
			transform.GetComponent<CarController>().topSpeed = backup1 - 100;
			effectTime = 300;
			print(transform.name + " SpeedDOWN");
			break;
		case 5:
			backup1 = transform.GetComponent<Rigidbody>().mass;
			transform.GetComponent<Rigidbody>().mass = backup1 - 500;
			effectTime = 200;
			print(transform.name + " mass reduced");
			break;
		case 6:
			backup1 = transform.GetComponent<CarController>().lowSpeedSteerAngle;
			transform.GetComponent<CarController>().lowSpeedSteerAngle = 1;
			effectTime = 200;
			print(transform.name + " speed steer angle");
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		effectTime--;
		if(effectTime <= 0){
			switch(effectType){
			case 2:
				transform.GetComponent<CarController>().topSpeed = backup1;
				break;
			case 3:
				transform.GetComponent<CarController>().centerOfMassY = backup1;
				break;
			case 4:
				transform.GetComponent<CarController>().topSpeed = backup1;
				break;
			case 5:
				transform.GetComponent<Rigidbody>().mass = backup1;
				break;
			case 6:
				transform.GetComponent<CarController>().lowSpeedSteerAngle = backup1;
				break;
			}
			print("Effect off");
			this.enabled = false;
		}
	}

	public void setEffectType(int type){
		effectType = type;
	}
}
