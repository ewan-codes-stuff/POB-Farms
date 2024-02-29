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

    public void SpawnEnemies()
    {
        ChooseSpawnSide();

        for (int i = spawnBudget; i > 0; i -= enemyTax)
        {
            //Increment the enemy ID
            enemyIDCounter += 1;

            InstantiateEnemy(WorkoutSpawnPos());

            hasSpawnedEnemiesTonight = true;
        }
    }

    private void ChooseSpawnSide()
    {
        randomNum = Random.Range(1, 4);
        switch (randomNum)
        {
            case 1:
                xSpawn = 5;
                xWalls = true;
                break;
            case 2:
                xSpawn = -6;
                xWalls = true;
                break;
            case 3:
                zSpawn = 5;
                xWalls = false;
                break;
            case 4:
                zSpawn = -6;
                xWalls = false;
                break;
        }
    }

    private Vector2Int WorkoutSpawnPos()
    {
        //Get a new random number
        randomNum = Random.Range(0, 11);
        //If set to spawn on the "Left" or "Right" edges of the grid
        if (xWalls)
        {
            //if the randomly chosen tile is already occupided
            while (GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].entity != null)
            {
                //Regenerate a new random
                randomNum = Random.Range(0, 11);
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
                randomNum = Random.Range(0, 11);
            }
            //Once a free space is found return the location
            return new Vector2Int(randomNum - 6, zSpawn);
        }
    }

    private void InstantiateEnemy(Vector2Int pos)
    {
        //Spawn an enemy under the floor at the grid tile position
        spawnedEnemy = Instantiate(EnemiesToSpawn[0].gameObject, new Vector3(pos.x, 0, pos.y), Quaternion.identity);

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
}