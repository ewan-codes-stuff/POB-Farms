using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    PathFinder pathFinder;
    List<GridTile> path;
    List<GridTile> previousPath = new List<GridTile>();

    [SerializeField]
    protected bool isAlly = false;

    public Vector2Int gridPosition;

    [SerializeField]
    private int enemyDetectionRadius = 2;

    [SerializeField]
    private Grid grid;

    public bool debugPathFind = false;
    public GameObject test;

    // Start is called before the first frame update
    void Start()
    {
        //TurnManager.instance.EndTurn += 
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(grid.WorldToCell(transform.position).x + (GameManager.instance.tileArray.GetLength(0)/2));
        
        //gridPosition = GameManager.instance.tileArray[grid.WorldToCell(transform.position).x + (GameManager.instance.tileArray.GetLength(0) / 2), grid.WorldToCell(transform.position).z + (GameManager.instance.tileArray.GetLength(1) / 2)].position;
        //gridPosition = new Vector2Int(grid.WorldToCell(transform.position).x + (GameManager.instance.tileArray.GetLength(0) / 2), grid.WorldToCell(transform.position).z + (GameManager.instance.tileArray.GetLength(1) / 2));
        //gridPosition = new Vector2Int((int)(grid.WorldToCell(transform.position).x + (GameManager.instance.ground.transform.localScale.x*10/2)), (int)(grid.WorldToCell(transform.position).z + (GameManager.instance.ground.transform.localScale.z*10 / 2)));
        gridPosition = new Vector2Int((int)(transform.position.x + (GameManager.instance.ground.transform.localScale.x * 10 / 2)), (int)(transform.position.z + (GameManager.instance.ground.transform.localScale.z * 10 / 2)));
        Debug.Log("Update Grid Pos: " + gridPosition);
        GameManager.instance.tileArray[gridPosition].isBlockedByEnemy = !isAlly;
        GameManager.instance.tileArray[gridPosition].isBlockedByAlly = isAlly;
        //Debug.Log("Current Tile Pos: " + GameManager.instance.tileArray[gridPosition.x, gridPosition.y].position);
        if (debugPathFind)
        {
            //PathfindToTarget(GameManager.instance.tileArray[new Vector2Int(5,5)]);
            EnemyTurnPathFind();
            debugPathFind = false;
        }
    }

    public void EnemyTurnPathFind()
    {
        PathfindToTarget(FindTargetInRadius());
    }
    private GridTile FindTargetInRadius()
    {
        GridTile target;
        //Search around the enemy in it's detection radius to find either plants or the player
        List<GridTile> neighbours = pathFinder.GetNeighbourTiles(GameManager.instance.tileArray[gridPosition],enemyDetectionRadius);
        foreach (var neighbour in neighbours)
        {
            if(neighbour.isBlockedByAlly)
            {
                Debug.Log("Found new target plant;");
                target = neighbour;
                return target;
            }
        }
        //If it can't find any plants or the player then it'll default to the player's base.
        //currently a debug location
        int debugTargetX = (int)GameManager.instance.ground.transform.localScale.x*10/2;
        int debugTargetY = (int)GameManager.instance.ground.transform.localScale.z*10/2;
        target = GameManager.instance.tileArray[new Vector2Int(debugTargetX,debugTargetY)];
        return target;
    }

    void PathfindToTarget(GridTile target)
    {

        //Debug.Log(GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.y)].name);
        //Debug.Log(GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.y)]);
        path = pathFinder.FindPath(GameManager.instance.tileArray[gridPosition], target);
        
        Vector2 pathDifference = path[0].gridPosition - gridPosition;

        //Add current position to list of where I've been
        previousPath.Add(path[0]);
        //Update previous tile so it is no longer blocked by enemy
        GameManager.instance.tileArray[gridPosition].isBlockedByEnemy = !isAlly;
        GameManager.instance.tileArray[gridPosition].isBlockedByAlly = isAlly;
        //Move Enemy position in world and in grid space
        gameObject.transform.position = new Vector3(path[0].position.x, 1.0f, path[0].position.y);
        gridPosition += new Vector2Int((int)pathDifference.x,(int)pathDifference.y);
        //Update new tile to be blocked by enemy
        GameManager.instance.tileArray[gridPosition].isBlockedByEnemy = !isAlly;
        GameManager.instance.tileArray[gridPosition].isBlockedByAlly = isAlly;

        path.RemoveAt(0);
        debugPathFind = false;
    }
}
