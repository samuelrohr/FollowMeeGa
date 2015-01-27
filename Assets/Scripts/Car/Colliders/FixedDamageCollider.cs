using UnityEngine;
using System.Collections;

public class FixedDamageCollider : MonoBehaviour {

	private HealthController carBloodController;
	
	void Start () {
		// Obtemos o controlador da vida
		carBloodController = transform.parent.
			FindChild ("healthGUI").
				FindChild ("healthBar").
				GetComponent<HealthController> ();
		// Iniciamos o valor inicial dos pontos
		carBloodController.SetInitialHealth (transform.GetComponent<CarController> ().healthPoints);
	}
	
	void OnCollisionEnter (Collision collision) {
		// Devemos checar as colisões
		foreach (ContactPoint contact in collision.contacts) {
			// Checamos se a colisao foi com uma caixa
			if(contact.otherCollider.name.Equals("Box(Clone)")){
				if(transform.GetComponent<Effect>().enabled == false){
					int boxtype = contact.otherCollider.gameObject.GetComponent<BoxInfo>().getBoxType();
					transform.GetComponent<Effect>().setEffectType(boxtype);
					transform.GetComponent<Effect>().enabled = true;
					Destroy(contact.otherCollider.gameObject);
				}
			}

			// Checamos se devemos contabilizar o dano, ou seja, se interagimos com o outro veículo, ou se estamos capotando
			// Se for detectada uma colisão válida, requisitamos a computação do dano
			bool validCollision = false;
			// Selecionamos qual foi a colisão
			HealthController.DamageColliders damageCollider;
			switch (contact.thisCollider.name) {
			case "ColliderFront":
				damageCollider = HealthController.DamageColliders.FRONT;
				break;
			case "ColliderTop":
				damageCollider = HealthController.DamageColliders.TOP;
				break;
			case "ColliderRight":
				damageCollider = HealthController.DamageColliders.RIGHT;
				break;
			case "ColliderLeft":
				damageCollider = HealthController.DamageColliders.LEFT;
				break;
			case "ColliderFrontR":
				damageCollider = HealthController.DamageColliders.FRONTR;
				break;
			case "ColliderFrontL":
				damageCollider = HealthController.DamageColliders.FRONTL;
				break;
			case "ColliderBack":
				damageCollider = HealthController.DamageColliders.BACK;
				break;
			case "ColliderBottom":
				damageCollider = HealthController.DamageColliders.BOTTOM;
				break;
			default:
				// Ignoramos a colisão
				if (contact.otherCollider.name.Equals ("bottom")) {
					carBloodController.KillCar();
					return;
				}
				return;
			}
			
			// Agora vemos se a colisão foi válida
			if (contact.otherCollider != null) {
				validCollision = true;
				if (contact.otherCollider.name.Equals ("Terrain") && damageCollider == HealthController.DamageColliders.BOTTOM) {
					// Aqui, apenas consideramos capotamento ou raspagem das laterais no solo, portanto, desconsideramos colisão com a base
					validCollision = false;
				} 
				if (contact.otherCollider.name.Equals ("bottom")) {
					carBloodController.KillCar();
					return;
				}
			}
			
			// Computamos o dano se necessário
			if (validCollision) {
				// Calculamos a variação da quantidade de movimento
				float detalMomemtumX = (collision.relativeVelocity.x - Mathf.Abs (transform.rigidbody.velocity.x) * transform.rigidbody.mass);
				float deltaMomemtumY = (collision.relativeVelocity.y - Mathf.Abs (transform.rigidbody.velocity.y) * transform.rigidbody.mass);
				float detalMomemtumZ = (collision.relativeVelocity.z - Mathf.Abs (transform.rigidbody.velocity.z) * transform.rigidbody.mass);
				float deltaMomentum = (float)Mathf.Sqrt (Mathf.Pow (deltaMomemtumY, 2) + Mathf.Pow (detalMomemtumX, 2) + Mathf.Pow (detalMomemtumZ, 2));
				
				// Informamos a colisão
				carBloodController.Hit (damageCollider, deltaMomentum);
			}
		}
	}

}
