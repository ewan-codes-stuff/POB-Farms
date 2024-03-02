using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    private int enemyBudget = 3;
    [SerializeField]
    private List<GameObject> aiList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> enemyList = new List<GameObject>();

    bool isNight = false;

    // Start is called before the first frame update
    void Start()
    {
        //Subscribe functions to all relevant Events
        TurnManager.instance.EndTurnEvent += GetListOfEntities;
        TurnManager.instance.EndTurnEvent += RunAITurn;
        TurnManager.instance.EndTurnEvent += SpawnEnemies;
        TurnManager.instance.EndTurnEvent += EndNight;
        TurnManager.instance.InitiateNight += ChangeToNight;
    }
    
    public bool IsInAIList(AI ai)
    {
        //If the AI in the AI list return true otherwise return false
        return aiList.Contains(ai.gameObject);
    }
    public void AddAIToList(GameObject ai)
    {
        //Add the AI to the AI list
        aiList.Add(ai);
        //If the AI is an Enemy
        if (!ai.GetComponent<AI>().IsAlly())
        {
            //Add the AI to the Enemy list
            enemyList.Add(ai);
        }
    }
    public void RemoveAIFromList(GameObject ai)
    {
        //Remove the AI from the AI list
        aiList.Remove(ai);
        //If the AI is and Enemy
        if (!ai.GetComponent<AI>().IsAlly())
        {
            //Remove the AI from the Enemy list
            enemyList.Remove(ai);
        }
    }

    //You fucking twat why is this called every single turn?!
    public void GetListOfEntities()
    {
        //Check every object in the placed objects list
        foreach (GameObject placedObject in PlacementSystem.instance.placedGameObject)
        {
            //If the object is not null and is has an AI component
            if (placedObject != null && placedObject.GetComponent<AI>() != null)
            {
                //Check if the object is already in an AI list
                if (!IsInAIList(placedObject.GetComponent<AI>()))
                {
                    //If not add AI to the AI list
                    AddAIToList(placedObject);
                }
            }
        }

        //Hardcoded ungabunga house why? fuck you
        GameManager.instance.tileArray[new Vector2Int(0, 0)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
        GameManager.instance.tileArray[new Vector2Int(-1, 0)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
        GameManager.instance.tileArray[new Vector2Int(0, -1)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
        GameManager.instance.tileArray[new Vector2Int(-1, -1)].entity = PlacementSystem.instance.placedGameObject[0].gameObject.GetComponent<Entity>();
    }

    void RunAITurn()
    {
        Player.instance.FreezeInputs(true);

        StartCoroutine(WaitForCoroutine(RunAI()));
    }

    

    void EndNight()
    {
        //If the enemy count is 0 and enemys have been spawned tonight
        if (isNight && (enemyList.Count == 0 && GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight))
        {
            //Call the end night event
            TurnManager.instance.EndNight();
            //Return the has spawned enemies check to false
            GameManager.instance.enemySpawner.hasSpawnedEnemiesTonight = false;
            //Set isNight bool to false
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

        GameManager.instance.enemySpawner.ChangeSpawnBudget(enemyBudget);
    }

    #region Coroutines
    private IEnumerator WaitForCoroutine(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);

        Player.instance.FreezeInputs(false);
    }

    private IEnumerator RunAI()
    {
        //For every AI in the AI list
        for (int i = 0; i < aiList.Count; i++)
        {
            //Check if the AI is null
            if (aiList[i] == null)
            {
                //If null remove the AI from the AI list
                aiList.Remove(aiList[i]);
                //If the AI is in the Enemy list remove it from there too
                if (enemyList.Contains(aiList[i])) enemyList.Remove(aiList[i]);
            }
            //If the AI isn't null
            else
            {
                //If the AI has an action to take
                yield return aiList[i].GetComponent<AI>().AITurn();
            }
        }
    }
    #endregion
}
