 using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Plant : Entity
{
    #region Serialized Fields

    [SerializeField]
    private GameObject plantyBoi;
    [SerializeField]
    private int turnsToGrow = 3;
    [SerializeField]
    private AnimationCurve growthCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private float timeToAnimate = 1.0f;

    #endregion

    #region Private Variables

    //Store a reference to the turn manager
    private TurnManager turnManager;

    //Store the plant's animator
    private Animator animator;
    //Store the turn the plant was planted on
    private int plantedTurn;
    //Store int to indicate how far through growth cycle we are
    private int growthState;
    //Current turn of growth
    private int growthTurn;
    //store the currentTurn
    private int currentTurn;

    //Growth variables
    private float startGrowth;
    private float currentGrowth;
    private float targetGrowth;

    private float timer;

    #endregion

    private void Awake()
    {
        //Get the plant's animator
        if(this.GetComponent<Animator>() != null) animator = this.GetComponent<Animator>();
    }

    public override void Init()
    {
        base.Init();
        //Setup reference to the turn manager
        turnManager = TurnManager.instance;

        //Add the TurnUpdate to the End Turn event
        turnManager.FinishNight += TurnUpdate;

        //Get the turn the plant was placed
        currentTurn = turnManager.GetCurrentTurn();
        plantedTurn = currentTurn;
        growthTurn = currentTurn - plantedTurn;

        //Setup animation variables
        targetGrowth = 0.0f;
        currentGrowth = 0.0f;
        startGrowth = 0.0f;
    }

    private void FixedUpdate()
    {
        AnimatePlantGrowth();
    }

    private void TurnUpdate()
    {
        //Update the currentTurn
        currentTurn = turnManager.GetCurrentTurn();
        //Update how many turns since planted
        growthTurn = currentTurn - plantedTurn;

        //Update the start growth to continue from current growth value for lerping plant growth
        startGrowth = currentGrowth;

        //Calculate the new growth target on the animation curve
        targetGrowth = growthCurve.Evaluate((float)growthTurn / ((float)turnsToGrow - 1));

        //Reset the timer
        timer = 0;

        //Once you hit the turn for the plant to finish growing spawn the living plant
        if (currentTurn >= plantedTurn + turnsToGrow)
        {
            SpawnLivingPlant();
        }
    }

    private void AnimatePlantGrowth()
    {
        if (timer <= timeToAnimate) 
        {
            //Increment timer for smooth plant growth animation
            timer += Time.deltaTime;

            //Store the plant's current growth value
            currentGrowth = Mathf.Lerp(startGrowth, targetGrowth, timer / timeToAnimate);

            //Set the plant's animator to the current growth value
            if (animator != null) animator.SetFloat("Growth", currentGrowth);
        }
    }

    private void SpawnLivingPlant()
    {
        if (plantyBoi != null)
        {
            //Spawn fully grown plant
            Instantiate(plantyBoi, transform.position, transform.rotation);
        }
        Die();
    }

    public override void Die()
    {
        turnManager.EndTurnEvent -= TurnUpdate;
        base.Die();
    }
}