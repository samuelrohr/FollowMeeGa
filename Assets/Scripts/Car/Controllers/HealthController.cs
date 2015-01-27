using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	// Armazena quantos pontos de vida o carro ainda possui
	private float remainingHealthPoints;
	// Armazena os pontos de vida iniciais
	private float initialHealthPoints;
	// Armazena o comprimento inicial da barra de vida
	private float initialHealthBarWidth;

	// Enumeração com os tipos de colliders existentes, utilizado para dar peso diferente para cada colisão
	public enum DamageColliders {
		FRONT,
		TOP,
		RIGHT,
		LEFT,
		FRONTR,
		FRONTL,
		BACK,
		BOTTOM
	}

	public void SetInitialHealth (float initialHealthPoints) {
		this.initialHealthPoints = initialHealthPoints;
		remainingHealthPoints = initialHealthPoints;
		initialHealthBarWidth = guiTexture.pixelInset.width;
	}

	public void Hit (DamageColliders collider, float deltaMomentum) {
		// Calculamos o dano
		float damage = deltaMomentum * GetFactor (collider) * 0.0001f;
		//print ("Delta momentum: " + deltaMomentum + ", Damage: " + damage);

		remainingHealthPoints -= damage;

		if (remainingHealthPoints <= 0) {
			guiTexture.pixelInset = new Rect (guiTexture.pixelInset.x, guiTexture.pixelInset.y,
			                                  0, guiTexture.pixelInset.height);
			//Sinalizar morte
			AlertDeath ();
		} else {
			guiTexture.pixelInset = new Rect (guiTexture.pixelInset.x, guiTexture.pixelInset.y,
			                                  remainingHealthPoints * initialHealthBarWidth / initialHealthPoints,
			                                  guiTexture.pixelInset.height);
		}
	}

	public void KillCar(){
		AlertDeath();
	}

	public void HealCar(float healValue){
		remainingHealthPoints += healValue;
		if (remainingHealthPoints > initialHealthPoints)
			remainingHealthPoints = initialHealthPoints;
	}

	private void AlertDeath () {
		ApplicationData.defeatedPlayerName = transform.parent.parent.name;
		Application.LoadLevel("DeathMenu");
	}

	private float GetFactor (DamageColliders collider) {
		switch (collider) {
		case DamageColliders.FRONT:
		case DamageColliders.FRONTR:
		case DamageColliders.FRONTL:
			return 1;
		case DamageColliders.TOP:
			return 4;
		case DamageColliders.RIGHT:
		case DamageColliders.LEFT:
			return 3;
		case DamageColliders.BACK:
			return 2;
		case DamageColliders.BOTTOM:
			return 5;
		default:
			return 0;
		}
	}

}
