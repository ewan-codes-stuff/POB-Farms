 using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Plant : Entity
{
    TurnManager turnManager;

    [SerializeField]
    private const int cost = 5;

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
        if (currentTurn >= plantedTurn + turnsToGrow)
        {
            //When turning into an ally pay out cash reward
            Player.instance.AddCurrency(cost * 2);
            if (plantyBoi != null) 
            {
                PlacementSystem.instance.objectData.RemoveObjectAt(PlacementSystem.instance.grid.WorldToCell(transform.position), new Vector2Int(1, 1));
                GameObject temp = Instantiate(plantyBoi, transform.position, transform.rotation);

                //Setup the new GameObject's GridPosition
                if (temp.GetComponent<Entity>()) { temp.GetComponent<Entity>().SetGridPosition(GetGridPosition()); }

                //Add the living Carrot to the placed objects list
                GameManager.instance.GetPlacedObjects().Add(temp);

                //Add to object data
                GameManager.instance.GetObjectData().AddObjectAt(new Vector3Int(GetGridPosition().x, 0, GetGridPosition().y) , new Vector2Int(1,1), 2100, GameManager.instance.GetPlacedObjects().Count - 1);

                //Add to this stupid other thing
                GameManager.instance.tileArray[GetGridPosition()].entity = temp.GetComponent<Entity>();
            }
            Destroy(this.gameObject);
        }
    }

    public int GetCost()
    {
        return cost;
    }
}