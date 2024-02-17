using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    public List<GridTile> FindPath(GridTile start, GridTile end)
    {
        List<GridTile> openList = new List<GridTile>();
        List<GridTile> closedList = new List<GridTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            GridTile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
            {
                //finish
                return GetFinishedList(start, end);
            }
            var neighbourTiles = GetNeighbourTiles(currentTile,1);

            foreach (var neighbour in neighbourTiles)
            {
                if (!neighbour.traversable || closedList.Contains(neighbour))
                {
                    continue;
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previous = currentTile;

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }
        return new List<GridTile>();
    }

    private int GetManhattenDistance(GridTile start, GridTile neighbour)
    {
        return Mathf.Abs(start.position.x - neighbour.position.x) + Mathf.Abs(start.position.y - neighbour.position.y);
    }

    public List<GridTile> GetNeighbourTiles(GridTile currentTile, int neighbourRange)
    {
        var grid = GameManager.instance.tileArray;

        int xOffset = (int)(GameManager.instance.ground.transform.localScale.x*10/2);

        int yOffset = (int)(GameManager.instance.ground.transform.localScale.z * 10 / 2);


        List<GridTile> neighbours = new List<GridTile>();

        for (int c = 0; c <= neighbourRange; c++)
        {
            //Top Neighbour
            Vector2Int locationToCheck = new Vector2Int((int)currentTile.position.x + xOffset, yOffset + (int)currentTile.position.y + c);

            if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            {
                neighbours.Add(grid[locationToCheck]);
            }

            //Bottom Neighbour
            locationToCheck = new Vector2Int((int)currentTile.position.x + xOffset, yOffset + (int)currentTile.position.y - c);

            if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            {
                neighbours.Add(grid[locationToCheck]);
            }

            //Right Neighbour
            locationToCheck = new Vector2Int((int)currentTile.position.x + c + xOffset, yOffset + (int)currentTile.position.y);

            if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            {
                neighbours.Add(grid[locationToCheck]);
            }

            //Left Neighbour
            locationToCheck = new Vector2Int((int)currentTile.position.x - c + xOffset, yOffset + (int)currentTile.position.y);

            if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            {
                neighbours.Add(grid[locationToCheck]);
            }
        }

        return neighbours;
    }

    private List<GridTile> GetFinishedList(GridTile start, GridTile target)
    {
        List<GridTile> finishedList = new List<GridTile>();

        GridTile currentTile = target;
        //Debug.Log(currentTile.position);
        while (currentTile.position != start.position)
        {
            finishedList.Add(currentTile);

            currentTile = currentTile.previous;
              
        }

        finishedList.Reverse();
        return finishedList;
    }

    //public List<GridTile> GetShortestPathDijkstra()
    //{

    //}

    //private void DijkstraSearch()
    //{
        
    //}

}
