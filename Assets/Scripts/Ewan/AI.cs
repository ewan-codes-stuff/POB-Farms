using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Entity
{
    [SerializeField]
    PathFinder pathFinder;

    [SerializeField]
    private int AIDetectionRadius = 2;

    float speed = 8f;

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        //GameManager.instance.tileArray[GetGridPosition()].isBlockedByEntity = true;
        
        if (!PlacementSystem.instance.placedGameObject.Contains(gameObject) && GameManager.instance.tileArray[GetGridPosition()].entity == null) 
        {
            AddEntityToGrids();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        SetGridPosition(new Vector2Int((int)(transform.position.x), (int)(transform.position.z)));

        //GameManager.instance.tileArray[GetGridPosition()].isBlockedByEntity = true;
        GameManager.instance.tileArray[GetGridPosition()].entity = this;
    }

    public override void AddEntityToGrids()
    {
        base.AddEntityToGrids();
        if (!GameManager.instance.aiManager.isInAIList(this))
        {
            GameManager.instance.aiManager.AddAIToList(gameObject);
        }
    }
    public override void RemoveEntityFromGrids(Vector3Int gridPosition)
    {
        GameManager.instance.aiManager.RemoveAIFromList(gameObject);
        base.RemoveEntityFromGrids(gridPosition);
    }

    public override void Die()
    {
        GameManager.instance.aiManager.RemoveAIFromList(gameObject);
        base.Die();
        
    }

    //worldPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z))
    public void AddAIToArnieGrid(Vector3Int worldPos)
    {
        //Add to Unity Grid
        GameManager.instance.GetPlacedObjects().Add(gameObject);
        GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(worldPos), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);
        
    }
    public void RemoveAIFromArnieGrid()
    {
        //Remove to Unity Grid
        GameManager.instance.GetPlacedObjects().Remove(gameObject);
        GameManager.instance.GetObjectData().RemoveObjectAt(GameManager.instance.GetGrid().WorldToCell(transform.position), new Vector2Int(1, 1));
        
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
        if (targetEntity != null)
        {
            targetEntity.TakeDamage(1);
        }
        
    }

    public void Wander()
    {
        //Get list of neighbouring tiles
        List<GridTile> wanderNeighbours = pathFinder.GetNeighbourTiles(GameManager.instance.tileArray[GetGridPosition()], 1);

        //Pick a random neighbour tile to wander to
        GridTile wanderTile = wanderNeighbours[Random.Range(0, wanderNeighbours.Count)];

        //If there is nothing on the chosen tile
        if (wanderTile.entity == null)
        {
            Vector2 pathDifference = wanderTile.gridPosition - GetGridPosition();
            GameManager.instance.tileArray[GetGridPosition()].entity = null;
            RemoveAIFromArnieGrid();

            
            SetGridPosition(GetGridPosition() + new Vector2Int((int)pathDifference.x, (int)pathDifference.y));
            //Update new tile to be blocked by enemy
            GameManager.instance.tileArray[GetGridPosition()].entity = this;
            //Move Enemy position in world and in grid space
            StartCoroutine(Move(new Vector3(wanderTile.position.x, 0.0f, wanderTile.position.y)));

            AddAIToArnieGrid(new Vector3Int(wanderTile.position.x, 0, wanderTile.position.y));
        }
    }

    public IEnumerator Move(Vector3 target)
    {
        // While the player has not met the target position continue moving across a tile
        while ((target - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        // Sets the players position to the end position as they are close enough by a negligable amount
        transform.position = target;
    }

    public void MoveIt(Vector3 target)
    {
        // While the player has not met the target position continue moving across a tile
        while ((target - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        // Sets the players position to the end position as they are close enough by a negligable amount
        transform.position = target;
    }
}
