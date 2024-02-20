using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnManager : MonoBehaviour
{
    [SerializeField] int dayLength = 10;
    [SerializeField][Range(0.0f, 1.0f)] float daylightIntensity = 0.8f;
    [SerializeField][Range(1, 10)] int nightBrightness = 5;
    [SerializeField] private GameObject light;

    public static TurnManager instance;

    private int currentTurn;

    private int turnsTillNight;

    private bool isNight;

    public event Action EndTurnEvent, InitiateNight;

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
        InitiateNight += StartNight;
        turnsTillNight = dayLength;
    }

    //Update used for testing turns
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            ResetCurrentTurn();
        }
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }

    private void IncrementCurrentTurn()
    {
        if((currentTurn >= turnsTillNight) && !isNight) { InitiateNight?.Invoke(); }
        currentTurn++;
        Debug.Log(currentTurn);
    }

    private void StartNight()
    {
        isNight = true;
        light.transform.eulerAngles = new Vector3(-180.0f, -30.0f, 0.0f);
    }

    public void EndNight()
    {
        isNight = false;
        turnsTillNight = currentTurn + dayLength;
        light.transform.eulerAngles = new Vector3(50.0f, -30.0f, 0.0f);
    }

    private void ResetCurrentTurn()
    {
        currentTurn = 0;
        isNight = false;
    }

    public void EndTurn()
    {
        IncrementCurrentTurn();
        EndTurnEvent?.Invoke();
    }

    private void UpdateLight()
    {

    }
}
