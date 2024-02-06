using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    //public List<Tile> FindPath(Tile start, Tile end)
    //{
    //    List<Tile> openList = new List<Tile>();
    //    List<Tile> closedList = new List<Tile>();

    //    openList.Add(start);

    //    while (openList.Count > 0)
    //    {
    //        Tile currentTile = openList.OrderBy(x => x.F).First();

    //        openList.Remove(currentTile);
    //        closedList.Add(currentTile);

    //        if (currentTile == end)
    //        {
    //            //finish
    //            Debug.Log("Finished List");
    //            return GetFinishedList(start, end);
    //        }
    //        var neighbourTiles = GetNeighbourTiles(currentTile);

    //        foreach (var neighbour in neighbourTiles)
    //        {
    //            if (!neighbour.traversable || closedList.Contains(neighbour))
    //            {
    //                continue;
    //            }

    //            neighbour.G = GetManhattenDistance(start, neighbour);
    //            neighbour.H = GetManhattenDistance(end, neighbour);

    //            neighbour.previous = currentTile;

    //            if (!openList.Contains(neighbour))
    //            {
    //                openList.Add(neighbour);
    //            }
    //        }
    //    }
    //    return new List<Tile>();
    //}

    //private int GetManhattenDistance(Tile start, Tile neighbour)
    //{
    //    return Mathf.Abs(start.position.x - neighbour.position.x) + Mathf.Abs(start.position.y - neighbour.position.y);
    //}

    //private List<Tile> GetNeighbourTiles(Tile currentTile)
    //{
    //    var grid = GridManager.instance.tiles;

    //    List<Tile> neighbours = new List<Tile>();

    //    //Top Neighbour
    //    Vector2Int locationToCheck = new Vector2Int((int)currentTile.position.x, (int)currentTile.position.y + 1);

    //    if (grid.ContainsKey(locationToCheck))
    //    {
    //        neighbours.Add(grid[locationToCheck]);
    //    }

    //    //Bottom Neighbour
    //    locationToCheck = new Vector2Int((int)currentTile.position.x, (int)currentTile.position.y - 1);

    //    if (grid.ContainsKey(locationToCheck))
    //    {
    //        neighbours.Add(grid[locationToCheck]);
    //    }

    //    //Right Neighbour
    //    locationToCheck = new Vector2Int((int)currentTile.position.x + 1, (int)currentTile.position.y);

    //    if (grid.ContainsKey(locationToCheck))
    //    {
    //        neighbours.Add(grid[locationToCheck]);
    //    }

    //    //Left Neighbour
    //    locationToCheck = new Vector2Int((int)currentTile.position.x - 1, (int)currentTile.position.y);

    //    if (grid.ContainsKey(locationToCheck))
    //    {
    //        neighbours.Add(grid[locationToCheck]);
    //    }
    //    return neighbours;
    //}

    //private List<Tile> GetFinishedList(Tile start, Tile end)
    //{
    //    List<Tile> finishedList = new List<Tile>();

    //    Tile currentTile = end;

    //    while (currentTile != start)
    //    {
    //        finishedList.Add(currentTile);
    //        currentTile = currentTile.previous;
    //    }

    //    finishedList.Reverse();
    //    return finishedList;
    //}
}
