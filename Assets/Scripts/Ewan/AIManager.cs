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

    bool enemyOrAllyTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.instance.EndTurnEvent += GetListOfEntities;
        TurnManager.instance.EndTurnEvent += AIPathFindOnEndTurn;
        TurnManager.instance.EndTurnEvent += SpawnEnemies;
        TurnManager.instance.InitiateNight += ChangeToNight;
        
    }

    public bool isInAIList(AI ai)
    {
        if(ai.IsAlly())
        {
            return allyList.Contains(ai.gameObject);
        }
        if (!ai.IsAlly())
        {
            return enemyList.Contains(ai.gameObject);
        }
        else return false;
    }
    public bool IsNight()
    {
        return isNight;
    }
    public void AddAIToList(GameObject ai)
    {
        if (ai.GetComponent<AI>().IsAlly()) { allyList.Add(ai); }
        else if (!ai.GetComponent<AI>().IsAlly()) { enemyList.Add(ai); }
    }
    public void RemoveAIFromList(GameObject ai)
    {
        if (ai.GetComponent<AI>().IsAlly()) { allyList.Remove(ai); }
        if (!ai.GetComponent<AI>().IsAlly()) { enemyList.Remove(ai); }
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
        if (isNight) 
        {
            StartCoroutine(AITurnCoroutine());

            //StartCoroutine(AllyTurnCoroutine());

            if (enemyList.Count == 0 && GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight)
            {
                TurnManager.instance.EndNight();
                GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight = false;
                isNight = false;
            } 
        }
    }

    IEnumerator AITurnCoroutine()
    {
        Player.instance.FreezeInputs(true);
        foreach (GameObject ai in enemyList.ToArray())
        {
            if (ai == null) { enemyList.Remove(ai); }
            else
                {
                    ai.GetComponent<AI>().AITurn();
                    yield return new WaitForSeconds(0.1f);
                }

        }
            
        
        
        foreach (GameObject ai in allyList.ToArray())
        {
            if (ai == null) { allyList.Remove(ai); }
            //if(night)
            //Wander/Pathfind to enemy
            else
            {
                ai.GetComponent<AI>().AITurn();
                yield return new WaitForSeconds(0.1f);
            }
                //else
                //Wander

        }
        Player.instance.FreezeInputs(false);
            
        
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
