using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int spawnBudget = 1;
    int spawnTimer = 1;
    [SerializeField]
    List<Vector2Int> locationsToSpawn;

    public List<GameObject> EnemiesToSpawn;
    
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        spawnTimer -= 1;
        //if night
        if(spawnBudget < 0 && spawnTimer == 0)
        {
            GameObject.Instantiate(EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count)],new Vector3(GameManager.instance.tileArray[locationsToSpawn[0]].position.x,0.0f, GameManager.instance.tileArray[locationsToSpawn[0]].position.y),Quaternion.identity);
        }
    }
}
