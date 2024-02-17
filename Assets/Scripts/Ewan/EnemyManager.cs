using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemyParent;
    public List<AI> enemyList;
    public List<AI> enemyAliveList;
    public int maxSpawnCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DebugEnemySpawn();
            Debug.Log("Escape key pressed");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            DebugEnemyPathFind();
            Debug.Log("P key pressed");
        }
    }

    void DebugEnemySpawn()
    {
        var GMtileArray = GameManager.instance.tileArray;

        int randPosX = Random.Range(0, (int)GameManager.instance.ground.transform.localScale.x*10);
        int randPosY = Random.Range(0, (int)GameManager.instance.ground.transform.localScale.z * 10);
        for (int e = 0; e < maxSpawnCount; e++)
        {
            AI spawnedEnemy = GameObject.Instantiate(enemyList[0], new Vector3(GMtileArray[new Vector2Int(randPosX, randPosY)].position.x,GameManager.instance.ground.transform.position.y, GMtileArray[new Vector2Int(randPosX, randPosY)].position.y), Quaternion.identity, enemyParent.transform);
            spawnedEnemy.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
            spawnedEnemy.SetGridPosition(new Vector2Int(randPosX, randPosY));
            Debug.Log(spawnedEnemy.GetGridPosition());
            enemyAliveList.Add(spawnedEnemy);
        }


    }
    void DebugEnemyPathFind()
    {
        foreach(AI e in enemyAliveList)
        {
            e.EnemyTurnPathFind();
        }
    }
}
