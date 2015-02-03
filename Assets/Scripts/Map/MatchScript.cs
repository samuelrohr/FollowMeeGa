using UnityEngine;
using System.Collections;

public class MatchScript : MonoBehaviour {

    private MapGenerator mapGenerator;
    private SpawnNPC spawnNPC;
    private MapScript mapScript;
    private OrthographicCamera cameraOrthographicScript;
    private GameObject [] sugarTiles;
    private NPCAntJobScript [] antNPCStatus;

	// Use this for initialization
	void Start () {
        mapGenerator = transform.FindChild("Map").FindChild("MapPieces").GetComponent<MapGenerator>();
        spawnNPC = transform.FindChild("Map").FindChild("MapPieces").GetComponent<SpawnNPC>();
        mapScript = transform.FindChild("Map").GetComponent<MapScript>();
        cameraOrthographicScript = transform.FindChild("CameraOrtografica").GetComponent<OrthographicCamera>();

        //Solicitamos que o mapa seja gerado
        mapGenerator.generateLevel();
        //Guardamos os tiles de sugar colocados no mapa
        sugarTiles = mapGenerator.getSugarTiles();

        //Devemos iniciar o MapScript agora
        mapScript.initMapScript();
        
        //Informamos ao script que gera as formigas o tamanho do mapa
        //O valor de maxX e maxY 'e dado em quantidade de tile lembrando que cada bigTile tem 3 tiles
        spawnNPC.setMaxTilesAndMap(mapGenerator.numberOfBigTilesX * 3, mapGenerator.numberOfBigTilesZ * 3, mapScript.getMapMatrix());

        //Zeramos o nivel de acuar e a contagem de formigas
        NivelAcucar.Acucar = 0f; 
        QuantdFormigas.clearLists();

        //Solicitamos o spawn das ants e hopper npc e do player
        spawnNPC.SpawnAnts();
        antNPCStatus = spawnNPC.getAntsSpawnedStatus();

        spawnNPC.SpawnHopper();    
        
        //Iniciamos a camera Ortografica
        cameraOrthographicScript.initOrthoCamera(mapGenerator.numberOfBigTilesX, mapGenerator.numberOfBigTilesZ);

	}

    void LateUpdate()
    {
        int controll = 0;
        bool noneWorking = true;

        for(int i = 0; i < sugarTiles.Length; i++)
        {
            //Verificamos se aquela peca de chocolate ja foi comida
            if(sugarTiles[i].transform.FindChild("GroundChocolate").childCount == 0)
            {
                controll++;
            }
        }

        for(int j = 0; j < antNPCStatus.Length; j++)
        {
            if(antNPCStatus[j].isWorking)
                noneWorking = false;
        }
        //Se todos os chocolates foram pegos mais o total de acucar ainda nao foi coletado e 
        //nao ha nunhuma formiga trabalhando (ou seja carregando acucar)
        //Entao e gameOver (OBS isso ocorre qd uma formiga esta carregando um acucar e o gafanhoto
        //come ela antes dela chegar ao anthill)
        if(controll == sugarTiles.Length && noneWorking)
        {
            Application.LoadLevel("GameOver");
        }
    }

    void OnLevelWasLoaded(int level) {
        NivelAcucar.Acucar = 0f; 

        QuantdFormigas.clearLists();
    }
}
