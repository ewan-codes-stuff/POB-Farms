using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Entity
{
    [SerializeField]
    PathFinder pathFinder;

    [SerializeField]
    private int AIDetectionRadius = 2;

    bool lateStart = false;

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        GameManager.instance.tileArray[GetGridPosition()].isBlockedByEntity = true;
        
        if (!PlacementSystem.instance.placedGameObject.Contains(gameObject) && GameManager.instance.tileArray[GetGridPosition()].entity == null) 
        { 
            AddAIToArnieGrid();
            GameManager.instance.tileArray[GetGridPosition()].entity = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetGridPosition(new Vector2Int((int)(transform.position.x), (int)(transform.position.z)));

        GameManager.instance.tileArray[GetGridPosition()].isBlockedByEntity = true;
        GameManager.instance.tileArray[GetGridPosition()].entity = this;
    }
    public void AddAIToArnieGrid()
    {
        //Add to Unity Grid
        GameManager.instance.GetPlacedObjects().Add(gameObject);
        if (GameManager.instance.GetObjectData() != null)
        {
            GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z))), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);
        }
    }
    public void RemoveAIFromArnieGrid()
    {
        //Remove to Unity Grid
        GameManager.instance.GetPlacedObjects().Remove(gameObject);
        if (GameManager.instance.GetObjectData() != null)
        {
            GameManager.instance.GetObjectData().RemoveObjectAt(GameManager.instance.GetGrid().WorldToCell(transform.position), new Vector2Int(1, 1));
        }
    }
    public void AITurn()
    {
        GridTile target = FindTargetInRadius();
        if (target != null)
        {
            if (Vector2Int.Distance(target.gridPosition, GetGridPosition()) <= 1.0f)
            {
                Attack(this, target.entity);
            }
            else if (target != null)
            {
                pathFinder.PathfindToTarget(this, target);
            }
        }
        //Wander();
    }
    private GridTile FindTargetInRadius()
    {
        GridTile target = null;
        List<GridTile> targetList = new List<GridTile>();
        //Search around the enemy in it's detection radius to find either plants or the player
        for (int x = GetGridPosition().x - AIDetectionRadius; x <= GetGridPosition().x + AIDetectionRadius; x++)
        {
            for (int y = GetGridPosition().y - AIDetectionRadius; y <= GetGridPosition().y + AIDetectionRadius; y++)
            {   //If the location is actually in the Tile Dictionary
                if(GameManager.instance.tileArray.ContainsKey(new Vector2Int(x, y)))
                {   //Check the tile is occupied by an entity and the entity is not self
                    if (GameManager.instance.tileArray[new Vector2Int(x, y)].entity != null && GameManager.instance.tileArray[new Vector2Int(x, y)] !=GameManager.instance.tileArray[GetGridPosition()])
                    {
                        //Differing If statements based on whether the AI is enemy or ally

                        //If Self is an enemy and the target is an ally, add it to list of targets
                        if (!IsAlly() && GameManager.instance.tileArray[new Vector2Int(x, y)].entity.IsAlly())
                        {
                            targetList.Add(GameManager.instance.tileArray[new Vector2Int(x, y)]);
                            
                        }
                        //If Self is an ally and the target is an enemy, add it to list of targets
                        if (IsAlly() && GameManager.instance.tileArray[new Vector2Int(x, y)].entity.IsAlly() == false)
                        {
                            targetList.Add(GameManager.instance.tileArray[new Vector2Int(x, y)]);
                            
                        }
                    }
                }
            }
        }
        if (!IsAlly())
        {
            targetList.Add(GameManager.instance.tileArray[new Vector2Int(-1,-1)]);
            targetList.Add(GameManager.instance.tileArray[new Vector2Int(-1,0)]);
            targetList.Add(GameManager.instance.tileArray[new Vector2Int(0,0)]);
            targetList.Add(GameManager.instance.tileArray[new Vector2Int(0,-1)]);
            float checkValue = 10.0f;
            foreach(GridTile tile in targetList)
            {
                float currentDistance = Vector2Int.Distance(tile.gridPosition, GetGridPosition());
                if(checkValue > currentDistance)
                {
                    target = tile;
                    checkValue = currentDistance;
                }
            }
            return target;
        }
        if (targetList.Count != 0)
        {
            target = targetList[Random.Range(0, targetList.Count)];
            return target;
        }
        //If it can't find any plants or the player then it'll default to the player's base.
        //currently a debug location 
        

        return target;
    }

    void Attack(Entity self, Entity targetEntity)
    {
        //Debug.Log(self.gameObject.name+ " Attacking " + targetEntity.gameObject.name);
        targetEntity.TakeDamage(1);
    }

    void Wander()
    {
        List<GridTile> wanderNeighbours = pathFinder.GetNeighbourTiles(GameManager.instance.tileArray[GetGridPosition()], 1);

        GridTile wanderTile = wanderNeighbours[Random.Range(0, wanderNeighbours.Count)];

        if (wanderTile.entity == null)
        {
            Vector2 pathDifference = wanderTile.gridPosition - GetGridPosition();
            GameManager.instance.tileArray[GetGridPosition()].entity = null;
            RemoveAIFromArnieGrid();
            //Move Enemy position in world and in grid space
            gameObject.transform.position = new Vector3(wanderTile.position.x, 0.0f, wanderTile.position.y);
            AddAIToArnieGrid();
            SetGridPosition(GetGridPosition() + new Vector2Int((int)pathDifference.x, (int)pathDifference.y));
            //Update new tile to be blocked by enemy
            GameManager.instance.tileArray[GetGridPosition()].entity = this;
        }
    }
}
