using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnManager : MonoBehaviour
{
    [SerializeField] int numOfDayTurns = 10;
    [SerializeField][Range(0.0f, 1.0f)] float daylightIntensity = 0.8f;
    [SerializeField][Range(1, 10)]int nightBrightness = 5;

    public static TurnManager instance;

    private int currentTurn;

    public event Action EndTurnEvent;

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
        Debug.Log(currentTurn % numOfDayTurns);
        EndTurnEvent += IncrementCurrentTurn;
    }

    //Update used for testing turns
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            EndTurnEvent?.Invoke();
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

    public void LerpDayNight() 
    {
        Debug.Log(currentTurn % numOfDayTurns);
    }

    public void ResetCurrentTurn()
    {
        currentTurn = 0;
    }

    public void EndTurn()
    {
        EndTurnEvent?.Invoke();
    }
}
