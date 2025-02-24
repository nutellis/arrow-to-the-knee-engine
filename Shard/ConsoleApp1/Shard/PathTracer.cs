using System;
using System.Collections.Generic;
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

        // Node class might later put it in a different file
        public class Node
        {
            public int X, Y;
            public int G, H;
            public Node Parent;
            public int F => G + H;
            public bool isWalkable => true;

            public Node(int x, int y, Node parent = null)
            {
                X = x;
                Y = y;
                Parent = parent;
                G = parent != null ? parent.G + 1 : 0;
                H = 0;
            }

            public void setBool(bool isWalkable)
            {
                if (isWalkable == false)
                {
                    isWalkable = false;
                }
            }
        }

        public class Tile
        {
            public int minX, minY, maxX, maxY; 
        }

        private int displayWidth = Bootstrap.getDisplay().getWidth();
        private int displayHeight = Bootstrap.getDisplay().getHeight();
        private int tileWidth = 16;
        private int tileHeight = 16;
        private int[,] grid;
        private List<Node> path;

        public void setGrid()
        {
            grid = new int[displayWidth, displayHeight];
        }
       
        
        public void transformWorldToGrid()
        {
            // Assuming Bootstrap.getGameObjects() returns a list of game objects with X and Y properties  
            List<GameObject> gameObjects = GameObjectManager.getInstance().getMyObject();
            int gridWidth = displayWidth / tileWidth;
            int gridHeight = displayHeight / tileHeight;
            grid = new int[gridWidth, gridHeight];

            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    grid[i, j] = 0;
                }
            }

            foreach (var gameObject in gameObjects)
            {
                int x = (int)gameObject.Transform.X / tileWidth;
                int y = (int)gameObject.Transform.Y / tileHeight;
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                {
                    grid[x, y] = 1;
                }
            }
        }






        public static List<Node> FindPath(int[,] grid, (int, int) start, (int, int) goal)
        {
            int width = grid.GetLength(0), height = grid.GetLength(1);
            List<Node> openList = new List<Node>();
            HashSet<(int, int)> closedList = new HashSet<(int, int)>();

            Node startNode = new Node(start.Item1, start.Item2);
            Node goalNode = new Node(goal.Item1, goal.Item2);
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node current = openList.OrderBy(n => n.F).First();
                if (current.X == goalNode.X && current.Y == goalNode.Y)
                    return ReconstructPath(current);

                openList.Remove(current);
                closedList.Add((current.X, current.Y));

                foreach (var direction in Directions)
                {
                    int newX = current.X + direction[0], newY = current.Y + direction[1];
                    if (newX < 0 || newY < 0 || newX >= width || newY >= height || grid[newX, newY] == 1 || closedList.Contains((newX, newY)))
                        continue;

                    Node neighbor = new Node(newX, newY, current);
                    neighbor.H = Math.Abs(newX - goalNode.X) + Math.Abs(newY - goalNode.Y);

                    if (openList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y && n.G <= neighbor.G))
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
    }
}
