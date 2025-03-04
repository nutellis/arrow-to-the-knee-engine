using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks; 

//To Do:
/*
 * 1- need to get the display and make it to a grid
 * 2- need to check there is other game objects in each tile
 * 3- need to impelement the A* to find the shortest path
 * 
 */

namespace Shard
{
    public class PathTracer
    {
        //variables

        private static readonly int[][] Directions =
{
                new int[] { 0, 1 },  // Right
                new int[] { 1, 0 },  // Down
                new int[] { 0, -1 }, // Left
                new int[] { -1, 0 }  // Up
        };
        private int displayWidth = Bootstrap.getDisplay().getWidth();
        private int displayHeight = Bootstrap.getDisplay().getHeight();
        private int nodeWidth = 16; // deault value
        private int nodeHeight = 16; // deault value
        private int[,] grid;
        private List<Node> path;
        private Node[,] nodeMap;

        // Node class might later put it in a different file
        public class Node
        {
            public int minX, minY, maxX, maxY;
            public int PosX, posY;
            public int nodeWidth, nodeHeight;
            public bool walkable = true; // default value
            public struct isFilled()
            {
                public int row, column;
                
            }
            public List<isFilled> filled = new List<isFilled>();

            public int G, H;
            public Node Parent;
            public int F => G + H;

            public Node(int coordinateX, int coordinateY, Node parent = null)
            {
                PosX = coordinateX;
                posY = coordinateY;
                Parent = parent;
                G = parent != null ? parent.G + 1 : 0;
                H = 0;
            }

            public void setBool(bool isWalkable)
            {
                walkable = isWalkable;
            }
            public void setNodeInfo(int minX, int minY, int nodeWidth, int nodeHeight)
            {
                this.minX = minX;
                this.minY = minY;
                this.maxX = minX+nodeWidth-1;
                this.maxY = minY+nodeHeight-1;
            }
            public void setFilled(int row, int col)
            {
                isFilled temp = new isFilled();
                temp.row = row;
                temp.column = col;
                filled.Add(temp);
            }
        }


        public void setGrid()
        {
            grid = new int[displayHeight, displayWidth];
            for (int i = 0; i < displayHeight; i++)
            {
                for (int j = 0; j < displayWidth; j++)
                {
                    grid[i, j] = 0;
                }
            }
        }
       public void transformGameObjectsToGrid()
        {
            // Assuming Bootstrap.getGameObjects() returns a list of game objects with X and Y properties  
            List<GameObject> gameObjects = GameObjectManager.getInstance().getMyObject();
            foreach (var gameObject in gameObjects)
            {
                int x = (int)gameObject.Transform.X;
                int y = (int)gameObject.Transform.Y;
                grid[x, y] = 1;
            }
        }

        public void transformWorldToGrid()
        {
            setGrid();
            transformGameObjectsToGrid();
        }

        public void setNodeMap()
        {
            nodeMap = new Node[displayHeight / nodeHeight, displayWidth / nodeWidth];

        }
        // A little bit of a mess, works best in squere shapes, need to be checked for other shapes
        public void transformGridToNodeMap()
        {
            int posX = 0, posY = 0;
            int coordianteX = 0, coordinateY = 0;

            setNodeMap();

            for (int rowCounter = 0; rowCounter < displayHeight / nodeHeight; rowCounter++)
            {
                posX = rowCounter;

                for(int colCounter = 0; colCounter < displayWidth / nodeWidth; colCounter++)
                {
                    posY = colCounter;
                    coordianteX = rowCounter * nodeHeight;
                    coordinateY = colCounter * nodeWidth;
                    Node node = new Node(posX, posY);
                    node.setNodeInfo(coordianteX, coordinateY, nodeWidth, nodeHeight);
                    nodeMap[rowCounter, colCounter] = node;

                    for(int i = node.minX; i <= node.maxX; i++)
                    {
                        for (int j = node.minY; j <= node.maxY; j++)
                        {
                            if (grid[i, j] == 1)
                            {
                                node.setBool(false);
                                node.setFilled(i, j);
                            }
                        }
                    }
                }


            }
        }

        // this need to be fixed probably
        public void transformPathToGrid()
        {
            foreach (var node in path)
            {
                for (int i = node.minX; i <= node.maxX; i++)
                {
                    for (int j = node.minY; j <= node.maxY; j++)
                    {
                        grid[i, j] = 2;
                    }
                }
            }
        }





        public List<Node> FindPath((int, int) start, (int, int) goal)
        {
            int startX = start.Item1 / nodeHeight;
            int startY = start.Item2 / nodeWidth;
            int goalX = goal.Item1 / nodeHeight;
            int goalY = goal.Item2 / nodeWidth;

            List<Node> openList = new List<Node>();
            HashSet<(int, int)> closedList = new HashSet<(int, int)>();

            // I hard coded the the transformation of the start and goal to the nodeMap
            // I need to fix this later

            Node startNode = nodeMap[startX, startY];
            Node goalNode = nodeMap[goalX , goalY];
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node current = openList.OrderBy(n => n.F).First();
                if (current.PosX == goalNode.PosX && current.posY == goalNode.posY)
                    return ReconstructPath(current);

                openList.Remove(current);
                closedList.Add((current.PosX, current.posY));

                foreach (var direction in Directions)
                {
                    int newX = current.PosX + direction[0], newY = current.posY + direction[1];
                    if (newX < 0 || newY < 0 || newX >= nodeMap.GetLength(0) || newY >= nodeMap.GetLength(1) || !nodeMap[newX, newY].walkable || closedList.Contains((newX, newY)))
                        continue;

                    Node neighbor = nodeMap[newX, newY];
                    neighbor.Parent = current;
                    neighbor.G = current.G + 1;
                    neighbor.H = Math.Abs(newX - goalNode.PosX) + Math.Abs(newY - goalNode.posY);

                    if (openList.Any(n => n.PosX == neighbor.PosX && n.posY == neighbor.posY && n.G <= neighbor.G))
                        continue;

                    openList.Add(neighbor);
                }
            }
            return new List<Node>();
        }

        private static List<Node> ReconstructPath(Node node)
        {
            List<Node> path = new List<Node>();
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }

        public void setNodeWidth(int width)
        {
            nodeWidth = width;
        }
        public void setNodeHeight(int height)
        {
            nodeHeight = height;
        }
        public void setPath(List<Node> path)
        {
            this.path = path;
        }

        public void printGrid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j]);
                }
                Console.WriteLine();
            }
        }
        public void printNodeMap()
        {
            for (int i = 0; i < nodeMap.GetLength(0); i++)
            {
                for (int j = 0; j < nodeMap.GetLength(1); j++)
                {
                    if(nodeMap[i, j].walkable == false)
                    {
                        Console.Write(0);
                    }
                    else
                    {
                        Console.Write(1);
                    }

                }
                Console.WriteLine();
            }
        }
        public void printPath()
        {
            foreach (var node in path)
            {
                Console.WriteLine(node.PosX + " " + node.posY);
            }
        }

        public void testRun(int nodeWidth, int nodeHeight, (int,int) start, (int,int) goal)
        {
            setNodeWidth(nodeWidth);
            setNodeHeight(nodeHeight);
            transformWorldToGrid();
            transformGridToNodeMap();
            path = FindPath(start, goal);
            transformPathToGrid();
            printNodeMap();
            //printGrid();
            printPath();

        }



    }
}
