using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    List<GameObject> AIList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        TurnManager.instance.EndTurnEvent += AIPathFindOnEndTurn;
    }

    void GetListOfEntities()
    {
        foreach (GameObject placedObject in PlacementSystem.instance.placedGameObject)
        {
            if (placedObject.GetComponent<AI>() != null)
            {
                AIList.Add(placedObject);
            }
        }
    }

    void AIPathFindOnEndTurn()
    {
        foreach(GameObject ai in AIList)
        {
            if(ai.GetComponent<AI>().IsAlly())
            {
                //if(night)
                //Wander/Pathfind to enemy
                ai.GetComponent<AI>().AITurn();
                //else
                //Wander
            }
            else
            {
                //pathfind to plant or house**
                ai.GetComponent<AI>().AITurn();
            }
        }
    }
}
