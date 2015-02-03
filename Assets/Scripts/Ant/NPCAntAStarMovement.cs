using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCAntAStarMovement : MonoBehaviour
{

    private Stack<AStarScript.TilePosition> pathStack;
    public static float moveSpeed = 5;
    private bool goForward = false;

    // Use this for initialization
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if (goForward)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void SetNewDestination(AStarScript.TilePosition destinionTile)
    {
        // Devemos primeiramente obter o caminho que devemos percorrer
        AStarScript.TilePosition homeTile = MapScript.GetTileFromPosition(transform.position);
        pathStack = AStarScript.AStarAlgorithm.GetBestPath(MapScript.mapMatrix, homeTile, destinionTile);
        // Removemos o home tile
        if (pathStack.Count > 0)
        {
            pathStack.Pop();
        }
        StartCoroutine(MoveToNextTile());
    }

    IEnumerator MoveToNextTile()
    {
        float waitTime;
        if (pathStack.Count == 0)
        {
            goForward = false;
            transform.GetComponent<NPCAntJobScript>().finished();
        } else
        {
            AStarScript.TilePosition nextTile = pathStack.Pop();
            Vector3 nextPosition = MapScript.GetTileCenterGlobalPosition(nextTile);
            Vector3 distance = nextPosition - transform.position;
            float angle = Vector3.Angle(Vector3.forward, distance);
            if(distance.x < 0.0f)
                angle = angle * -1.0f;
            transform.eulerAngles = new Vector3(0, angle, 0);
            // Calculamos o tempo gasto
            waitTime = distance.magnitude / moveSpeed;
            // Habilitamos o movimento
            goForward = true;
            yield return new WaitForSeconds(waitTime);
            goForward = false;
            StartCoroutine(MoveToNextTile());
        }
    }

}
