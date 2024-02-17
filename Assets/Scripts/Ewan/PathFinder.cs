using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    List<GridTile> path;
    List<GridTile> previousPath = new List<GridTile>();
    bool done = false;

    public List<GridTile> FindPath(GridTile start, GridTile end)
    {
        List<GridTile> openList = new List<GridTile>();
        List<GridTile> closedList = new List<GridTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            //Compare tiles in the openList (which are comprised of the neighbours we have checked)
            //and order them from closest to furthest
            
            openList = new List<GridTile>(openList.OrderBy(x => x.F));
            done = false;
            int count = 0;
            int checkValue = openList[0].F;
            while (!done)
            {
                if (count < openList.Count) 
                { 
                    if (openList[count].F != checkValue)
                    {
                        done = true;
                    }
                    else
                    {
                        count++;
                    }
                }
                else
                {
                    done = true;
                }
            }
            GridTile currentTile = openList[Random.Range(0,count)];


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
        return Mathf.Abs(start.gridPosition.x - neighbour.gridPosition.x) + Mathf.Abs(start.gridPosition.y - neighbour.gridPosition.y);
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

            ////Top-Left Neighbour
            //locationToCheck = new Vector2Int((int)currentTile.position.x + xOffset, yOffset + (int)currentTile.position.y - c);

            //if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            //{
            //    neighbours.Add(grid[locationToCheck]);
            //}

            //Top-Right Neighbour
            locationToCheck = new Vector2Int((int)currentTile.position.x + xOffset, yOffset + (int)currentTile.position.y - c);

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

    public void PathfindToTarget(AI self,GridTile target)
    {
        //Debug.Log(GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.y)].name);
        //Debug.Log(GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.y)]);
        path = FindPath(GameManager.instance.tileArray[self.GetGridPosition()], target);

        Vector2 pathDifference = path[0].gridPosition - self.GetGridPosition();

        //Add current position to list of where I've been
        previousPath.Add(path[0]);
        //Update previous tile so it is no longer blocked by enemy
        GameManager.instance.tileArray[self.GetGridPosition()].isBlockedByEntity = false;
        //Move Enemy position in world and in grid space
        gameObject.transform.position = new Vector3(path[0].position.x, 0.5f, path[0].position.y);
        self.SetGridPosition(self.GetGridPosition() + new Vector2Int((int)pathDifference.x, (int)pathDifference.y));
        //Update new tile to be blocked by enemy
        GameManager.instance.tileArray[self.GetGridPosition()].isBlockedByEntity = true;

        path.RemoveAt(0);
    }

}
