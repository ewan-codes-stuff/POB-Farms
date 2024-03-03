using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    private List<AI> enemies;
    #endregion

    #region Public Variables
    public bool hasSpawnedEnemiesTonight = false;
    #endregion

    #region Private Variables
    //Budget used for spawning enemies in
    private int spawnBudget;
    //Stores a list of enemies that can be spawned based on the budget
    private List<AI> spawnPool;
    //Used to hold edges of board for spawning
    private int xSpawn;
    private int zSpawn;
    //Bool for if spawning on one of the edges on the x-axis
    private bool xWalls = false;
    //Stores the location to spawn the enemy in
    private Vector2Int posToSpawnEnemy;

    //Stores the spawned enemy
    private GameObject spawnedEnemy;
    //Stores the spawned enemy's cost
    private int enemyCost;
    //Used for setting the stored enemy name
    private int enemyIDCounter = 0;

    //Random number store for use in many places
    private int randomNum;

    //A bool for is spawns have been randomised
    private bool hasRandomisedSpawnsForNight = false;
    #endregion

    private void Start()
    {
        TurnManager.instance.EndTurnEvent += ChooseSpawnSide;
        spawnPool = new List<AI>();
    }

    public void SpawnEnemies()
    {
        //Keep spawning enemies until budget is used up
        for (int i = spawnBudget; i > 0; i -= enemyCost)
        {
            //Get the position to spawn the enemy
            posToSpawnEnemy = WorkoutSpawnPos();

            //For each enemy in the enemies list
            for(int j = 0; j < enemies.Count; j++)
            {
                //If the enemy's cost is less than the currently available budget
                if (enemies[j].gameObject.GetComponent<AI>().GetCost() <= i)
                {
                    //Add the enemy to the spawnPool
                    spawnPool.Add(enemies[j]);
                }
            }
            //Get a random enemy from the spawn pool and spawn them at the spawn location
            InstantiateEnemy(spawnPool[Random.Range(0, spawnPool.Count)], posToSpawnEnemy);
            //Reset the spawn pool
            spawnPool.Clear();
        }
        hasSpawnedEnemiesTonight = true;
        hasRandomisedSpawnsForNight = false;
    }

    public bool GetSpawnWall()
    {
        return xWalls;
    }

    public int GetSpawnColumn()
    {
        if(xWalls)
        {
            return xSpawn;
        }
        else
        {
            return zSpawn;
        }
    }

    private void ChooseSpawnSide()
    {
        if (!hasRandomisedSpawnsForNight)
        {
            randomNum = Random.Range(1, 5);
            switch (randomNum)
            {
                case 1:
                    xSpawn = 6;
                    xWalls = true;
                    break;
                case 2:
                    xSpawn = -7;
                    xWalls = true;
                    break;
                case 3:
                    zSpawn = 6;
                    xWalls = false;
                    break;
                case 4:
                    zSpawn = -7;
                    xWalls = false;
                    break;
            }
            hasRandomisedSpawnsForNight = true;
        }
    }

    private Vector2Int WorkoutSpawnPos()
    {
        //Get a new random number
        randomNum = Random.Range(0, 12);
        //If set to spawn on the "Left" or "Right" edges of the grid
        if (xWalls)
        {
            //if the randomly chosen tile is already occupided
            while (GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].entity != null)
            {
                //Regenerate a new random
                randomNum = Random.Range(0, 12);
            }
            //Once a free space is found return the location
            return new Vector2Int(xSpawn, randomNum - 6);
        }
        //If not the "Left" or "Right" edges
        else
        {
            //Check random "Top" / "Bottom" edge positions for if they are occupided
            while (GameManager.instance.tileArray[new Vector2Int(randomNum - 6, zSpawn)].entity != null)
            {
                //Regenerate a new random
                randomNum = Random.Range(0, 12);
            }
            //Once a free space is found return the location
            return new Vector2Int(randomNum - 6, zSpawn);
        }
    }

    private void InstantiateEnemy(AI EnemyToSpawn, Vector2Int pos)
    {
        //Spawn an enemy under the floor at the grid tile position
        spawnedEnemy = Instantiate(EnemyToSpawn.gameObject, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        //Add Enemy to AI Manager List
        GameManager.instance.aiManager.AddAIToList(spawnedEnemy);
        //Make sure the entity is stored on the tile
        GameManager.instance.tileArray[new Vector2Int(pos.x, pos.y)].entity = spawnedEnemy.GetComponent<Entity>();
        //Increment the enemy ID
        enemyIDCounter += 1;
        //Set the enemy's name to equal it's enemy number
        spawnedEnemy.name = "Enemy " + enemyIDCounter;
        //Set the enemy's tax to equal it's value
        enemyCost = spawnedEnemy.GetComponent<AI>().GetCost();
    }

    public void SetSpawnBudget(int budget)
    {
        spawnBudget = budget;
    }

    public bool GetHasRandomisedSpawns()
    {
        return hasRandomisedSpawnsForNight;
    }
}