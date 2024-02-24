using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    List<GridTile> path;
    List<GridTile> previousPath = new List<GridTile>();

    GridTile closestTileToTarget;
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
            GridTile currentTile = openList[UnityEngine.Random.Range(0,count)];


            openList.Remove(currentTile);
            closedList.Add(currentTile);
            closedList = new List<GridTile>(closedList.OrderBy(x => x.F));
            if (closedList.Count > 1) { closestTileToTarget = closedList[1]; }

            if (currentTile == end)
            {
                //finish
                return GetFinishedList(start, end);
            }
            var neighbourTiles = GetNeighbourTiles(currentTile,1);

            foreach (var neighbour in neighbourTiles)
            {
                //You can't go there if it's
                //Not traversable, you've been there before, or something is there that isn't the house
                if (!neighbour.traversable || closedList.Contains(neighbour) || (neighbour.entity != null && neighbour.entity != end.entity))
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
        //When you cannot get to the end position, return the list with what you have
        return GetFinishedList(start, closestTileToTarget);
        //return new List<GridTile>();
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

        for (int currentLookingDistance = 0; currentLookingDistance <= neighbourRange; currentLookingDistance++)
        {
            //Top Neighbour
            Vector2Int locationToCheck = new Vector2Int((int)currentTile.position.x, (int)currentTile.position.y + currentLookingDistance);

            if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            {
                
                    neighbours.Add(grid[locationToCheck]);
                
            }

            //Bottom Neighbour
            locationToCheck = new Vector2Int((int)currentTile.position.x, (int)currentTile.position.y - currentLookingDistance);

            if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            {
                
                    neighbours.Add(grid[locationToCheck]);
                
            }

            //Right Neighbour
            locationToCheck = new Vector2Int((int)currentTile.position.x + currentLookingDistance, (int)currentTile.position.y);

            if (grid.ContainsKey(locationToCheck) && GameManager.instance.tileArray[locationToCheck] != currentTile)
            {
                
                    neighbours.Add(grid[locationToCheck]);
                
                
            }

            //Left Neighbour
            locationToCheck = new Vector2Int((int)currentTile.position.x - currentLookingDistance, (int)currentTile.position.y);

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
        //Debug.Log(start.gridPosition);
        //Debug.Log(target.gridPosition);
        GridTile currentTile = target;
        GridTile tempPrevTile = null;
        //Debug.Log(currentTile.position);
        if (currentTile == null) return finishedList;
        
        while (currentTile.position != start.position)
        {
            finishedList.Add(currentTile);
            if (currentTile.previous == null)
            { 
                Debug.Log("Checked if previous tile was null, and found it was nill");
                return finishedList;
            }
            tempPrevTile = currentTile.previous;
            currentTile.previous = null;
            currentTile = tempPrevTile;
              
        }

        finishedList.Reverse();
        return finishedList;
    }

    public void PathfindToTarget(AI self,GridTile target)
    {
        path = FindPath(GameManager.instance.tileArray[self.GetGridPosition()], target);

        if (path.Count == 0) { Debug.LogError("Path not found"); }
        else
        {
            Vector2 pathDifference = path[0].gridPosition - self.GetGridPosition();

            //if(path[0])
            // If Unity Grid is occupied by an object
            // You cannot go there
            if (path[0].entity == null || path[0].entity == this)
            {
                //Add current position to list of where I've been
                previousPath.Add(path[0]);
                //Update previous tile so it is no longer blocked by enemy
                GameManager.instance.tileArray[self.GetGridPosition()].entity = null;
                self.RemoveAIFromArnieGrid();
                //Move Enemy position in world and in grid space
                gameObject.transform.position = new Vector3(path[0].position.x, 0.5f, path[0].position.y);
                //Add to grids
                self.SetGridPosition(self.GetGridPosition() + new Vector2Int((int)pathDifference.x, (int)pathDifference.y)); //Questionable
                self.AddAIToArnieGrid();
                
                //Update new tile to be blocked by enemy
                GameManager.instance.tileArray[self.GetGridPosition()].entity = self;
            }
            path.RemoveAt(0);
        }
    }

}
