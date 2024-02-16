using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private Vector2 gridSize;

    private Cell[,] grid;

    private void Start()
    {
        grid = new Cell[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y].position = new Vector2(x - ((int)gridSize.x /2), y - ((int)gridSize.y / 2));
            }
        }
    }

    private struct Cell
    {
        public Vector2 position;
        public bool occupided;
    }
}
