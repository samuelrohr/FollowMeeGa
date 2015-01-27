using UnityEngine;
using System.Collections;

public class MatchConstructor : MonoBehaviour {
	public Transform gameCamera;
	private Vector3 player1RespawnPos;
	private Quaternion player1RespawnRot;
	private Vector3 player2RespawnPos;
	private Quaternion player2RespawnRot;
	private GameObject player1Car;
	private GameObject player2Car;
	private Vector3 terrainSize;
	private Vector3 terrainPos;
	private int boxCount;
	private float deltaTime;

	// Use this for initialization
	void Start () {
		deltaTime = 0;
		//Carregamos o level desejado
		string path = "Prefabs/Levels/" + ApplicationData.levelName;
		Object prefab = Resources.Load(path);
		GameObject level = (GameObject)GameObject.Instantiate(prefab);
		level.transform.parent = transform;
		terrainSize = level.GetComponent<Terrain>().terrainData.size;
		terrainPos = level.transform.position;

		//Criamos o carro do player1
		path = "Prefabs/Cars/" + ApplicationData.carNamePlayer1;
		prefab = Resources.Load(path);
		GameObject newCar = (GameObject)GameObject.Instantiate(prefab);

		//Armazenamos o carro bem como sua posicao e rotacao
		player1Car = newCar;
		player1RespawnRot = newCar.transform.rotation;
		player1RespawnPos = transform.FindChild(ApplicationData.levelName + "(Clone)").FindChild("player1Respawn").position;

		//Setamos o pai do carro criado
		newCar.transform.parent = GameObject.Find("Player1").transform;

		//Posicionamos o carro no mapa
		newCar.transform.position = player1RespawnPos;

		//Configuramos as teclas de jogo
		newCar.transform.GetComponent<CarController>().setControlKeys(
											ApplicationData.forwardPlayer1,
											ApplicationData.backwardPlayer1,
											ApplicationData.leftPlayer1,
											ApplicationData.rightPlayer1);
		
		//Criamos a camera do player1
		Transform cam = Instantiate(gameCamera) as Transform;
		cam.transform.parent = GameObject.Find("Player1").transform;
		FollowTarget camFollowTarget = cam.GetComponent<FollowTarget>();
		camFollowTarget.setTarget(transform.FindChild("Player1").FindChild(ApplicationData.carNamePlayer1 + "(Clone)"));
		camFollowTarget.setCameraRect(0, 0, 0.5f, 1);

		//Criamos o carro do player2
		path = "Prefabs/Cars/" + ApplicationData.carNamePlayer2;
		prefab = Resources.Load(path);
		newCar = (GameObject)GameObject.Instantiate(prefab);

		player2Car = newCar;
		player2RespawnRot = newCar.transform.rotation;
		player2RespawnPos = transform.FindChild(ApplicationData.levelName + "(Clone)").FindChild("player2Respawn").position;

		newCar.transform.parent = GameObject.Find("Player2").transform;

		newCar.transform.position = player2RespawnPos;

		newCar.transform.GetComponent<CarController>().setControlKeys(
											ApplicationData.forwardPlayer2,
											ApplicationData.backwardPlayer2,
											ApplicationData.leftPlayer2,
											ApplicationData.rightPlayer2);
		
		//Criamos a camera do player2
		cam = Instantiate(gameCamera) as Transform;
		cam.transform.parent = GameObject.Find("Player2").transform;
		camFollowTarget = cam.GetComponent<FollowTarget>();
		camFollowTarget.setTarget(transform.FindChild("Player2").FindChild(ApplicationData.carNamePlayer2  + "(Clone)"));
		camFollowTarget.setCameraRect(0.5f, 0, 0.5f, 1);

		//Se o modo de jogo e derrubada
		if (ApplicationData.gameMode.Equals ("Derrubada")) {
			//Temos que desabilitar o FixedDamageCollider dos carros
			player1Car.GetComponent<FixedDamageCollider>().enabled = false;
			player2Car.GetComponent<FixedDamageCollider>().enabled = false;

			//Desabilitamos as texturas da vida
			transform.FindChild("Player1").FindChild("healthGUI").GetComponent<GUITexture>().enabled = false;
			transform.FindChild("Player1").FindChild("healthGUI").FindChild("healthBar").GetComponent<GUITexture>().enabled = false;
			transform.FindChild("Player2").FindChild("healthGUI").GetComponent<GUITexture>().enabled = false;
			transform.FindChild("Player2").FindChild("healthGUI").FindChild("healthBar").GetComponent<GUITexture>().enabled = false;
		}
	}

	void Update(){
		if(Input.GetKey(ApplicationData.resetCarsKey)){
			if(player1Car.GetComponent<CarController>().currSpeed <= 0 &&
			   				player2Car.GetComponent<CarController>().currSpeed <= 0){
				player1Car.transform.position = player1RespawnPos;
				player1Car.transform.rotation = player1RespawnRot;
				player2Car.transform.position = player2RespawnPos;
				player2Car.transform.rotation = player2RespawnRot;
			}
		}
		if (deltaTime <= 300) {
			deltaTime++;
		}
		if (ApplicationData.boxCount < 30 && deltaTime > 300.0f) {
			string path = "Prefabs/Box";
			Object prefab = Resources.Load(path);
			GameObject newBox = (GameObject)GameObject.Instantiate(prefab);
			float min = player2Car.transform.position.x - 100;
			float max = player2Car.transform.position.x + 100;
			float x = Random.Range(min, max);
			min = player1Car.transform.position.z - 100;
			max = player1Car.transform.position.z + 100;
			float z = Random.Range(min, max);
			newBox.transform.position = new Vector3(x, terrainSize.y + 100, z);

			newBox.transform.parent = transform;

			newBox.GetComponent<BoxInfo>().setBoxType(Mathf.FloorToInt(Random.Range(1, 6)));

			deltaTime = 0;
			ApplicationData.boxCount++;
		}
	}
}
