using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapScript : MonoBehaviour
{
    private static Vector3 mapMin;
    private static Vector3 mapMax;

    public static bool[,] mapMatrix;

    //TODO
    public static AStarScript.TilePosition anthillTile;

    public void initMapScript()
    {
        //Pegamos os limites do terreno
        Transform mapSize = transform.FindChild("MapSize").FindChild("Quad");
        mapMax = mapSize.GetComponent<Renderer>().bounds.max;
        mapMin = mapSize.GetComponent<Renderer>().bounds.min;

        //Escondemos a mascara q demarca o tamanho do terreno
        mapSize.GetComponent<Renderer>().enabled = false;

        //Pq o unity e zuado
        float lengthX = (mapMax.x - mapMin.x);
        int aux = (int)lengthX / 2;

        //Criamos a matrix de posicoes do mapa
        mapMatrix = new bool[aux, (int)(mapMax.z - mapMin.z) / 2];

        Transform mapPieces = transform.FindChild("MapPieces");

        //Cada filho da mapPieces e um bigtile (conjunto de 9 tiles)
        for (int i = 0; i < mapPieces.childCount; i++)
        {
            Transform bigTile = mapPieces.GetChild(i);
            //Para cada bigtile temos 9 filhos que sao as posicoes no mapa efetivamente
            for (int j = 0; j < bigTile.childCount; j++)
            {
                Transform tile = bigTile.GetChild(j);
                AStarScript.TilePosition tilePosition = GetTileFromPosition(tile.position);

                //verificamos se a posicao esta livre
                if (tile.name.Equals("Ground"))
                    mapMatrix [tilePosition.X(), tilePosition.Y()] = true;
                else if(tile.name.Equals("GroundEntrance"))
                {
                    mapMatrix[tilePosition.X(), tilePosition.Y()] = true;
                    anthillTile = tilePosition;
                }
                else if(tile.name.Equals("Anthill"))
                {
                    mapMatrix[tilePosition.X(), tilePosition.Y()] = true;
                }
            }

        }
    }

    public static Vector3 GetTileCenterGlobalPosition(AStarScript.TilePosition position)
    {
        return new Vector3((position.X() * 2 + 1) + mapMin.x, 0.1f, (position.Y() * 2 + 1) + mapMin.z);
    }

    public static AStarScript.TilePosition GetTileFromPosition(Vector3 position)
    {
        float deltaX = (position.x - mapMin.x);
        int indexX = (int)(deltaX / 2f);
        float deltaY = (position.z - mapMin.z);
        int indexY = (int)(deltaY / 2f);

        return new AStarScript.TilePosition(indexX, indexY);
    }

    public Vector3 getMapMin()
    {
        return mapMin;
    }

    public Vector3 getMapMax()
    {
        return mapMax;
    }

}
