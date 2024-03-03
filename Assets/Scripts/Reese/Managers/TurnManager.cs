using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private int dayLength = 10;
    [SerializeField] private AudioClip dayClip;
    [SerializeField] private AudioClip nightClip;
    #endregion

    #region Private Variables
    private int currentRound;
    private int currentTurn;
    private int turnsTillNight;
    private bool isNight;
    private AudioSource source;
    #endregion

    #region Public Variables
    public static TurnManager instance;
    public event Action EndTurnEvent, InitiateNight, FinishNight;
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

        source = gameObject.GetComponent<AudioSource>();
    }
    #endregion

    //Debug Checks only
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndNight();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EndTurn();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

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
    public void EndTurn()
    {
        IncrementCurrentTurn();
        EndTurnEvent?.Invoke();
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
        source.Stop();
        source.PlayOneShot(nightClip, source.volume);
        UpdateLight();
    }
    public void EndNight()
    {
        isNight = false;
        IncrementCurrentRound();
        turnsTillNight = currentTurn + dayLength;
        source.Stop();
        source.PlayOneShot(dayClip, source.volume);
        UpdateLight();
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