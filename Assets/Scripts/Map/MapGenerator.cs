using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

    public Transform mapPiece0;
    public int numberOfTilesX;
    public int numberOfTilesZ;

    private Transform [,] mapTiles;

    // Use this for initialization
	void Start () {
        mapTiles = new Transform[numberOfTilesX, numberOfTilesZ];

        //Certa o mapa com blocos que nao permitem o player ir para as pontas
        for(int i = 0; i < numberOfTilesX; i++)
        {
            string path = "Prefabs/Map_components/FullGrass";
            Object prefab = Resources.Load(path, typeof(GameObject));
            GameObject p = (GameObject)Instantiate(prefab, new Vector3(i * 6, 0, 0), Quaternion.identity);
            p.transform.SetParent(transform);
        }
        
        for(int i = 0; i < numberOfTilesX; i++)
        {
            Transform p = Instantiate(mapPiece0, new Vector3(i * 6, 0, (numberOfTilesZ - 1) * 6), Quaternion.identity) as Transform;
            p.SetParent(transform);
        }
        
        for(int i = 1; i < numberOfTilesZ - 1; i++)
        {
            Transform p = Instantiate(mapPiece0, new Vector3(0, 0, i * 6), Quaternion.identity) as Transform;
            p.SetParent(transform);
        }
        
        for(int i = 1; i < numberOfTilesZ - 1; i++)
        {
            Transform p = Instantiate(mapPiece0, new Vector3((numberOfTilesX - 1) * 6, 0, i * 6), Quaternion.identity) as Transform;
            p.SetParent(transform);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}
