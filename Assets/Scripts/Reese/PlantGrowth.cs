 using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlantGrowth : MonoBehaviour
{
    TurnManager turnManager;

    [SerializeField]
    private GameObject plantyBoi;

    [SerializeField]
    private int turnsToGrow = 3;

    [SerializeField]
    private AnimationCurve growthCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    private float timeToAnimate = 1.0f;

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

    //Growth variables
    private float startGrowth;
    private float currentGrowth;
    private float targetGrowth;

    private float timer;

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

        //Setup animation variables
        targetGrowth = 0.0f;
        currentGrowth = 0.0f;
        startGrowth = 0.0f;
    }

    private void FixedUpdate()
    {
        TurnUpdate();
        
        AnimatePlantGrowth();

        SpawnLivingPlant();
    }

    private void TurnUpdate()
    {
        if (currentTurn != turnManager.GetCurrentTurn() && turnManager != null)
        {
            //Update the currentTurn
            currentTurn = turnManager.GetCurrentTurn();
            //Update how many turns since planted
            growthTurn = currentTurn - plantedTurn;

            //Update the start growth to continue from current growth value
            startGrowth = currentGrowth;
            //Calculate the new growth target on the animation curve
            targetGrowth = growthCurve.Evaluate((float)growthTurn / ((float)turnsToGrow - 1));

            //Reset the timer
            timer = 0;
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
        //Once you hit the turn for the plant to finish growing spawn the living plant
        if (growthTurn >= plantedTurn + turnsToGrow)
        {
            if (plantyBoi != null) 
            {
                Instantiate(plantyBoi, transform.position, transform.rotation);
            }
            Destroy(this.gameObject);
        }
    }
}