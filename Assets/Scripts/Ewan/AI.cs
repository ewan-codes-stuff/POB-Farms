using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Entity
{
    [SerializeField]
    PathFinder pathFinder;
    

    [SerializeField]
    public bool isAlly = false;

    [SerializeField]
    private int AIDetectionRadius = 2;

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
        GameManager.instance.tileArray[gridPosition].isBlockedByEntity = true;
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
        pathFinder.PathfindToTarget(this,FindTargetInRadius());
    }
    private GridTile FindTargetInRadius()
    {
        GridTile target;
        List<GridTile> targetList = new List<GridTile>();
        //Search around the enemy in it's detection radius to find either plants or the player
        for (int x = gridPosition.x - AIDetectionRadius; x <= gridPosition.x + AIDetectionRadius; x++)
        {
            for (int y = gridPosition.y - AIDetectionRadius; y <= gridPosition.y + AIDetectionRadius; y++)
            {
                if(GameManager.instance.tileArray.ContainsKey(new Vector2Int(x, y)))
                {
                    if (GameManager.instance.tileArray[new Vector2Int(x, y)].isBlockedByEntity)
                    {
                        
                        Debug.Log("Found new target plant;");
                        if (!isAlly && GameManager.instance.tileArray[new Vector2Int(x, y)].entity.isAlly())
                        {
                            targetList.Add(GameManager.instance.tileArray[new Vector2Int(gridPosition.x + AIDetectionRadius, gridPosition.y + AIDetectionRadius)]);
                            
                        }
                        if (isAlly && GameManager.instance.tileArray[new Vector2Int(x, y)].entity.isAlly == false)
                        {
                            targetList.Add(GameManager.instance.tileArray[new Vector2Int(gridPosition.x + AIDetectionRadius, gridPosition.y + AIDetectionRadius)]);
                            
                        }
                    }
                }
            }
        }
        if(targetList.Count != 0)
        {
            target = targetList[Random.Range(0, targetList.Count)];
        }
        //If it can't find any plants or the player then it'll default to the player's base.
        //currently a debug location
        int debugTargetX = (int)GameManager.instance.ground.transform.localScale.x*10/2;
        int debugTargetY = (int)GameManager.instance.ground.transform.localScale.z*10/2;
        target = GameManager.instance.tileArray[new Vector2Int(debugTargetX,debugTargetY)];
        Debug.Log("Target Grid Position: " + target.gridPosition);
        Debug.Log("Target Tile: " + target.name);
        return target;
    }

    
}
