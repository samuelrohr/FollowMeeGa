using UnityEngine;
using System.Collections;

public class NPCAntJobScript : MonoBehaviour {

    private NPCAntAStarMovement movementScript;

    private static AStarScript.TilePosition anthillTile;
    private AStarScript.TilePosition jobTile;

    public bool isWorking;
    private bool isCarringSugar;

    private Transform sugarBar;


	// Use this for initialization
	void Start () {
        isWorking = false;
        isCarringSugar = false;

        movementScript = transform.GetComponent<NPCAntAStarMovement>();

        anthillTile = MapScript.anthillTile;
       
	}

    public void finished()
    {
        AStarScript.TilePosition currentTile = MapScript.GetTileFromPosition(transform.position);
        if ( currentTile.Equals(jobTile))
        {
            if(Physics.Raycast(transform.position, transform.forward) && sugarBar != null)
            {
                movementScript.SetNewDestination(anthillTile);
                isCarringSugar = true;
                sugarBar.GetComponent<SugarScript>().Eat();
                if(!sugarBar.GetComponent<SugarScript>().hasMorePieces())
                {
                    Destroy(sugarBar.gameObject);
                }
            }
            else
            {
                isWorking = false;
                sugarBar = null;
            }
        } else if (currentTile.Equals(anthillTile))
        {
            if(isCarringSugar)
            {
                NivelAcucar.Coleta(1);
                isCarringSugar = false;
            }
            movementScript.SetNewDestination(jobTile);
        }
    }
   
    public void AssignJob(AStarScript.TilePosition tilePosition, Transform sugarBar)
    {
        // Armazenamos a informação referente ao trabalho
        jobTile = tilePosition;
        this.sugarBar = sugarBar;
        isWorking = true;
        // Mandamos a formiga para o tile
        movementScript.SetNewDestination(jobTile);
    }
}
