using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int spawnBudget = 1;
    int spawnLocationRotator = 0;
    [SerializeField]
    List<Vector2Int> locationsToSpawn;

    int xSpawn = -6;
    int zSpawn = -6;

    bool xWalls = false;

    public List<AI> EnemiesToSpawn;

    int enemyIDCounter = 0;

    int enemyTax = 1;

    GameObject spawnedEnemy;

    int randomNum;

    public bool hasSpawnedEnemiesTonight = false;

    private bool hasRandomisedSpawnsForNight = false;

    private void Start()
    {
        TurnManager.instance.EndTurnEvent += ChooseSpawnSide;
        //Randomise which side you spawn on
    }

    public void SpawnEnemies()
    {
        //ChooseSpawnSide(); //Remove this from here and do it earlier
                           //Then use the Danger Indicator
        for (int i = spawnBudget; i > 0; i -= enemyTax)
        {
            //Increment the enemy ID
            enemyIDCounter += 1;

            InstantiateEnemy(WorkoutSpawnPos());

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
        randomNum = Random.Range(0, 13);
        //If set to spawn on the "Left" or "Right" edges of the grid
        if (xWalls)
        {
            //if the randomly chosen tile is already occupided
            while (GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].entity != null)
            {
                //Regenerate a new random
                randomNum = Random.Range(0, 13);
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
                randomNum = Random.Range(0, 13);
            }
            //Once a free space is found return the location
            return new Vector2Int(randomNum - 6, zSpawn);
        }
    }

    private void InstantiateEnemy(Vector2Int pos)
    {
        //Spawn an enemy under the floor at the grid tile position
        spawnedEnemy = Instantiate(EnemiesToSpawn[0].gameObject, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        //Add Enemy to AI Manager List
        GameManager.instance.aiManager.AddAIToList(spawnedEnemy);
        //Make sure the entity is stored on the tile
        GameManager.instance.tileArray[new Vector2Int(pos.x, pos.y)].entity = spawnedEnemy.GetComponent<Entity>();
        //Set the enemy's name to equal it's enemy number
        spawnedEnemy.name = "Enemy " + enemyIDCounter;
        //Set the enemy's tax to equal it's value
        enemyTax = spawnedEnemy.GetComponent<AI>().GetCost();
    }

    public void ChangeSpawnBudget(int budget)
    {
        spawnBudget = budget;
    }

    public bool GetHasRandomisedSpawns()
    {
        return hasRandomisedSpawnsForNight;
    }
}