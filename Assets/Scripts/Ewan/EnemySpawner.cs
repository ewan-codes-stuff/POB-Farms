using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int spawnBudget = 10;
    int spawnTimer = 1;
    int spawnLocationRotator = 0;
    [SerializeField]
    List<Vector2Int> locationsToSpawn;

    public List<AI> EnemiesToSpawn;

    int enemyIDCounter = 0;

    bool isNight = false;

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
        TurnManager.instance.EndTurnEvent += SpawnEnemies;
    }

    public void ChangeTimePeriod(bool isNightTrue)
    {
        isNight = isNightTrue;
    }

    public void ChangeSpawnBudget(int budget)
    {
        spawnBudget = budget;
    }
    // Update is called once per frame
    void Update()
    {
        if(spawnLocationRotator < 1) { spawnLocationRotator = -1; }
    }

    void SpawnEnemies()
    {
        enemyIDCounter += 1;
        Debug.Log("Spawned Enemies");
        spawnTimer -= 1;
        if (isNight)
        {
            if (spawnBudget > 0 && spawnTimer <= 0)
            {
                spawnTimer = 1;
                spawnLocationRotator += 1;
                GameObject spawnedEnemy = Instantiate(EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count)].gameObject, new Vector3(GameManager.instance.tileArray[locationsToSpawn[spawnLocationRotator]].position.x, 0.0f, GameManager.instance.tileArray[locationsToSpawn[spawnLocationRotator]].position.y), Quaternion.identity);
                Debug.Log(spawnedEnemy.GetComponent<AI>().GetGridPosition());
                spawnedEnemy.GetComponent<AI>().SetGridPosition(new Vector2Int((int)spawnedEnemy.transform.position.x, (int)spawnedEnemy.transform.position.z));
                GameManager.instance.tileArray[locationsToSpawn[spawnLocationRotator]].entity = spawnedEnemy.GetComponent<AI>();
                spawnedEnemy.name = "Enemy " + enemyIDCounter;
                GameManager.instance.aiManager.AddAIToList(spawnedEnemy.gameObject);
                hasSpawnedEnemiesTonight = true;
                spawnBudget -= 1;
            }
        }
    }

    
}
