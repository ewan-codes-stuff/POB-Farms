using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    TurnManager turnManager;

    [SerializeField]
    private GameObject plantyBoi;

    [SerializeField]
    private int turnsToGrow = 3;

    [SerializeField]
    private AnimationCurve growthCurve;

    //Store the plant's animator
    Animator animator;
    //Store the turn the plant was planted on
    private int plantedTurn;
    //Store int to indicate how far through growth cycle we are
    private int growthState;
    //Current turn of growth
    private int growthTurn;
    //store the currentTurn
    private int currentTurn;

    private void Awake()
    {
        //Get the plant's animator
        if(this.GetComponent<Animator>() != null) animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        //Setup reference to the turn manager
        turnManager = TurnManager.instance;
        //Get the turn the plant was placed
        currentTurn = turnManager.GetCurrentTurn();
        plantedTurn = currentTurn;
        growthTurn = currentTurn - plantedTurn;
    }

    //Grow plants through 4 basic states
    //Sprout (inital)
    //Seedling
    //Budding
    //Ripe (final before becoming living plant)

    private void FixedUpdate()
    {
        if (currentTurn != turnManager.GetCurrentTurn())
        {
            currentTurn = turnManager.GetCurrentTurn();
            growthTurn = currentTurn - plantedTurn;
        }

        if (growthTurn >= turnsToGrow)
        {
            if(plantyBoi != null) Instantiate(plantyBoi, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        if(animator != null && animator.GetFloat("Growth") != null) animator.SetFloat("Growth", growthCurve.Evaluate((float)growthTurn / ((float)turnsToGrow - 1)));
    }
}