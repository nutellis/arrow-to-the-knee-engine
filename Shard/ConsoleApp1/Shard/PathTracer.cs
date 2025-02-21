using System;
using System.Collections.Generic;
using System.Linq;
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
    class PathTracer
    {

    
        // To make The node system for A*
        class Node
        {
            public int gridX, gridY;
            public Vector2 worldPosition;
            public bool isWalkable;

            public Node(int x, int y)
            {
                gridX = x;
                gridY = y;
                worldPosition = new Vector2(x * 16, y * 16);
                isWalkable = true; // Default to walkable
            }
        }

        // To make the display in a grid
        class Grid
        {
            private int tileSize = 16;
            private int gridWidth, gridHeight;
            private Node[,] grid;

            public Grid(int screenWidth, int screenHeight)
            {
                gridWidth = screenWidth / tileSize;
                gridHeight = screenHeight / tileSize;
                grid = new Node[gridWidth, gridHeight];
                GenerateGrid();
            }

            private void GenerateGrid()
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        grid[x, y] = new Node(x, y);

                        // Example: Mark some tiles as obstacles (modify this condition)
                        // need to get the game objects and check if they are in the tile

                        // This needs Fix ???
                        if ((x + y) % 5 == 0)
                        {
                            grid[x, y].isWalkable = false;
                        }
                    }
                }
            }

            public Node GetNodeAt(int x, int y)
            {
                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                {
                    return grid[x, y];
                }
                return null;
            }

            public void PrintGrid()
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    for (int x = 0; x < gridWidth; x++)
                    {
                        Console.Write(grid[x, y].isWalkable ? "." : "#"); // '.' = walkable, '#' = obstacle
                    }
                    Console.WriteLine();
                }
            }
        }



    }
}
