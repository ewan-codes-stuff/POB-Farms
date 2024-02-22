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
        if (Input.GetKeyDown(KeyCode.E))
        {
            GridManager.UpdateGridForHouse();
            GameManager.instance.aiManager.GetListOfEntities();
        }

    }

    void DebugEnemySpawn()
    {
        var GMtileArray = GameManager.instance.tileArray;

        int randPosX = Random.Range((int)-((GameManager.instance.ground.transform.localScale.x * 10) / 2) + 1, -3);//(int)((GameManager.instance.ground.transform.localScale.x*10)/2)-1);
        int randPosY = Random.Range((int)-((GameManager.instance.ground.transform.localScale.z * 10) / 2) + 1, -3);//(int)((GameManager.instance.ground.transform.localScale.z * 10) / 2)-1);
        for (int e = 0; e < maxSpawnCount; e++)
        {
            AI spawnedEnemy = GameObject.Instantiate(enemyList[0], new Vector3(GMtileArray[new Vector2Int(randPosX, randPosY)].position.x,GameManager.instance.ground.transform.position.y, GMtileArray[new Vector2Int(randPosX, randPosY)].position.y), Quaternion.identity, enemyParent.transform);
            spawnedEnemy.transform.position += new Vector3(0.0f, 0.5f, 0.0f);
            spawnedEnemy.SetGridPosition(new Vector2Int(randPosX, randPosY));
            GMtileArray[new Vector2Int(randPosX, randPosY)].entity = spawnedEnemy.GetComponent<AI>();
            Debug.Log(spawnedEnemy.GetGridPosition());
            enemyAliveList.Add(spawnedEnemy);
            //spawnedEnemy.Name = "Enemy " + GMtileArray[spawnedEnemy.GetGridPosition()].position.x + " " + GMtileArray[spawnedEnemy.GetGridPosition()].position.y;
        }
    }
    void DebugEnemyPathFind()
    {
        foreach(AI e in enemyAliveList)
        {
            e.AITurn();
        }
    }
}
