using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
    
    private Object [] pieces;

    public int numberOfBigTilesX;
    public int numberOfBigTilesZ;
    public int totalOfSugar;
    private int numberOfSugarTiles;
    
    private int [,,] mapTilesPosition;
    private GameObject [,] mapTiles;
    private bool done;

    private Surroundings surroundings;

    //Constantes
    private const int RIGHT = 0;
    private const int UP = 1;
    private const int LEFT = 2;
    private const int DOWN = 3;

    private const int FullGrass = 0;
    private const int FullGrass2 = 1;
    private const int Empty = 2;
    private const int Anthill = 3;
    private const int LeftT = 4;
    private const int LineH = 5;
    private const int LineV = 6;
    private const int LowerLeft = 7;
    private const int LowerRight = 8;
    private const int LowerT = 9;
    private const int RightT = 10;
    private const int Sugar = 11;
    private const int UpperLeft = 12;
    private const int UpperRight = 13;
    private const int UpperT = 14;
    private const int isAPath = 20;

    // Use this for initialization
    public void generateLevel () {
        if(totalOfSugar <= 0)
            totalOfSugar = 1;

        numberOfSugarTiles = totalOfSugar;


        surroundings = new Surroundings(numberOfBigTilesX, numberOfBigTilesZ);

        done = false;
        pieces = new Object[15];
        
        pieces[0] = Resources.Load("Prefabs/Map_pieces/FullGrass", typeof(GameObject));
        pieces[1] = Resources.Load("Prefabs/Map_pieces/FullGrass2", typeof(GameObject));
        pieces[2] = Resources.Load("Prefabs/Map_pieces/Empty", typeof(GameObject));
        pieces[3] = Resources.Load("Prefabs/Map_pieces/Anthill", typeof(GameObject));
        pieces[4] = Resources.Load("Prefabs/Map_pieces/LeftT", typeof(GameObject));
        pieces[5] = Resources.Load("Prefabs/Map_pieces/LineH", typeof(GameObject));
        pieces[6] = Resources.Load("Prefabs/Map_pieces/LineV", typeof(GameObject));
        pieces[7] = Resources.Load("Prefabs/Map_pieces/LowerLeft", typeof(GameObject));
        pieces[8] = Resources.Load("Prefabs/Map_pieces/LowerRight", typeof(GameObject));
        pieces[9] = Resources.Load("Prefabs/Map_pieces/LowerT", typeof(GameObject));
        pieces[10] = Resources.Load("Prefabs/Map_pieces/RightT", typeof(GameObject));
        pieces[11] = Resources.Load("Prefabs/Map_pieces/Sugar", typeof(GameObject));
        pieces[12] = Resources.Load("Prefabs/Map_pieces/UpperLeft", typeof(GameObject));
        pieces[13] = Resources.Load("Prefabs/Map_pieces/UpperRight", typeof(GameObject));
        pieces[14] = Resources.Load("Prefabs/Map_pieces/UpperT", typeof(GameObject));
        
        mapTilesPosition = new int[numberOfBigTilesX, numberOfBigTilesZ, 2];
        mapTiles = new GameObject[numberOfBigTilesX,numberOfBigTilesZ];
        
        //Primeiro criamos as bordas
        fillEdges();

        //Depois colocamos o Anthill
        GameObject p = (GameObject)Instantiate(pieces[3], new Vector3(6, 0, 6), Quaternion.identity);
        p.transform.SetParent(transform);
        mapTiles[1,1] = p;
        //Marcamos a posicao q o anthill ocupou como ocupada
        mapTilesPosition[1, 1, 0] = -1;
        //Marcamos as areas que sao efetadas pela presenca do anthill
        mapTilesPosition[2, 1, 0] = 3;
        mapTilesPosition[2, 1, 1] = RIGHT;

        mapTilesPosition[1, 2, 0] = 3;
        mapTilesPosition[1, 2, 1] = UP;

        //Chamamos o loop para colocar as demais pecas
        while(!done)
        {
            fillMap();
        }

        //preenchemos os demais espacos
        fillEmptyPlaces();

        //Verifica se a quantidade de sugar solicitada foi colocada
        //Caso contrario subistitui algumas pecas por sugar
        placeRemainderSugar();

        //Movemos o quad para a posicao correta, cobrindo assim o mapa
        Transform quad = transform.parent.FindChild("MapSize").FindChild("Quad");
        //A posicao central do mapa sera em cada eixo igual a numero total de pecas aquele eixo vezes o total de pecas por tile (3)
        //vezes o tamanho de cada peca que compoem o tile (2), divido por 2
        quad.position = new Vector3((numberOfBigTilesX * 3 * 2)/2 - 1, 0, (numberOfBigTilesZ * 3 * 2)/2 - 5);
        quad.localScale = new Vector3(numberOfBigTilesX * 3 * 2, numberOfBigTilesZ * 3 * 2, 0);

    }

    private void placeRemainderSugar()
    {
        bool flag;
        while(totalOfSugar > 0)
        {
            flag = false;
            //Procuramos um BigTile no mapa partindo da estremidade final
            for(int x = numberOfBigTilesX - 1; x > 0; x = x - getRandom(0,4))
            {
                for(int z = numberOfBigTilesZ - 1; z > 0; z = z - getRandom(0,4))
                {
                    //Verificamos se o bigTiles atual e um caminho (nao 'e sugar, fullgrass e nem fullgrass2)
                    if(mapTilesPosition[x,z,1] == isAPath)
                    {       
                        flag = true;
                        //Removemos a peca
                        Destroy(mapTiles[x,z]);

                        //Criamos a nova peca como um Sugar
                        GameObject p = (GameObject)Instantiate(pieces[Sugar], new Vector3(x * 6, 0, z *6), Quaternion.identity);
                        p.transform.SetParent(transform);
                        mapTiles[x,z] = p;

                        mapTilesPosition[x,z,0] = -1;
                        mapTilesPosition[x,z,1] = 0;

                        totalOfSugar--;
                        break;
                    }
                }
                if(flag)
                    break;
            }
        }
    }

    private void fillEmptyPlaces()
    {
        int pieceNumber;
        for(int x = 1; x < numberOfBigTilesX - 1; x++)
        {
            for(int z = 1; z < numberOfBigTilesZ - 1; z++)
            {
                //Procuramos por locais com 0 e iremos completalos com fullgrass ou fullgrass2
                if(mapTilesPosition[x,z,0] == 0)
                {
                    pieceNumber = getRandom(0,2);
                    GameObject p = (GameObject)Instantiate(pieces[pieceNumber], new Vector3(x * 6, 0, z * 6), Quaternion.identity);
                    p.transform.SetParent(transform);
                    mapTiles[x,z] = p;

                    mapTilesPosition[x, z, 0] = -1;
                }
            }
        }
    }

    private void fillMap()
    {
        done = true;
        for(int x = 1; x < numberOfBigTilesX - 1; x++)
        {
            for(int z = 1; z < numberOfBigTilesZ - 1; z++)
            {
                //Procuramos uma posicao ja foi marcar como continuacao
                //Para uma peca anteriormente colocada
                //-1 locais com pecas ja colocadas, 0 locais sem pecas mas sem indicacao de pecas anteriores
                //outros locais com indicacao de pecas anteriores para formar caminhos
                if(mapTilesPosition[x,z,0] != 0 && mapTilesPosition[x,z,0] != -1)
                {
                    done = false;
                    int []possiblePieces = piecesRules(mapTilesPosition[x,z,0], mapTilesPosition[x,z,1]);
                    //Sorteamos uma peca
                    int random = getRandom(0, possiblePieces.Length);

                    //Caso a peca escolhida seja FullGrass ou FullGrass2, por serem pecas de terminacao
                    //E necessario verificar se ainda irao existir outros caminhos
                    if(possiblePieces[random] == FullGrass || possiblePieces[random] == FullGrass2)
                    {
                        if(!stillPaths(x,z))
                        {
                            //Caso nao existao mais caminhos a serem expandidos entao devemos trocar essa peca por outra
                            while(possiblePieces[random] == FullGrass || possiblePieces[random] == FullGrass2)
                            {
                                random = getRandom(0, possiblePieces.Length);
                            }
                        }
                    }

                    //Se for colocar um Sugar diminuimos a contagem 
                    if(possiblePieces[random] == Sugar)
                    {
                        //Caso ja tenha sido colocado o numero de sugar solicitado devemos entao trocar a peca selecionada
                        if(totalOfSugar == 0)
                        {  
                            while(possiblePieces[random] == Sugar || possiblePieces[random] == FullGrass || possiblePieces[random] == FullGrass2)
                            {
                                random = getRandom(0, possiblePieces.Length);
                            }
                        }
                        else
                        {
                            totalOfSugar--;
                        }
                    }

                    surroundings.calcSurroundings(x, z);
                    //Por fim antes de criar a peca devemos checar se essa peca ira criar algum caminho ao ser colocada
                    //Ignoramos pecas fullgrass pois elas sempre nao irao criar caminhos
                    if(possiblePieces[random] != FullGrass || possiblePieces[random] != FullGrass2)
                    {
                        if(x < numberOfBigTilesX - 3 && z < numberOfBigTilesZ - 3)
                        {
                            int controll = 0;
                            while(!checkPath(possiblePieces[random]) && controll < 20)
                            {
                                //Se caso entrou no while e porque a peca que iria ser colocada nao ira gerar caminhos
                                //Logo devemos troca-la por outra
                                //Se entrou aqui e pq nao era um fullGrass logo devemos evitar q uma peca fullgrass seja
                                //selecionada para isso pegamos um random entre 2 e possiblepieces.length
                                while(possiblePieces[random] == FullGrass || possiblePieces[random] == FullGrass2 || possiblePieces[random] == Sugar)
                                    random = getRandom(0, possiblePieces.Length);   
                                controll++;
                            }
                            if(controll == 20)
                                random = 0;
                        }
                    }

                    //Criamos a peca
                    GameObject p = (GameObject)Instantiate(pieces[possiblePieces[random]], new Vector3(x * 6, 0, z *6), Quaternion.identity);
                    p.transform.SetParent(transform);
                    mapTiles[x,z] = p;

                    //Marcamos essa posicao do mapa como ocupada
                    mapTilesPosition[x,z,0] = -1; 

                    //Caso a peca colocada seja um Sugar marcamos essa posicao como ocupada (-1) 
                    //e marcamos o sentido com o valor Sugar para uso futuro
                    if(possiblePieces[random] != Sugar && possiblePieces[random] != FullGrass && possiblePieces[random] != FullGrass2)
                        mapTilesPosition[x,z,1] = isAPath;


                    //Chamamos alguem aqui que ira marcar as posicoes do mapa que podem receber pecas agora que esta peca foi colocada 
                    //Marca os arredores da peca
                    markPath(x, z, possiblePieces[random]);

                    /*
                    print("***********");
                    for(int i = 0; i < numberOfTilesZ; i++)
                    {
                        string aux = "";
                        for(int j = 0; j < numberOfTilesX; j++)
                        {
                            aux = aux + mapTiles[j,i,0] + " ";
                        }
                        print(aux);
                    }
                    print("***********");
                    */

                    return;
                }
            }
        }
    }

    //Procura na matrix mapTiles se ainda existe marcacoes para proximas pecas
    private bool stillPaths(int currX, int currZ)
    {
        for(int x = currX; x < numberOfBigTilesX - 1; x++)
        {
            for(int z = currZ; z < numberOfBigTilesZ - 1; z++)
            {
                if(mapTilesPosition[x,z,0] != 0 && mapTilesPosition[x,z,0] != -1)
                    return true;
            }
        }
        return false;
    }

    //Retorna os tipos de pecas que podem ser usadas 
    //dado o numero da peca
    private int[] piecesRules(int pieceNumber, int direction)
    {
        int[] possiblePieces = new int[15];
        switch(pieceNumber)
        {
            case Empty:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                else if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                else
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                break;
            case Anthill:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                else if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                else
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                break;
            case LeftT:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                else if(direction == DOWN)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                break;
            case LineH:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == LEFT)
                {
                    possiblePieces = new int[11] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, RightT, Sugar, UpperRight, UpperT};
                }
                break;
            case LineV:
                if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                else if(direction == DOWN)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                break;
            case LowerLeft:
                if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                else if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                break;
            case LowerRight:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                break;
            case LowerT:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                else if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                break;
            case RightT:
                if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                else if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                else if(direction == DOWN)
                {
                    possiblePieces = new int[12] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                break;
            case Sugar:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == UP)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, RightT, Sugar, UpperLeft, UpperRight, UpperT};
                }
                else if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                else
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                break;
            case UpperLeft:
                if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                else if(direction == DOWN)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                break;
                //FullGrass FullGrass2 Empty LeftT LineH LineV LowerLeft LowerRight LowerT RightT Sugar UpperLeft UpperRight UpperT
            case UpperRight:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == DOWN)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                break;
            case UpperT:
                if(direction == RIGHT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LineH, LowerLeft, LowerT, RightT, Sugar, UpperLeft, UpperT};
                }
                else if(direction == DOWN)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineV, LowerLeft, LowerRight, LowerT, RightT, Sugar};
                }
                else if(direction == LEFT)
                {
                    possiblePieces = new int[10] {FullGrass, FullGrass2, Empty, LeftT, LineH, LowerRight, LowerT, Sugar, UpperRight, UpperT};
                }
                break;
            default:
                possiblePieces = new int[15];
                break;
        }
        return possiblePieces;
    }

    //Verifica se a peca que se pretende colocar ira gerar pelo menos 1 caminho ao redor dela
    private bool checkPath(int pieceNumber)
    {
        //So nao checamos para o caso de pecas fullGrass
        switch(pieceNumber)
        {
            case Empty: //Empty - marca-se todos os lados
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 4; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case Anthill: //Anthill - marca-se todos os lados
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 4; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case LeftT: //LeftT - marca-se somente RIGHT, UP, DOWN
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 4; i++)
                {
                    if(i != 2)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            return true;
                        }
                    }
                }
                break;
            case LineH: // LineH - marca-se somente RIGHT, LEFT
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 3; i++)
                {
                    if(i != 1)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                return true;
                            }
                        }
                    }
                }
                break;
            case LineV: // LineV - marca-se somente UP, DOWN
                //Vamos macar os arredores da peca com seu numero
                for(int i = 1; i < 4; i++)
                {
                    if(i != 2)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                return true;
                            }
                        }
                    }
                }
                break;
            case LowerLeft: // LowerLeft - marca-se somente UP, LEFT
                //Vamos macar os arredores da peca com seu numero
                for(int i = 1; i < 3; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case LowerRight: // LowerRight - marca-se somente UP, RIGHT
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 3; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case LowerT: // LowerT - marca-se somente UP, RIGHT, LEFT
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 3; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case RightT: // RightT - marca-se somente UP, DOWN, LEFT
                //Vamos macar os arredores da peca com seu numero
                for(int i = 1; i < 4; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case Sugar: // Sugar - marca-se todas as direcoes
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 4; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case UpperLeft: // UpperLeft - marca-se somente LEFT, DOWN
                //Vamos macar os arredores da peca com seu numero
                for(int i = 2; i < 4; i++)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case UpperRight: // UpperRight - marca-se somente RIGHT, DOWN
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 4; i = i + 3)
                {
                    //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                    if(surroundings.surroundingsMap[i,0] != -100)
                    {
                        //Verificamos se aquela posicao do mapa ja nao esta preenchida
                        if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                        {
                            return true;
                        }
                    }
                }
                break;
            case UpperT: // UpperT - marca-se somente RIGHT, LEFT, DOWN
                //Vamos macar os arredores da peca com seu numero
                for(int i = 0; i < 4; i++)
                {
                    if(i != 1)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                return true;
                            }
                        }
                    }
                }
                break;    
        }                      
        return false;
    }

    //Marca as posicao ao redor desta peca que podem continuar o caminho
    private void markPath(int currX, int currZ, int pieceNumber)
    {
        //Verifica se nao e uma peca FullGrass pois se for nao marcamos nada ao redor dela pois ela e uma terminacao
        if(pieceNumber >= 2){
            switch(pieceNumber){
                case Empty: //Empty - marca-se todos os lados
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 4; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case Anthill: //Anthill - marca-se todos os lados
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 4; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case LeftT: //LeftT - marca-se somente RIGHT, UP, DOWN
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 4; i++)
                    {
                        if(i != 2)
                        {
                            //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                            if(surroundings.surroundingsMap[i,0] != -100)
                            {
                                //Verificamos se aquela posicao do mapa ja nao esta preenchida
                                if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                                {
                                    //Marcamos no mapa o numero da peca
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                    //E marcamos sua direcao em relacao a anterior
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                                }
                            }
                        }
                    }
                    break;
                case LineH: // LineH - marca-se somente RIGHT, LEFT
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 3; i++)
                    {
                        if(i != 1)
                        {
                            //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                            if(surroundings.surroundingsMap[i,0] != -100)
                            {
                                //Verificamos se aquela posicao do mapa ja nao esta preenchida
                                if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                                {
                                    //Marcamos no mapa o numero da peca
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                    //E marcamos sua direcao em relacao a anterior
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                                }
                            }
                        }
                    }
                    break;
                case LineV: // LineV - marca-se somente UP, DOWN
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 1; i < 4; i++)
                    {
                        if(i != 2)
                        {
                            //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                            if(surroundings.surroundingsMap[i,0] != -100)
                            {
                                //Verificamos se aquela posicao do mapa ja nao esta preenchida
                                if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                                {
                                    //Marcamos no mapa o numero da peca
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                    //E marcamos sua direcao em relacao a anterior
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                                }
                            }
                        }
                    }
                    break;
                case LowerLeft: // LowerLeft - marca-se somente UP, LEFT
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 1; i < 3; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case LowerRight: // LowerRight - marca-se somente UP, RIGHT
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 2; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case LowerT: // LowerT - marca-se somente UP, RIGHT, LEFT
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 3; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case RightT: // RightT - marca-se somente UP, DOWN, LEFT
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 1; i < 4; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case Sugar: // Sugar - marca-se todas as direcoes
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 4; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case UpperLeft: // UpperLeft - marca-se somente LEFT, DOWN
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 2; i < 4; i++)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case UpperRight: // UpperRight - marca-se somente RIGHT, DOWN
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 4; i = i + 3)
                    {
                        //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                        if(surroundings.surroundingsMap[i,0] != -100)
                        {
                            //Verificamos se aquela posicao do mapa ja nao esta preenchida
                            if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                            {
                                //Marcamos no mapa o numero da peca
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                //E marcamos sua direcao em relacao a anterior
                                mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                            }
                        }
                    }
                    break;
                case UpperT: // UpperT - marca-se somente RIGHT, LEFT, DOWN
                    //Vamos macar os arredores da peca com seu numero
                    for(int i = 0; i < 4; i++)
                    {
                        if(i != 1)
                        {
                            //-100 indica que aquela posicao nao pode ser marcada pois esta na beirada do mapa e ja foi preenchida com a borda
                            if(surroundings.surroundingsMap[i,0] != -100)
                            {
                                //Verificamos se aquela posicao do mapa ja nao esta preenchida
                                if(mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1], 0] != -1)
                                {
                                    //Marcamos no mapa o numero da peca
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],0] = pieceNumber;
                                    //E marcamos sua direcao em relacao a anterior
                                    mapTilesPosition[surroundings.surroundingsMap[i, 0], surroundings.surroundingsMap[i, 1],1] = i;
                                }
                            }
                        }
                    }
                    break;    
            }                      
        }
    }
       
    
    private int getRandom(int from, int to)
    {
        int value;
        float valueF = (Random.Range(from, to));
        //print(valueF);
        value = (int) valueF;
        //if((valueF - value) > 0.5f)
         //   value++;
        //print(value);
        return value;
    }

    private void fillEdges()
    {
        int pieceNumber;
        //Certa o mapa com blocos que nao permitem o player ir para as pontas
        for(int i = 0; i < numberOfBigTilesX; i++)
        {
            pieceNumber = getRandom(0,2);
            GameObject p = (GameObject)Instantiate(pieces[pieceNumber], new Vector3(i * 6, 0, 0), Quaternion.identity);
            p.transform.SetParent(transform);
            mapTiles[i,0] = p;

            mapTilesPosition[i, 0, 0] = -1;
        }
        
        for(int i = 0; i < numberOfBigTilesX; i++)
        {   
            pieceNumber = getRandom(0,2);
            GameObject p = (GameObject)Instantiate(pieces[pieceNumber], new Vector3(i * 6, 0, (numberOfBigTilesZ - 1) * 6), Quaternion.identity);
            p.transform.SetParent(transform);
            mapTiles[i,numberOfBigTilesZ - 1] = p;

            mapTilesPosition[i, numberOfBigTilesZ - 1, 0] = -1;
        }
        
        for(int i = 1; i < numberOfBigTilesZ - 1; i++)
        {
            pieceNumber = getRandom(0,2);
            GameObject p = (GameObject)Instantiate(pieces[pieceNumber], new Vector3(0, 0, i * 6), Quaternion.identity);
            p.transform.SetParent(transform);
            mapTiles[0,i] = p;

            mapTilesPosition[0, i, 0] = -1;
        }
        
        for(int i = 1; i < numberOfBigTilesZ - 1; i++)
        {
            pieceNumber = getRandom(0,2);
            GameObject p = (GameObject)Instantiate(pieces[pieceNumber], new Vector3((numberOfBigTilesX - 1) * 6, 0, i * 6), Quaternion.identity);
            p.transform.SetParent(transform);
            mapTiles[numberOfBigTilesX - 1,i] = p;

            mapTilesPosition[numberOfBigTilesX - 1, i, 0] = -1;
        }
    }
   
    private class Surroundings {
        //Vetor no sentido antihorario
        private const int Right = 0;
        private const int Up = 1;
        private const int Left = 2;
        private const int Down = 3;
        private const int X = 0;
        private const int Z = 1;

        private int xLowLimit;
        private int xHighLimit;
        private int zLowLimit;
        private int zHighLimit;



        public int [,] surroundingsMap;

        public Surroundings(int mapXSize, int mapZSize)
        {
            xLowLimit = 0;
            xHighLimit = mapXSize - 1;
            zLowLimit = 0;
            zHighLimit = mapZSize - 1;

            surroundingsMap = new int[4,2];
        }

        public void calcSurroundings(int currX, int currZ)
        {
            if(currX + 1 == xHighLimit)
            {
                surroundingsMap[Right,X] = -100;
                surroundingsMap[Right,Z] = -100;
            }
            else
            {
                surroundingsMap[Right,X] = currX + 1;
                surroundingsMap[Right,Z] = currZ;
            }

            if( currZ + 1 == zHighLimit)
            {
                surroundingsMap[Up,X] = -100;
                surroundingsMap[Up,Z] = -100;
            }
            else
            {
                surroundingsMap[Up,X] = currX;
                surroundingsMap[Up,Z] = currZ + 1;
            }

            if(currX - 1 == xLowLimit)
            {
                surroundingsMap[Left,X] = -100;
                surroundingsMap[Left,Z] = -100;
            }
            else
            {
                surroundingsMap[Left,X] = currX - 1;
                surroundingsMap[Left,Z] = currZ;
            }

            if(currZ - 1 == zLowLimit)
            {
                surroundingsMap[Down,X] = -100;
                surroundingsMap[Down,Z] = -100;
            }
            else
            {
                surroundingsMap[Down,X] = currX;
                surroundingsMap[Down,Z] = currZ - 1;
            }
        }
    }

    //Retorna um vetor que aponta para o tiles de sugar colocados no jogo
    public GameObject[] getSugarTiles()
    {
        GameObject[] sugarTiles = new GameObject[numberOfSugarTiles];
        int i = 0;

        for(int x = 1; x < numberOfBigTilesX - 1; x++)
        {
            for(int z = 1; z < numberOfBigTilesZ - 1; z++)
            {
                if(mapTiles[x,z].name.Equals("Sugar(Clone)"))
                {
                    sugarTiles[i] = mapTiles[x,z];
                    i++;
                }
            }
        }
        return sugarTiles;
    }
}
