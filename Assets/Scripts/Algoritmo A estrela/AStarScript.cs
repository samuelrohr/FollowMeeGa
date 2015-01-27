using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarScript : MonoBehaviour
{   
    // Cria um mapa dummy
    private static bool[,] CreateDummyMap()
    {
        bool[,] map = new bool[7, 5];
        
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                map [i, j] = true;
            }
        }
        // Paredes
        map [3, 1] = false;
        map [3, 2] = false;
        map [3, 3] = false;
        map [3, 4] = false;
        
        return map;
    }

    /**
     * Armazena a posição de um tile no grid.
     */
    public class TilePosition
    {
        
        private int x;
        private int y;
        
        public TilePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /**
         * Coluna
         */
        public int X()
        {
            return x;
        }

        /**
         * Linha
         */
        public int Y()
        {
            return y;
        }
        
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            TilePosition tilePosition = (TilePosition)obj;
            if (tilePosition == null)
            {
                return false;
            }
            if (tilePosition.x == this.x && tilePosition.y == this.y)
            {
                return true;
            }
            return false;
        }
        
        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
        
    }

    /**
     * Classe responsável por realizar a busca do melhor caminho em uma grid 2D utilizando
     * o algoritmo A*.
     * 
     * Para obter o melhor caminho deve ser utilizado o método estático GetBestPath, onde
     * é informado o estado corrente dos tiles (matriz de bool onde false indica tiles
     * proibidos e true os tiles já visitados).
     * 
     * O retorno do algoritmo é uma pilha de TilePosition. Se a pilha for vazia, ou o tile
     * de início é o tile de destino, ou não há um caminho possível.
     */
    public class AStarAlgorithm
    {
        
        // Representa um nó do algoritmo A*
        private class Node
        {
            
            private Node parent;
            private TilePosition position;
            private int H;
            private int G;
            private int F;
            
            // Inicializa o node raiz
            public Node(TilePosition position)
            {
                parent = null;
                this.position = position;
                H = 0;
                G = 0;
                F = 0;
            }
            
            // Inicializa os nodes posteriores, posição do objetivo utilizada para calcular métricas
            public Node(TilePosition position, Node parent, TilePosition goalPosition)
            {
                this.parent = parent;
                this.position = position;
                InitializeCosts(goalPosition);
            }
            
            // Inicializa os custos com base na posição do objetivo
            private void InitializeCosts(TilePosition goalPosition)
            {
                // Computamos o valor de G
                G = parent.G;
                // Checamos se movemos na diagonal ou não
                if (position.X() == parent.position.X() || position.Y() == parent.position.Y())
                {
                    G += 10;
                } else
                {
                    G += 14;
                }
                
                // Computamos o valor de H
                H = Mathf.Abs(goalPosition.X() - position.X()) * 10 + Mathf.Abs(goalPosition.Y() - position.Y()) * 10;
                
                // Computamos o valor de F
                F = G + H;
            }
            
            public TilePosition GetPosition()
            {
                return position;
            }
            
            public int GetF()
            {
                return F;
            }
            
            public int GetG()
            {
                return G;
            }
            
            public Node GetParent()
            {
                return parent;
            }
            
            public override bool Equals(System.Object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                
                Node node = obj as Node;
                if (node == null)
                {
                    return false;
                }
                if (node.position.Equals(position))
                {
                    return true;
                }
                return false;
            }
            
            public override string ToString()
            {
                return "Node Position: " + position;
            }
            
        }

        /**
         * Método responsável por dar acesso ao algoritmo A*, calculando o melhor caminho entre os pontos iniciais e finais.
         */
        public static Stack<TilePosition> GetBestPath(bool[,] map, TilePosition initial, TilePosition goal)
        {
            AStarAlgorithm aStar = new AStarAlgorithm(map, initial, goal);
            return aStar.execute();
        }
        
        private TilePosition goalPosition;
        private TilePosition initialPosition;
        private List<Node> open;
        private List<TilePosition> closed;
        private bool[,] map;

        private AStarAlgorithm(bool[,] map, TilePosition initial, TilePosition goal)
        {
            // Salvamos o mapa e as posições
            goalPosition = goal;
            initialPosition = initial;
            this.map = map;
        }

        private AStarAlgorithm()
        {
        }
        
        // Método responsável pela lógica do algoritmo e que devolve a pilha com o caminho
        private Stack<TilePosition> execute()
        {
            // Inicializamos as listas
            open = new List<Node>();
            closed = new List<TilePosition>();
            // Pegamos as dimensões do mapa
            int maxColumn = map.GetLength(0);
            int maxRow = map.GetLength(1);
            
            // 1. Adicionamos o nó inicial na lista de nós abertos
            open.Add(new Node(initialPosition));
            
            // 2. Repetimos
            Node targetNode = null;
            while (open.Count != 0)
            {
                // a. Obtemos o nó com menor custo F na lista de nós abertos
                Node currentNode = GetLowestFCostNode();
                
                // b. Adicionamos a informação do nó na lista de fechados
                TilePosition currentPosition = currentNode.GetPosition();
                closed.Add(currentPosition);
                
                // c. Checamos se encontramos o objetivo
                if (currentPosition.Equals(goalPosition))
                {
                    targetNode = currentNode;
                    // Abortamos o laço
                    break;
                }
                
                // d. Para os até 8 quadrados adjacentes
                for (int i = -1; i <= 1; i++)
                {
                    int column = currentPosition.X() + i;
                    for (int j = -1; j <= 1; j++)
                    {
                        int row = currentPosition.Y() + j;
                        TilePosition tempPosition = new TilePosition(column, row);
                        // Checamos se é um par de coordenadas válidas
                        if ((!currentPosition.Equals(tempPosition))
                            && (column >= 0 && column < maxColumn)
                            && (row >= 0 && row < maxRow))
                        {
                            // i.   Se o tile não puder ser caminhado ou está na lista de fechados, ignoramos
                            // ii.  Se não estiver na lista de abertos, adicionamos na lista
                            // iii. Se estiver na lista de abertos, checamos se o caminho até o tile é melhor, se sim atualizamos os valores
                            if (!map [column, row] || closed.Contains(tempPosition))
                            {
                            } else
                            {
                                Node tempNode = new Node(tempPosition, currentNode, goalPosition);
                                int indexInList = open.IndexOf(tempNode);
                                if (indexInList == -1)
                                {
                                    AddNodeToOpen(tempNode);
                                } else
                                {
                                    // Recuperamos o nó da lista e comparamos o G
                                    Node listNode = open [indexInList];
                                    if (listNode.GetG() > tempNode.GetG())
                                    {
                                        // Substituimos os nós
                                        open.RemoveAt(indexInList);
                                        AddNodeToOpen(tempNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Devolvemos a solução encontrada
            Stack<TilePosition> stack = new Stack<TilePosition>();
            // Buscamos os tiles
            if (targetNode != null)
            {
                Node temp = targetNode;
                while (temp != null)
                {
                    stack.Push(temp.GetPosition());
                    temp = temp.GetParent();
                }
            }
            return stack;
        }
        
        private Node GetLowestFCostNode()
        {
            Node tempNode = open [0];
            open.RemoveAt(0);
            return tempNode;
        }
        
        private void AddNodeToOpen(Node node)
        {
            // Procuramos o local onde devemos inserir o nó com base no custo 
            int F = node.GetF();
            
            for (int i = 0; i < open.Count; i++)
            {
                if (F <= open[i].GetF())
                {
                    // Inserimos e retornamos
                    open.Insert(i, node);
                    return;
                }
            }
            // Caso não tenhamos encontrado algum nó com F maior, simplesmente adicionamos
            open.Add(node);
        }
        
    }

}