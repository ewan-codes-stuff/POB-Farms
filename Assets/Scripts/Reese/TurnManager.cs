using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnManager : MonoBehaviour
{
    [SerializeField] int numOfDayTurns = 10;

    public static TurnManager instance;

    private int currentTurn;

    public event Action EndTurn;

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

        EndTurn += IncrementCurrentTurn;
    }

    //Update used for testing turns
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            EndTurn?.Invoke();
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
