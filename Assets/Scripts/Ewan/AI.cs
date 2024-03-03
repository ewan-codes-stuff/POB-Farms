using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AI : Entity
{
    #region Serialized Fields
    [SerializeField]
    PathFinder pathFinder;
                                                                                
    [SerializeField]
    private int AIDetectionRadius = 2;

    [SerializeField]
    private float speed = 8f;
    #endregion

    #region Private Variables
    private GridTile targetTile;
    private bool atEnemy = false;
    #endregion

    #region Initalisation
    //Initalise the AI here
    public override void Init()
    {
        //Run the base initalise code
        base.Init();

        SetGridPosition(new Vector2Int((int)transform.position.x, (int)transform.position.z));

        //Add AI to the grid systems
        if (IsAlly())
        {
            AddEntityToGrids();
        }
    }
    #endregion
    private GridTile FindTargetInRadius()
    {
        //Make sure the current target tile is cleared
        targetTile = null;
        List<GridTile> targetList = new List<GridTile>();
        //Check tiles within the AI's detection radius for other entities
        for (int x = GetGridPosition().x - AIDetectionRadius; x <= GetGridPosition().x + AIDetectionRadius; x++)
        {
            for (int y = GetGridPosition().y - AIDetectionRadius; y <= GetGridPosition().y + AIDetectionRadius; y++)
            {   
                //If the Tile Dictionary contains this location
                if(GameManager.instance.tileArray.ContainsKey(new Vector2Int(x, y)))
                {   
                    //Check that the tile is occupied by an entity and the entity is not self
                    //The second part of this is a stupid check
                    if (GameManager.instance.tileArray[new Vector2Int(x, y)].entity != null && GameManager.instance.tileArray[new Vector2Int(x, y)] != GameManager.instance.tileArray[GetGridPosition()])
                    {

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
                    targetTile = tile;
                    checkValue = currentDistance;
                }
            }
            return targetTile;
        }
        if (targetList.Count != 0)
        {
            targetTile = targetList[Random.Range(0, targetList.Count)];
            return targetTile;
        }
        //If it can't find any plants or the player then it'll default to the player's base.
        //currently a debug location 
        

        return targetTile;
    }

    #region Coroutines
    public IEnumerator AITurn()
    {
        targetTile = FindTargetInRadius();
        if (targetTile != null)
        {
            if (Vector2Int.Distance(targetTile.gridPosition, GetGridPosition()) <= 1.0f)
            {
                yield return Attack(targetTile.entity);
            }
            else if (targetTile != null)
            {

                yield return pathFinder.PathfindToTarget(this, targetTile);
            }
        }
    }

    public IEnumerator Wander()
    {
        //Get list of neighbouring tiles
        List<GridTile> wanderNeighbours = pathFinder.GetNeighbourTiles(GameManager.instance.tileArray[GetGridPosition()], 1);

        //Pick a random neighbour tile to wander to
        GridTile wanderTile = wanderNeighbours[Random.Range(0, wanderNeighbours.Count)];

        //If there is nothing on the chosen tile
        if (wanderTile.entity == null)
        {
            Vector2 pathDifference = wanderTile.gridPosition - GetGridPosition();

            //Remove Entity from previous Grid position
            RemoveEntityFromGrids();

            SetGridPosition(wanderTile.gridPosition);

            //Update the Enity's Grid Position
            AddEntityToGrids(new Vector3Int(GetGridPosition().x, 0, GetGridPosition().y));

            //Move Enemy position in world and in grid space
            yield return StartCoroutine(Move(new Vector3(wanderTile.position.x, 0.0f, wanderTile.position.y)));
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

    private IEnumerator Attack(Entity targetEntity)
    {
        //While the AI has not approached the target yet and is not at the target position
        while (((targetEntity.transform.position - transform.position).sqrMagnitude > Mathf.Epsilon) && !atEnemy)
        {
            //Move the AI towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetEntity.transform.position, speed * Time.deltaTime);
            yield return null;
        }
        //Once at the target set atEnemy to true
        atEnemy = true;
        //Check that the target entity isn't null
        if (targetEntity != null)
        {
            //Then Damage the target entity
            yield return StartCoroutine(targetEntity.TakeDamage(GetDamage()));
        }
        //While now not at the original poition and after approaching the target
        while (((new Vector3(GetGridPosition().x, 0, GetGridPosition().y) - transform.position).sqrMagnitude > Mathf.Epsilon) && atEnemy)
        {
            //Move the AI back towards it's original position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(GetGridPosition().x, 0, GetGridPosition().y), speed * Time.deltaTime);
            yield return null;
        }
        //Reset atEnemy bool for use again
        atEnemy = false;
    }
    #endregion

    #region Override Functions
    public override void Die()
    {
        //Remove the AI from the list of AI
        GameManager.instance.aiManager.RemoveAIFromList(gameObject);
        //Run the base Death code
        base.Die();
    }
    #endregion
}