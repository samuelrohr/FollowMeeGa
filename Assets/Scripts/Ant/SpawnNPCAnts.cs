using UnityEngine;
using System.Collections;

public class SpawnNPCAnts : MonoBehaviour {
	public int n;
	public GameObject antNPC;
	public static bool[,] freeTiles;
	public int maxX;
	public int maxY;
	Random rnd = new Random();

	// Use this for initialization
	void Start () {
		maxX = 24;
		maxY = 18;
		freeTiles = new bool[24,18];
		for (int i = 0; i < 24; i++) {
			for (int j = 0; j < 18; j++) {
				freeTiles[i,j] = i < j ;
			}
		}
		Quaternion spawnRotation = Quaternion.identity;
		for (int i = 0; i < n; i++) {
			int x = (int) Random.Range (0, maxX);
			int y = (int) Random.Range (0, maxY);
			while (!freeTiles[x,y]) {
				x = (int) Random.Range (0, maxX);
				y = (int) Random.Range (0, maxY);
			}
			AStarScript.TilePosition tile = new AStarScript.TilePosition (x, y);

			GameObject newAnt = (GameObject) Instantiate (antNPC, MapScript.GetTileCenterGlobalPosition(tile), spawnRotation);
			QuantdFormigas.AddLostAnt(newAnt);
		}


	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
