using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
	public Transform target;
	public float distance;
	public float height;
	public float rotationDamping;
	public float heightDamping;
	public float zoomRatio;
	public float defaultFOV;
	private Vector3 rotationVector;

	public void setTarget(Transform target){
		this.target = target;
	}

	public void setCameraRect(float x, float y, float width, float height){
		camera.rect = new Rect(x, y, width, height);
	}

	// LateUpdate e achamado ao fim da montagem do frame
	void LateUpdate () {
		float nextAngle = rotationVector.y;
		float nextHeight = target.position.y + height;
		float camAngle = transform.eulerAngles.y;
		float camHeight = transform.position.y;

		//Faz a movimentaçao da camera interpolada entre os valores atual e proximo ponderando pelo damping
		//camAngle = Mathf.LerpAngle(camAngle, nextAngle, Time.deltaTime);
        camAngle = nextAngle;
		camHeight = Mathf.Lerp(camHeight, nextHeight, heightDamping * Time.deltaTime);

		Quaternion currAngle = Quaternion.Euler(0, camAngle, 0);
        transform.position = target.position;
		transform.position -= currAngle * Vector3.forward * distance;
		transform.position = new Vector3(transform.position.x, camHeight, transform.position.z);
		transform.LookAt(target);
	}

	void FixedUpdate(){

		//De acordo com a velocidade do target coloca a camera atras dele ou na frente dele
		rotationVector = new Vector3(0, target.eulerAngles.y , 0);
	
		//Faz o efeito de zoom out quando o target anda
		camera.fieldOfView = defaultFOV;
	}
}
