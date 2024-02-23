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

    

    public bool hasSpawnedEnemiesTonight = false;

    public bool debugSpawnEnemies = false;
    
    //Things we need for spawner
    /*
    Locations to spawn in
    To pass the enemy into the AI Manager
    When to spawn enemies
    Enemy Types
     
    */

    // Start is called before the first frame update
    void Start()
    {
    }

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
            
        enemyIDCounter += 1;
        Debug.Log("Spawned Enemies");
        spawnTimer -= 1;
        if (GameManager.instance.aiManager.IsNight())
        {
            for(int m = spawnBudget; m>0; m -= enemyTax)
            {
                GameObject spawnedEnemy = null;
                int randomNum = Random.Range(0, 11);
                if (xRandomised) { spawnedEnemy = Instantiate(EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count)].gameObject, new Vector3(GameManager.instance.tileArray[new Vector2Int(xSpawn,randomNum-6)].position.x, 0.0f, GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].position.y), Quaternion.identity); }
                else { spawnedEnemy = Instantiate(EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count)].gameObject, new Vector3(GameManager.instance.tileArray[new Vector2Int(randomNum - 6, zSpawn)].position.x, 0.0f, GameManager.instance.tileArray[new Vector2Int(randomNum - 6, zSpawn)].position.y), Quaternion.identity); }
                Debug.Log(spawnedEnemy.GetComponent<AI>().GetGridPosition());
                spawnedEnemy.GetComponent<AI>().SetGridPosition(new Vector2Int((int)spawnedEnemy.transform.position.x, (int)spawnedEnemy.transform.position.z));
                GameManager.instance.tileArray[new Vector2Int((int)spawnedEnemy.transform.position.x, (int)spawnedEnemy.transform.position.z)].entity = spawnedEnemy.GetComponent<AI>();
                spawnedEnemy.name = "Enemy " + enemyIDCounter;
                GameManager.instance.aiManager.AddAIToList(spawnedEnemy.gameObject);
                hasSpawnedEnemiesTonight = true;
                enemyTax = spawnedEnemy.GetComponent<AI>().GetCost();
            }
        }
    }

    
}
