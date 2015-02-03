using UnityEngine;
using System.Collections;

public class SpawnNPCAnts : MonoBehaviour {
	public int n;
	public GameObject antNPC;
	public static bool[,] freeTiles;
	private int maxTilesX;
	private int maxTilesY;

	// Use this for initialization
	public void SpawnAnts() {
		freeTiles = new bool[maxTilesX,maxTilesY];
		for (int i = 0; i < maxTilesX; i++) {
			for (int j = 0; j < maxTilesY; j++) {
				freeTiles[i,j] = i < j ;
			}
		}
		Quaternion spawnRotation = Quaternion.identity;
		for (int i = 0; i < n; i++) {
			int x = (int) Random.Range (0, maxTilesX);
			int y = (int) Random.Range (0, maxTilesY);
			while (!freeTiles[x,y]) {
				x = (int) Random.Range (0, maxTilesX);
				y = (int) Random.Range (0, maxTilesY);
			}
			AStarScript.TilePosition tile = new AStarScript.TilePosition (x, y);

			GameObject newAnt = (GameObject) Instantiate (antNPC, MapScript.GetTileCenterGlobalPosition(tile), spawnRotation);
			QuantdFormigas.AddLostAnt(newAnt);
            newAnt.transform.parent = transform.parent;
		}


	}

	public void setMaxTiles(int x, int y)
    {
        maxTilesX = x;
        maxTilesY = y;
    }


}
