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
        int spawnSide = Random.Range(1, 5);
        switch (spawnSide) 
        {
            case 1:
                xSpawn = 6;
                xRandomised = true;
                zRandomised = false;
                break;
            case 2:
                xSpawn = -7;
                xRandomised = true;
                zRandomised = false;
                break;
            case 3:
                zSpawn = 6;
                zRandomised = true;
                xRandomised = false;
                break;
            case 4:
                zSpawn = -7;
                zRandomised = true;
                xRandomised = false;
                break;

        }
            
        
        Debug.Log("Spawned Enemies");
        spawnTimer -= 1;
        if (GameManager.instance.aiManager.IsNight()) //Check if night
        {
            for(int m = spawnBudget; m>0; m -= enemyTax) //Loop until you've spawned all the enemies for this round
            {
                int randomiseSpawnCounter = 0; //Counter to prevent infinite while loops by setting a limit (which is currently 100 randomisations)
                enemyIDCounter += 1; //To name the enemy in the heirachy
                int randomNum = Random.Range(0, 11); 
                if (xRandomised) 
                { 
                    while(GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].entity != null) //If the proposed spawning location is occupied by an entity,
                                                                                                                //randomise for a new spawn
                    {
                        randomNum = Random.Range(0, 11);
                        randomiseSpawnCounter += 1;
                        if(randomiseSpawnCounter > 100)     //If the spawn counter has gone over 100, spawn 1 closer to the centre until we reach 0 or find a space
                        {
                            if(xSpawn > 0) { xSpawn--; randomiseSpawnCounter = 0; }
                            if(xSpawn < 0) { xSpawn++; randomiseSpawnCounter = 0; }
                            if(xSpawn == 0) { break; }
                        }
                    }
                    if (GameManager.instance.tileArray[new Vector2Int(xSpawn, randomNum - 6)].entity == null) //If there is nothing there, spawn an enemy, otherwise do nothing
                    {
                        SpawnEnemy(xSpawn, randomNum - 6);
                        hasSpawnedEnemiesTonight = true; //Boolean to check that something has been spawned in the round. For the AIManager
                    }
                }
                else 
                {
                    while (GameManager.instance.tileArray[new Vector2Int(randomNum - 6, zSpawn)].entity != null)//If the proposed spawning location is occupied by an entity,
                                                                                                                //randomise for a new spawn
                    {
                        randomNum = Random.Range(0, 11);
                        randomiseSpawnCounter += 1;
                        if (randomiseSpawnCounter > 100)    //If the spawn counter has gone over 100, spawn 1 closer to the centre until we reach 0 or find a space
                        {
                            if (zSpawn > 0) { zSpawn--; randomiseSpawnCounter = 0; }
                            if (zSpawn < 0) { zSpawn++; randomiseSpawnCounter = 0; }
                            if (zSpawn == 0) { break; }
                        }
                    }
                    if (GameManager.instance.tileArray[new Vector2Int(randomNum - 6, zSpawn)].entity == null)   //If there is nothing there, spawn an enemy, otherwise do nothing
                    {
                        SpawnEnemy(randomNum - 6, zSpawn);
                        hasSpawnedEnemiesTonight = true;    //Boolean to check that something has been spawned in the round. For the AIManager
                        
                    }
                }
                
            }
        }
    }

    void SpawnEnemy(int SpawnX, int SpawnZ)
    {
        //Instatiates enemy game object
        GameObject spawnedEnemy = Instantiate(EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count)].gameObject, new Vector3(GameManager.instance.tileArray[new Vector2Int(SpawnX, SpawnZ)].position.x, 0.0f, GameManager.instance.tileArray[new Vector2Int(SpawnX, SpawnZ)].position.y), Quaternion.identity);
        Debug.Log(spawnedEnemy.GetComponent<AI>().GetGridPosition());
        GameManager.instance.aiManager.AddAIToList(spawnedEnemy); //Adds it to the AI Manager List
        spawnedEnemy.GetComponent<AI>().SetGridPosition(new Vector2Int((int)spawnedEnemy.transform.position.x, (int)spawnedEnemy.transform.position.z));
        spawnedEnemy.name = "Enemy " + enemyIDCounter; //Gives it a useful name in the heirarchy
        GameManager.instance.tileArray[new Vector2Int(SpawnX, SpawnZ)].entity = spawnedEnemy.GetComponent<AI>(); //Makes the enemy occupy the grid Tile entity variable
        enemyTax = spawnedEnemy.GetComponent<AI>().GetCost(); //Changes the tax variable on the spawner so the correct amount of points is taken away from the budget
    }    
}
