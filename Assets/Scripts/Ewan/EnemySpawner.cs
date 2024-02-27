using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int spawnBudget = 1;
    int spawnTimer = 1;
    int spawnLocationRotator = 0;
    [SerializeField]
    List<Vector2Int> locationsToSpawn;

    int xSpawn = -6;
    int zSpawn = -6;

    bool xRandomised = false;
    bool zRandomised = false;

    public List<AI> EnemiesToSpawn;

    int enemyIDCounter = 0;

    int enemyTax = 1;

    GameObject spawnedEnemy;

    public bool hasSpawnedEnemiesTonight = false;
    
    //Things we need for spawner
    /*
    Locations to spawn in
    To pass the enemy into the AI Manager
    When to spawn enemies
    Enemy Types
     
    */

    public void ChangeSpawnBudget(int budget)
    {
        spawnBudget = budget;
    }
    // Update is called once per frame
    void Update()
    {
     
    }

    public void SpawnEnemies()
    {
        //Random side to spawn from: 1 - x=5, 2- x=-6 3- z=5, 4- z=-6
        int spawnSide = Random.Range(1, 4);
        switch (spawnSide) 
        {
            case 1:
                xSpawn = 5;
                xRandomised = true;
                zRandomised = false;
                break;
            case 2:
                xSpawn = -6;
                xRandomised = true;
                zRandomised = false;
                break;
            case 3:
                zSpawn = 5;
                zRandomised = true;
                xRandomised = false;
                break;
            case 4:
                zSpawn = -6;
                zRandomised = true;
                xRandomised = false;
                break;

        }
        
        Debug.Log("Spawned Enemies");
        spawnTimer -= 1;

        //This is called before running this function this is unnecessary
        if (GameManager.instance.aiManager.IsNight())
        {
            for(int m = spawnBudget; m > 0; m -= enemyTax)
            {
                enemyIDCounter += 1;
                int randomNum = Random.Range(0, 11);
                if (xRandomised)
                { 
                    while(GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].entity != null)
                    {
                        randomNum = Random.Range(0, 11);
                    }
                    SpawnEnemy(xSpawn, randomNum - 6);
                }
                else 
                {
                    while (GameManager.instance.tileArray[new Vector2Int(randomNum - 6, zSpawn)].entity != null)
                    {
                        randomNum = Random.Range(0, 11);
                    }
                    SpawnEnemy(randomNum - 6, zSpawn);
                }

                hasSpawnedEnemiesTonight = true;
            }
        }
    }

    private void SpawnEnemy(int x, int z)
    {
        spawnedEnemy = Instantiate(EnemiesToSpawn[0].gameObject, new Vector3(x, 0, z), Quaternion.identity);
        GameManager.instance.tileArray[new Vector2Int(x, z)].entity = spawnedEnemy.GetComponent<Entity>();
        spawnedEnemy.name = "Enemy " + enemyIDCounter;
        enemyTax = spawnedEnemy.GetComponent<AI>().GetCost();
    }
}
