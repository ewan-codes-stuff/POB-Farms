using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    public List<GridTile> FindPath(GridTile start, GridTile end)
    {
        //List of our positions
        List<GridTile> openList = new List<GridTile>();
        //List of positions we have processed through
        List<GridTile> closedList = new List<GridTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            //Order the unprocessed tiles by the x positions 
            GridTile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile.position == end.position)
            {
                //finish when reach our last position in the list and return the list
                Debug.Log("Finished List");
                return GetFinishedList(start, end);
            }
            var neighbourTiles = GetNeighbourTiles(currentTile);

            //foreach(var neighbour in neighbourTiles) { 
             for(int n = 0; n <= neighbourTiles.Count; n++)
                {
                var neighbour = neighbourTiles[n];

                if (!neighbour.traversable || closedList.Contains(neighbour))
                {
                    continue;
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previousTilePosition = currentTile.position;

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

    private List<GridTile> GetNeighbourTiles(GridTile currentTile)
    {
        var grid = GameManager.instance.tileArray;

        List<GridTile> neighbours = new List<GridTile>();

        //Top Neighbour
        Vector2Int locationToCheck = new Vector2Int((int)currentTile.position.x, (int)currentTile.position.y + 1);

        if (grid[locationToCheck.x,locationToCheck.y].traversable&& !grid[locationToCheck.x, locationToCheck.y].isBlockedByEnemy)
        {
            neighbours.Add(grid[locationToCheck.x,locationToCheck.y]);
        }

        //Bottom Neighbour
        locationToCheck = new Vector2Int((int)currentTile.position.x, (int)currentTile.position.y - 1);

        if (grid[locationToCheck.x, locationToCheck.y].traversable && !grid[locationToCheck.x, locationToCheck.y].isBlockedByEnemy)
        {
            neighbours.Add(grid[locationToCheck.x, locationToCheck.y]);
        }

        //Right Neighbour
        locationToCheck = new Vector2Int((int)currentTile.position.x + 1, (int)currentTile.position.y);

        if (grid[locationToCheck.x, locationToCheck.y].traversable && !grid[locationToCheck.x, locationToCheck.y].isBlockedByEnemy)
        {
            neighbours.Add(grid[locationToCheck.x, locationToCheck.y]);
        }

        //Left Neighbour
        locationToCheck = new Vector2Int((int)currentTile.position.x - 1, (int)currentTile.position.y);

        if (grid[locationToCheck.x, locationToCheck.y].traversable && !grid[locationToCheck.x, locationToCheck.y].isBlockedByEnemy)
        {
            neighbours.Add(grid[locationToCheck.x, locationToCheck.y]);
        }
        return neighbours;
    }

    private List<GridTile> GetFinishedList(GridTile start, GridTile end)
    {
        List<GridTile> finishedList = new List<GridTile>();

        GridTile currentTile = end;

        while (currentTile.position != start.position)
        {
            finishedList.Add(currentTile);
            currentTile = GameManager.instance.tileArray[currentTile.previousTilePosition.x, currentTile.previousTilePosition.y];
        }

        finishedList.Reverse();
        return finishedList;
    }
}
