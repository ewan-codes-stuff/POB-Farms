using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    TurnManager turnManager;

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
        animator = this.GetComponent<Animator>();
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

    private void Update()
    {
        if (currentTurn != turnManager.GetCurrentTurn())
        {
            currentTurn = turnManager.GetCurrentTurn();
            growthTurn = currentTurn - plantedTurn;
        }

        if (growthCurve.Evaluate(growthTurn / turnsToGrow) >= 1f)
        {
            //Turn into living plant
        }
        else if(growthCurve.Evaluate(growthTurn / turnsToGrow) >= 0.75f)
        {
            //Ripe
        }
        else if (growthCurve.Evaluate(growthTurn / turnsToGrow) >= 0.5f)
        {
            //Budding
        }
        else if (growthCurve.Evaluate(growthTurn / turnsToGrow) >= 0.25f)
        {
            //Seedling
        }
    }
}