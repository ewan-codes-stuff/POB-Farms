using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class TurnManager : MonoBehaviour
{
    [SerializeField] int numOfDayTurns = 10;

    public static TurnManager instance;

    private int currentTurn;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void Start()
    {
        ResetCurrentTurn();
    }

    //Update used for testing turns
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        { 
            IncrementCurrentTurn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetCurrentTurn();
        }
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }

    public void IncrementCurrentTurn()
    {
        currentTurn++;
    }

    public void ResetCurrentTurn()
    {
        currentTurn = 0;
    }
}
