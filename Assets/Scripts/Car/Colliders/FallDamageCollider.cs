using UnityEngine;
using System.Collections;

public class FallDamageCollider : MonoBehaviour {
	private HealthController carBloodController;

	void Start () {
		// Obtemos o controlador da vida
		carBloodController = transform.parent.
			FindChild ("healthGUI").
				FindChild ("healthBar").
				GetComponent<HealthController> ();
		// Iniciamos o valor inicial dos pontos
		carBloodController.SetInitialHealth(transform.GetComponent<CarController> ().healthPoints);
	}

	void OnCollisionEnter (Collision collision) {
		// Devemos checar as colisões
		foreach (ContactPoint contact in collision.contacts) {
			//Checamos se houve colisao com o fundo da fase
			if (contact.otherCollider.name.Equals ("bottom")) {
				carBloodController.KillCar();
				return;
			}
		}
	}

	void OnCollisionStay(Collision collision){
		foreach (ContactPoint contact in collision.contacts) {
			if(contact.otherCollider.name.Equals("bottom")){
				carBloodController.KillCar();
				return;
			}
		}
	}
}
