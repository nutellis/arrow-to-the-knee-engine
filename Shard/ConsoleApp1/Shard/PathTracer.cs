using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

//To Do:
/*
 * 1- Do not use grid , use the game objects to create the node system ##Done
 * 2- Make meaningful fucntions for the path tracer #inProgress
 * 3- Make the path tracer work with the game objects ##Done
 * 4- Make the class to singleton ##Done
 * 
 */

namespace Shard
{
    public class PathTracer
    {
        private static PathTracer instance = null;

        private PathTracer()
        {
            setNodeWidth(nodeWidth);
            setNodeHeight(nodeHeight);
        }

        public static PathTracer getInstance()
        {
            if (instance == null)
            {
                instance = new PathTracer();
            }
            return instance;
        }

        private static readonly int[][] Directions =
        {
                new int[] { 0, 1 },  // Right
                new int[] { 1, 0 },  // Down
                new int[] { 0, -1 }, // Left
                new int[] { -1, 0 },  // Up
                new int [] { 1, 1 },  // Down Right
                new int [] { 1, -1 }, // Down Left
                new int [] { -1, 1 }, // Up Right
                new int [] { -1, -1 } // Up Left
            };
        private int displayWidth = Bootstrap.getDisplay().getWidth();
        private int displayHeight = Bootstrap.getDisplay().getHeight();
        private int nodeWidth = 16; // deault value
        private int nodeHeight = 16; // deault value
        private int[,] grid;
        private List<Node> path;
        public List<Node> temp = new List<Node>();
        private Node[,] nodeMap;
        private List<string> excludedTags;
        private GameObject owner = new GameObject();


        // Node class might later put it in a different file
        public class Node
        {
            public int minX, minY, maxX, maxY;
            public int posX, posY;
            public int centerX, centerY;    
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

            public Transform transform
            {
                get
                {
                    Transform t = new Transform();
                    t.X = posX * nodeWidth;
                    t.Y = posY * nodeHeight;
                    return t;
                }
            }

            public Node(int coordinateX, int coordinateY, int width, int height, Node parent = null)
            {
                nodeWidth = width;
                nodeHeight = height;
                posX = coordinateX;
                posY = coordinateY;
                Parent = parent;
                G = parent != null ? parent.G + 1 : 0;
                H = 0;
            }

            public void setWalkable(bool isWalkable)
            {
                walkable = isWalkable;
            }
            public void setNodeInfo(int minX, int minY, int nodeWidth, int nodeHeight)
            {
                this.minX = minX;
                this.minY = minY;
                this.maxX = minX + nodeWidth - 1;
                this.maxY = minY + nodeHeight - 1;
                this.centerX = ((minX + maxX) / 2) + 1;
                this.centerY = ((minY + maxY) / 2) + 1;
            }
            public void setFilled(int row, int col)
            {
                isFilled temp = new isFilled();
                temp.row = row;
                temp.column = col;
                filled.Add(temp);
            }
        }
        
        public void setNodeMap(GameObject gameObject)
        {
            int numNodesX = 30;  
            int numNodesY = 30; 

            int objectNodeX = (int)(gameObject.transform.X / nodeWidth);
            int objectNodeY = (int)(gameObject.transform.Y / nodeHeight);

            // Define the bounds of the node map
            int startX = Math.Max(objectNodeX - numNodesX / 2, 0);
            int startY = Math.Max(objectNodeY - numNodesY / 2, 0);
            int endX = Math.Min(objectNodeX + numNodesX / 2, displayWidth / nodeWidth - 1);
            int endY = Math.Min(objectNodeY + numNodesY / 2, displayHeight / nodeHeight - 1);

            // Create the node map for this limited region
            nodeMap = new Node[endX - startX + 1, endY - startY + 1];

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    nodeMap[i - startX, j - startY] = new Node(i, j, nodeWidth, nodeHeight);
                }
            }
        }

        public bool CheckExcludedTags(string tag)
        {
            if (excludedTags.Contains(tag))
            {
                return true;
            }
            return false;
        }
        public void setGameObjectsToNodeMap(GameObject ownerObject)
        {
            // Assuming Bootstrap.getGameObjects() returns a list of game objects with X and Y properties  
            List<GameObject> gameObjects = GameObjectManager.getInstance().getMyObject();

            int nodeMapWidth = nodeMap.GetLength(0);
            int nodeMapHeight = nodeMap.GetLength(1);

            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Tags.checkTag("Ai") || gameObject.Tags.checkTag("ignore"))
                {
                    continue;
                }

                //convert object bounds to node coordinates
                int minNodeX = Math.Max((int)(gameObject.transform.X / nodeWidth), 0);
                int minNodeY = Math.Max((int)(gameObject.transform.Y / nodeHeight), 0);
                int maxNodeX = Math.Min((int)((gameObject.transform.X + gameObject.transform.Wid) / nodeWidth), nodeMapWidth - 1);
                int maxNodeY = Math.Min((int)((gameObject.transform.Y + gameObject.transform.Ht) / nodeHeight), nodeMapHeight - 1);

                // skip this object if it's completely outside the node map
                if (minNodeX > nodeMapWidth - 1 || minNodeY > nodeMapHeight - 1 || maxNodeX < 0 || maxNodeY < 0)
                {
                    continue;
                }

                // iterate within the object’s node-space bounds
                for (int i = minNodeX; i <= maxNodeX; i++)
                {
                    for (int j = minNodeY; j <= maxNodeY; j++)
                    {
                        nodeMap[i, j].setWalkable(false);
                        nodeMap[i, j].setFilled(i * nodeWidth, j * nodeHeight);
                    }
                }
            }
        }
        public void transformWorldToNodeMap(GameObject gameObject)
        {
            setNodeMap(gameObject);
            setGameObjectsToNodeMap(gameObject);
        }

        // Helper Function to Reconstruct Path
        private List<Node> ReconstructPath1(Node node)
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




        //public List<Node> CalculatePath(GameObject gameObject, (int, int) goal)
        //{
        //    int startX, startY, goalX, goalY;

        //    // This need fixing, cuz right now it only checks if its the last node
        //    // I might need to check other wierd cases that might make it go out of bounds
        //    if (start.Item1 == displayWidth || start.Item2 == displayHeight)
        //    {
        //        startX = (start.Item1 / nodeWidth) - 1;
        //        startY = (start.Item2 / nodeHeight) - 1;
        //    }
        //    else
        //    {
        //        startX = start.Item1 / nodeWidth;
        //        startY = start.Item2 / nodeHeight;
        //    }
        //    if (goal.Item1 == displayWidth || goal.Item2 == displayHeight)
        //    {
        //        goalX = (goal.Item1 / nodeWidth) - 1;
        //        goalY = (goal.Item2 / nodeHeight) - 1;
        //    }
        //    else
        //    {
        //        goalX = goal.Item1 / nodeWidth;
        //        goalY = goal.Item2 / nodeHeight;
        //    }


        //    List<Node> openList = new List<Node>();
        //    HashSet<(int, int)> closedList = new HashSet<(int, int)>();

        //    // I hard coded the the transformation of the start and goal to the nodeMap
        //    // I need to fix this later

        //    Node startNode = nodeMap[startX, startY];
        //    Node goalNode = nodeMap[goalX, goalY];
        //    openList.Add(startNode);

        //    while (openList.Count > 0)
        //    {
        //        Node current = openList.OrderBy(n => n.F).First();
        //        if (current.posX == goalNode.posX && current.posY == goalNode.posY)
        //            return ReconstructPath(current);

        //        openList.Remove(current);
        //        closedList.Add((current.posX, current.posY));

        //        foreach (var direction in Directions)
        //        {
        //            int newX = current.posX + direction[0], newY = current.posY + direction[1];
        //            if (newX < 0 || newY < 0 || newX >= nodeMap.GetLength(0) || newY >= nodeMap.GetLength(1) || !nodeMap[newX, newY].walkable || closedList.Contains((newX, newY)))
        //                continue;

        //            Node neighbor = nodeMap[newX, newY];
        //            neighbor.Parent = current;
        //            neighbor.G = current.G + 1;
        //            neighbor.H = Math.Abs(newX - goalNode.posX) + Math.Abs(newY - goalNode.posY);

        //            if (openList.Any(n => n.posX == neighbor.posX && n.posY == neighbor.posY && n.G <= neighbor.G))
        //                continue;

        //            openList.Add(neighbor);
        //        }
        //    }
        //    return new List<Node>();
        //}

        public List<Node> CalculatePath((int, int) start, (int, int) goal)
        {
            //int startX, startY, goalX, goalY;

            // This need fixing, cuz right now it only checks if its the last node
            // I might need to check other wierd cases that might make it go out of bounds
            int nodeMapWidth = nodeMap.GetLength(0);
            int nodeMapHeight = nodeMap.GetLength(1);

            int startX = Math.Clamp((int)(start.Item1 / nodeWidth), 0, nodeMapWidth - 1);
            int startY = Math.Clamp((int)(start.Item2 / nodeHeight), 0, nodeMapHeight - 1);
            int goalX = Math.Clamp(goal.Item1 / nodeWidth, 0, nodeMapWidth - 1);
            int goalY = Math.Clamp(goal.Item2 / nodeHeight, 0, nodeMapHeight - 1);



            List<Node> openList = new List<Node>();
            HashSet<(int, int)> closedList = new HashSet<(int, int)>();

            Node startNode = nodeMap[startX, startY];
            Node goalNode = nodeMap[goalX, goalY];
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node current = openList.OrderBy(n => n.F).First();
                if (current.posX == goalNode.posX && current.posY == goalNode.posY)
                    return ReconstructPath(current);

                openList.Remove(current);
                closedList.Add((current.posX, current.posY));

                foreach (var direction in Directions)
                {
                    int newX = current.posX + direction[0], newY = current.posY + direction[1];
                    if (newX < 0 || newY < 0 || newX >= nodeMap.GetLength(0) || newY >= nodeMap.GetLength(1) || !nodeMap[newX, newY].walkable || closedList.Contains((newX, newY)))
                        continue;

                    Node neighbor = nodeMap[newX, newY];
                    neighbor.Parent = current;
                    neighbor.G = current.G + 1;
                    neighbor.H = Math.Abs(newX - goalNode.posX) + Math.Abs(newY - goalNode.posY);

                    if (openList.Any(n => n.posX == neighbor.posX && n.posY == neighbor.posY && n.G <= neighbor.G))
                        continue;

                    openList.Add(neighbor);
                }
            }
            return new List<Node>();
        }

        private List<Node> ReconstructPath(Node node)
        {
            List<Node> path = new List<Node>();
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            path.Reverse();
            path.RemoveAt(0);
            this.temp = path;
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
        public List<Node> getPath()
        {
            return path;
        }


        public void debugPrintNodeMap()
        {
            for (int i = 0; i < nodeMap.GetLength(0); i++)
            {
                for (int j = 0; j < nodeMap.GetLength(1); j++)
                {
                    if (nodeMap[i, j].walkable == false)
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
        public void debugPrintPath()
        {
            foreach (var node in path)
            {
                Console.WriteLine(node.posX + " " + node.posY);
            }
        }

        public void debugPrintPathVisual(List<Node> path)
        {
            Display d = Bootstrap.getDisplay();
            for (int i = 0; i < nodeMap.GetLength(0); i++)
            {
                for (int j = 0; j < nodeMap.GetLength(1); j++)
                {
                    if (path.Contains(nodeMap[i, j]))
                    {
                        int minX = nodeMap[i, j].minX;
                        int minY = nodeMap[i, j].minY;
                        int maxX = nodeMap[i, j].maxX;
                        int maxY = nodeMap[i, j].maxY;
                        d.drawLine(minX, minY, maxX, maxY, Color.Red);
                    }
                }
            }
        }
        public void debugTestRun(int nodeWidth, int nodeHeight, (int, int) start, (int, int) goal)
        {
            setNodeWidth(nodeWidth);
            setNodeHeight(nodeHeight);
            //transformWorldToGrid();
           // transformGridToNodeMap();
            //transformWorldToNodeMap();
            path = CalculatePath(start, goal);
           // transformPathToGrid();
            Console.WriteLine("The nodeMap for Debugging");
            debugPrintNodeMap();
            Console.WriteLine("The path for Debugging");
            debugPrintPath();
            Console.WriteLine("The path visual for Debugging");
            debugPrintPathVisual(path);

        }

        public void setNodeDimensions(int nodeWidth, int nodeHeight)
        {
            setNodeWidth(nodeWidth);
            setNodeHeight(nodeHeight);
        }


        public void findPath(GameObject gameObject, (int, int) goal)
        {
            transformWorldToNodeMap(gameObject);
            //path = CalculatePath3(gameObject, goal);
            path = CalculatePath(((int)gameObject.transform.X,(int)gameObject.transform.Y), goal);
            //path = CalculatePath2(start, goal);
            debugPrintPathVisual(path);
        }

        public void excludingTags(List<string> tags)
        { 
            excludedTags = tags;
        }


    }   
}
