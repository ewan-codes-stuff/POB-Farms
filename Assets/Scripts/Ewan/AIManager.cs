using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> allyList = new List<GameObject>();
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.instance.EndTurnEvent += AIPathFindOnEndTurn;
    }

    public void GetListOfEntities()
    {
        foreach (GameObject placedObject in PlacementSystem.instance.placedGameObject)
        {
            if (placedObject.GetComponent<AI>() != null)
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
    }

    void AIPathFindOnEndTurn()
    {
        //if(Is Night Time)
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
        foreach (GameObject ai in enemyList)
        {
            ai.GetComponent<AI>().AITurn();
        }
    }


}     
