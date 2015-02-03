using UnityEngine;
using System.Collections;

public class SpawnNPC : MonoBehaviour {
    public int numberAnts;
    public int numberHoppers;
    public float hopperDifficulty;
    public GameObject antNPC;
    public GameObject grassHopper;
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
        for (int i = 0; i < numberAnts; i++) {
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

    public void SpawnHopper() {
        freeTiles = new bool[maxTilesX,maxTilesY];
        for (int i = 0; i < maxTilesX; i++) {
            for (int j = 0; j < maxTilesY; j++) {
                freeTiles[i,j] = i < j ;
            }
        }
        Quaternion spawnRotation = Quaternion.identity;
        for (int i = 0; i < numberHoppers; i++) {
            int x = (int) Random.Range (0, maxTilesX);
            int y = (int) Random.Range (0, maxTilesY);
            while (!freeTiles[x,y]) {
                x = (int) Random.Range (0, maxTilesX);
                y = (int) Random.Range (0, maxTilesY);
            }
            AStarScript.TilePosition tile = new AStarScript.TilePosition (x, y);
            
            GameObject newHopper = (GameObject) Instantiate (grassHopper, MapScript.GetTileCenterGlobalPosition(tile), spawnRotation);
            QuantdFormigas.AddLostAnt(newHopper);
            newHopper.transform.parent = transform.parent;
            newHopper.GetComponent<BeetleBehavior>().difficulty = hopperDifficulty; 
            newHopper.transform.eulerAngles = new Vector3(-90, 0, 0); 
        }
        
        
    }
    
    public void setMaxTiles(int x, int y)
    {
        maxTilesX = x;
        maxTilesY = y;
    }
}
