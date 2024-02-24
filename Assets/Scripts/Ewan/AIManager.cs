using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> allyList = new List<GameObject>();
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    bool isNight = false;

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.instance.EndTurnEvent += GetListOfEntities;
        TurnManager.instance.EndTurnEvent += AIPathFindOnEndTurn;
        TurnManager.instance.EndTurnEvent += SpawnEnemies;
        TurnManager.instance.InitiateNight += ChangeToNight;
        
    }

    private void Update()
    {
        
    }
    public bool IsNight()
    {
        return isNight;
    }
    public void AddAIToList(GameObject ai)
    {
        if (ai.GetComponent<AI>() != null)
        {
            if (ai.GetComponent<AI>().IsAlly()) { allyList.Add(ai); }
            else if (!ai.GetComponent<AI>().IsAlly()) { enemyList.Add(ai); }
        }
    }
    public void RemoveAIFromList(GameObject ai)
    {
        if (ai.GetComponent<AI>() != null)
        {
            if (ai.GetComponent<AI>().IsAlly()) { allyList.Remove(ai); }
            else if (!ai.GetComponent<AI>().IsAlly()) { enemyList.Remove(ai); }
        }
    }

    public void GetListOfEntities()
    {
        foreach (GameObject placedObject in PlacementSystem.instance.placedGameObject)
        {
            if (placedObject != null && placedObject.GetComponent<AI>() != null)
            {
                if(placedObject.GetComponent<AI>().IsAlly() && !allyList.Contains(placedObject))
                {
                    allyList.Add(placedObject.gameObject);

                }
                else if(!placedObject.GetComponent<AI>().IsAlly() && !enemyList.Contains(placedObject))
                {
                    enemyList.Add(placedObject.gameObject);
                }
            }
        }
        if(GameManager.instance.house == null)
        {
            GameManager.instance.tileArray[new Vector2Int(0, 0)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
            GameManager.instance.tileArray[new Vector2Int(-1, 0)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
            GameManager.instance.tileArray[new Vector2Int(0, -1)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
            GameManager.instance.tileArray[new Vector2Int(-1, -1)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
            GameManager.instance.house = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
        }
    }

    void AIPathFindOnEndTurn()
    {
        foreach (GameObject ai in enemyList)
        {
            ai.GetComponent<AI>().AITurn();
        }
        foreach (GameObject ai in allyList)
        {
            if (ai.GetComponent<AI>().IsAlly())
            {
                //if(night)
                //Wander/Pathfind to enemy
                ai.GetComponent<AI>().AITurn();
                //else
                //Wander
            }
        }
        
        if (isNight&&(enemyList.Count == 0 && GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight))
        {
            TurnManager.instance.EndNight();
            GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight = false;
            isNight = false;
        }
    }

    void SpawnEnemies()
    {
        if (!GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight)
        {
            if (isNight) GameManager.instance.enemySpawner.SpawnEnemies();
        }
    }

    void ChangeToNight()
    {
        isNight = true;
        GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight = false;

        //Change from magic number 
        GameManager.instance.enemySpawner.ChangeSpawnBudget(5);
    }

}     
