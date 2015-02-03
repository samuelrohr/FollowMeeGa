using UnityEngine;
using System.Collections;

public class OrthographicCamera : MonoBehaviour {

    private int numberofTiles;

	// Use this for initialization
	public void initOrthoCamera (int numberOfTilesX, int numberOfTilesZ) {
        GetComponent<Camera>().enabled = false;
        GetComponent<Camera>().enabled = true;

        Vector3 mapMax = transform.parent.FindChild("Map").GetComponent<MapScript>().getMapMax();
        Vector3 mapMin = transform.parent.FindChild("Map").GetComponent<MapScript>().getMapMin();
        
        transform.position = new Vector3(mapMin.x, 100, mapMax.z);

        if(numberOfTilesX > numberOfTilesZ)
            numberofTiles = numberOfTilesX;
        else
            numberofTiles = numberOfTilesZ;

        GetComponent<Camera>().camera.orthographicSize = numberofTiles * 6;
	}
   
}
