using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDayToNightSlider : MonoBehaviour
{
    [SerializeField] private Slider dayNightSlider;
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private int currentTurn;
    [SerializeField] private int turnsTillNight;

    // Start is called before the first frame update
    void Start()
    {
        dayNightSlider = GetComponent<Slider>();

        turnManager = TurnManager.instance;
        turnManager.FinishNight += ResetSlider;
        turnManager.EndTurnEvent += UpdateSlider;

        ResetSlider();
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        currentTurn = turnManager.GetCurrentTurn();
        dayNightSlider.value = currentTurn;
    }

    private void ResetSlider()
    {
        currentTurn = turnManager.GetCurrentTurn();
        turnsTillNight = turnManager.GetTurnsTillNight();

        dayNightSlider.minValue = currentTurn;
        dayNightSlider.maxValue = turnsTillNight;
    }
}