using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private int dayLength = 4;
    [SerializeField] private int maxDayLength = 10;
    #endregion

    #region Private Variables
    private int currentRound;
    private int currentTurn;
    private int turnsTillNight;
    private bool isNight;
    #endregion

    #region Public Variables
    public static TurnManager instance;
    public event Action EndTurnEvent, FinishTurn, InitiateNight, FinishNight;
    #endregion

    #region Initalisation
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
        ResetCurrentRound();
        InitiateNight += StartNight;
        turnsTillNight = dayLength;
    }
    #endregion

    #region TurnControl
    private void ResetCurrentTurn()
    {
        currentTurn = 0;
        isNight = false;
    }
    private void IncrementCurrentTurn()
    {
        if ((currentTurn >= turnsTillNight) && !isNight) { InitiateNight?.Invoke(); }
        currentTurn++;
    }
    public int GetCurrentTurn()
    {
        return currentTurn;
    }
    public int GetTurnsTillNight()
    {
        return turnsTillNight;
    }
    public void EndTurn()
    {
        IncrementCurrentTurn();
        EndTurnEvent?.Invoke();
    }

    public void CompleteTurn()
    {
        FinishTurn?.Invoke();
    }
    #endregion

    #region RoundControl
    private void ResetCurrentRound()
    {
        currentRound = 1;
    }
    private void IncrementCurrentRound()
    {
        currentRound++;
    }
    public int GetCurrentRound()
    {
        return currentRound;
    }
    #endregion

    #region NightControl
    private void StartNight()
    {
        isNight = true;
        GameManager.instance.GetAudio().Stop();
        GameManager.instance.PlayNightAudio();
        UpdateLight();
    }
    public void EndNight()
    {
        isNight = false;
        IncrementCurrentRound();
        turnsTillNight = currentTurn + dayLength;
        GameManager.instance.RoundReward();
        GameManager.instance.GetAudio().Stop();
        GameManager.instance.PlayDayAudio();
        UpdateLight();
        FinishNight?.Invoke();
    }
    public bool GetIsNight()
    {
        return isNight;
    }
    private void UpdateLight()
    {
        if (isNight)
        {
            GameManager.instance.light.LowerIntensity();
        }
        else
        {
            GameManager.instance.light.RaiseIntensity();
        }
    }
    #endregion
}