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
    private NPCAntJobScript [] antStatus;

    // Use this for initialization
    public void SpawnAnts() {  
        antStatus = new NPCAntJobScript[numberAnts];

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
            antStatus[i] = newAnt.transform.GetComponent<NPCAntJobScript>();
        }

    }

    public void SpawnHopper() {
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
    
    public void setMaxTilesAndMap(int x, int y, bool[,] freeTilesMap)
    {
        maxTilesX = x;
        maxTilesY = y;
        freeTiles = freeTilesMap;
    }

    public NPCAntJobScript[] getAntsSpawnedStatus()
    {
        return antStatus;
    }
}
