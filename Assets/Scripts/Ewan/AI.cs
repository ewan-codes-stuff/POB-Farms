using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Entity
{
    [SerializeField]
    PathFinder pathFinder;

    [SerializeField]
    private int AIDetectionRadius = 2;

    [SerializeField]
    private Grid grid;

    public bool debugPathFind = false;
    public GameObject test;
    bool lateStart = false;

    // Start is called before the first frame update
    void Start()
    {
        SetGridPosition(new Vector2Int((int)(transform.position.x + (GameManager.instance.ground.transform.localScale.x * 10 / 2)), (int)(transform.position.z + (GameManager.instance.ground.transform.localScale.z * 10 / 2))));
        Debug.Log("Update Grid Pos: " + GetGridPosition());
        //GameManager.instance.tileArray[GetGridPosition()].isBlockedByEntity = true;
        //GameManager.instance.tileArray[GetGridPosition()].entity = this;
        //AddAIToArnieGrid();
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG FUNCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if(!lateStart)
        {
            AddAIToArnieGrid();
            lateStart = true;
        }
        //DEBUG FUNCTION DELETE AFTER USE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Debug.Log(grid.WorldToCell(transform.position).x + (GameManager.instance.tileArray.GetLength(0)/2));

        //gridPosition = GameManager.instance.tileArray[grid.WorldToCell(transform.position).x + (GameManager.instance.tileArray.GetLength(0) / 2), grid.WorldToCell(transform.position).z + (GameManager.instance.tileArray.GetLength(1) / 2)].position;
        //gridPosition = new Vector2Int(grid.WorldToCell(transform.position).x + (GameManager.instance.tileArray.GetLength(0) / 2), grid.WorldToCell(transform.position).z + (GameManager.instance.tileArray.GetLength(1) / 2));
        //gridPosition = new Vector2Int((int)(grid.WorldToCell(transform.position).x + (GameManager.instance.ground.transform.localScale.x*10/2)), (int)(grid.WorldToCell(transform.position).z + (GameManager.instance.ground.transform.localScale.z*10 / 2)));
        SetGridPosition(new Vector2Int((int)(transform.position.x), (int)(transform.position.z)));
        Debug.Log("Update Grid Pos: " + GetGridPosition());
        GameManager.instance.tileArray[GetGridPosition()].isBlockedByEntity = true;
        GameManager.instance.tileArray[GetGridPosition()].entity = this;
        //Debug.Log("Current Tile Pos: " + GameManager.instance.tileArray[gridPosition.x, gridPosition.y].position);
        if (debugPathFind)
        {
            //PathfindToTarget(GameManager.instance.tileArray[new Vector2Int(5,5)]);
            SetAlly(false);
            EnemyTurnPathFind();
            debugPathFind = false;
        }
    }
    public void AddAIToArnieGrid()
    {
        GameManager.instance.GetPlacedObjects().Add(gameObject);
        GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z))), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);
    }
    public void RemoveAIFromArnieGrid()
    {
        GameManager.instance.GetPlacedObjects().Remove(gameObject);
        GameManager.instance.GetObjectData().RemoveObjectAt(GameManager.instance.GetGrid().WorldToCell(transform.position), new Vector2Int(1, 1));
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
        for (int x = GetGridPosition().x - AIDetectionRadius; x <= GetGridPosition().x + AIDetectionRadius; x++)
        {
            for (int y = GetGridPosition().y - AIDetectionRadius; y <= GetGridPosition().y + AIDetectionRadius; y++)
            {
                if(GameManager.instance.tileArray.ContainsKey(new Vector2Int(x, y)))
                {
                    Debug.Log("Tile to Check: "+GameManager.instance.tileArray[new Vector2Int(x, y)].gridPosition);
                    if (GameManager.instance.tileArray[new Vector2Int(x, y)].isBlockedByEntity && GameManager.instance.tileArray[new Vector2Int(x, y)] !=GameManager.instance.tileArray[GetGridPosition()])
                    {
                        
                        Debug.Log("Found new target plant;");
                        Debug.Log(GameManager.instance.tileArray[new Vector2Int(x, y)].gridPosition);
                        if (!IsAlly() && GameManager.instance.tileArray[new Vector2Int(x, y)].entity.IsAlly())
                        {
                            targetList.Add(GameManager.instance.tileArray[new Vector2Int(x, y)]);
                            
                        }
                        if (IsAlly() && GameManager.instance.tileArray[new Vector2Int(x, y)].entity.IsAlly() == false)
                        {
                            targetList.Add(GameManager.instance.tileArray[new Vector2Int(x, y)]);
                            
                        }
                    }
                }
            }
        }
        if(targetList.Count != 0)
        {
            target = targetList[Random.Range(0, targetList.Count)];
            return target;
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
