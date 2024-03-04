using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private List<GameObject> enemies;
    #endregion

    #region Public Variables
    public bool hasSpawnedEnemiesTonight = false;
    #endregion

    #region Private Variables
    //Budget used for spawning enemies in
    private int spawnBudget;
    //Stores a list of enemies that can be spawned based on the budget
    private List<GameObject> spawnPool;
    //Used to hold edges of board for spawning
    private int xSpawn;
    private int zSpawn;
    //Bool for if spawning on one of the edges on the x-axis
    private bool xWalls = false;
    //Stores the location to spawn the enemy in
    private Vector2Int posToSpawnEnemy;

    [SerializeField] private int[] setSpawnPositions = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    [SerializeField] private List<int> availableSpawnLocations;
    [SerializeField] private int minEnemyCost;

    //Stores the spawned enemy
    private GameObject spawnedEnemy;
    //Stores the spawned enemy's cost
    private int enemyCost;
    //Used for setting the stored enemy name
    private int enemyIDCounter = 0;

    //Random number store for use in many places
    private int randomNum;

    private bool canSpawnEnemies;

    //A bool for is spawns have been randomised
    private bool hasRandomisedSpawnsForNight = false;
    #endregion

    private void Start()
    {
        TurnManager.instance.EndTurnEvent += ChooseSpawnSide;
        spawnPool = new List<GameObject>();
    }

    public void SpawnEnemies()
    {
        canSpawnEnemies = true;
        availableSpawnLocations = setSpawnPositions.ToList<int>();

        //Keep spawning enemies until budget is used up
        while(canSpawnEnemies)
        {
            //Get the position to spawn the enemy
            posToSpawnEnemy = WorkoutSpawnPos();

            if (posToSpawnEnemy == new Vector2Int(0, 0))
            {
                canSpawnEnemies = false;
            }
            else if(spawnBudget >= minEnemyCost)
            {
                //For each enemy in the enemies list
                for (int j = 0; j < enemies.Count; j++)
                {
                    //If the enemy's cost is less than the currently available budget
                    if (enemies[j].gameObject.GetComponent<AI>().GetCost() <= spawnBudget)
                    {
                        //Add the enemy to the spawnPool
                        spawnPool.Add(enemies[j]);
                    }
                }
                
                //Get a random enemy from the spawn pool and spawn them at the spawn location
                if (spawnPool.Count > 0)
                {
                    Debug.Log(spawnBudget);
                    InstantiateEnemy(spawnPool[UnityEngine.Random.Range(0, spawnPool.Count)], posToSpawnEnemy);
                }
                else
                {
                    canSpawnEnemies = false;
                }
                //Reset the spawn pool
                spawnPool.Clear();
            }
            else
            {
                canSpawnEnemies = false;
                hasSpawnedEnemiesTonight = true;
            }
        }
        if (spawnBudget < minEnemyCost)
        {
            hasSpawnedEnemiesTonight = true;
        }
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
            randomNum = UnityEngine.Random.Range(1, 5);
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
        if(availableSpawnLocations.Count > 0)
        {
            //Get a new random number
            randomNum = availableSpawnLocations[UnityEngine.Random.Range(0, availableSpawnLocations.Count)];
            //If set to spawn on the "Left" or "Right" edges of the grid
            if (xWalls)
            {
                //if the randomly chosen tile is already occupided
                while (GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].entity != null)
                {
                    //Remove Position from available positions
                    availableSpawnLocations.Remove(randomNum);
                    //Regenerate a new random
                    if (availableSpawnLocations.Count > 0)
                    {
                        randomNum = availableSpawnLocations[UnityEngine.Random.Range(0, availableSpawnLocations.Count)];
                    }
                    else
                    {
                        return new Vector2Int(0, 0);
                    }
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
                    //Remove Position from available positions
                    availableSpawnLocations.Remove(randomNum);
                    //Regenerate a new random
                    if (availableSpawnLocations.Count > 0)
                    {
                        randomNum = availableSpawnLocations[UnityEngine.Random.Range(0, availableSpawnLocations.Count)];
                    }
                    else
                    {
                        return new Vector2Int(0, 0);
                    }
                }
                //Once a free space is found return the location
                return new Vector2Int(randomNum - 6, zSpawn);
            }
        }
        else
        {

            return new Vector2Int(0, 0);
        }
    }

    private void InstantiateEnemy(GameObject EnemyToSpawn, Vector2Int pos)
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
        //Remove the enemy's cost from the spawn budget
        spawnBudget -= spawnedEnemy.GetComponent<AI>().GetCost();
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