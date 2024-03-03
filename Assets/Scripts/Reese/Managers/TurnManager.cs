using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    [SerializeField] int dayLength = 10;

    public static TurnManager instance;

    private int currentTurn;

    private int turnsTillNight;

    private bool isNight;

    public event Action EndTurnEvent, InitiateNight, FinishNight;

    private AudioSource source;

    public AudioClip dayClip;
    public AudioClip nightClip;

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

        source = gameObject.GetComponent<AudioSource>();
    }

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

    public bool GetIsNight()
    {
        return isNight;
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }

    private void IncrementCurrentTurn()
    {
        if((currentTurn >= turnsTillNight) && !isNight) { InitiateNight?.Invoke(); }
        currentTurn++;
    }

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
        turnsTillNight = currentTurn + dayLength;
        source.Stop();
        source.PlayOneShot(dayClip, source.volume);
        UpdateLight();
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
        if (isNight)
        {
            GameManager.instance.light.LowerIntensity();
        }
        else
        {
            GameManager.instance.light.RaiseIntensity();
        }
    }
}
