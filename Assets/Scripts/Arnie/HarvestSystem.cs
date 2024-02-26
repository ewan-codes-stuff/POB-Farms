using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    public Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip planted;
    [SerializeField]
    private AudioClip error;


    public GridData objectData;

    private Renderer[] previewRenderer;

    public List<GameObject> placedGameObject = new List<GameObject>();

    // Singleton instance
    public static HarvestSystem instance;

    public void Awake()
    {
        // Initialise Singleton
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        instance = this;
    }

    private void Start()
    {
        StopHarvest();

        gridVisualization.SetActive(false);

        objectData = GameManager.instance.GetObjectData();
        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();

    }




    private void Update()
    {

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Checks if the placement position is valid
        bool placementValidity = PlacementSystem.instance.CheckPlacementValidity(gridPosition, 1);
        foreach (Renderer rend in previewRenderer)
        {
            if (placementValidity)
            {
                rend.material.color = Color.red;
            }
            else
            {
                rend.material.color = Color.green;
            }
        }

        // Positions the indicators for when moving the placement
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

    }

    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // Checks if the object exists in the placement position
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            objectData :
            objectData;

        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public GameObject GetAllyGameobject(Vector3Int gridPosition)
    {
        return GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.z)].entity.gameObject;

    }


    public bool CheckIfAlly(Vector3Int gridPosition)
    {
        if (GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.z)].entity != null)
        {
            return GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.z)].entity.IsAlly();
        }
        else
        {
            return false;
        }

        
    }


    public void StartHarvest()
    {
        // Initially stops harvest to make sure all variables are reset just incase
        StopHarvest();

        // Activates the grid for the player to see
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);

        // Adds these functions to the OnClicked/OnExit Action
        inputManager.OnClicked += HarvestVegetable;
        inputManager.OnExit += StopHarvest;

    }

    public void StopHarvest()
    {
        // Resets all initial values 
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);

        // Removes these functions to the OnClicked/OnExit Action
        inputManager.OnClicked -= HarvestVegetable;
        inputManager.OnExit -= StopHarvest;
    }

    private void HarvestVegetable()
    {

        // Doesnt do anything if player is hovering over UI elements
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        // Finds the position on the grid
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Compares the position being attempted to place on to the dictionary to see if its valid
        bool vegetableValidity = CheckIfAlly(gridPosition);
        if (vegetableValidity == false)
        {
            source.clip = error;
            source.Play();
            return;
        }

        // Placement begins so depreciate currency, play audio and instantate the releveant vegeteble
        source.clip = planted;
        source.Play();


        GameObject temp = GetAllyGameobject(gridPosition);

        // adds the price to the players currency
        Player.instance.AddCurrency(50);

        //// Remove this gameobject from the placed objects list
        //GameManager.instance.GetPlacedObjects().Remove(GetAllyGameobject(gridPosition));

        ////Remove the existing plant from the gridData
        //PlacementSystem.instance.objectData.RemoveObjectAt(PlacementSystem.instance.grid.WorldToCell(GetAllyGameobject(gridPosition).transform.position), new Vector2Int(1, 1));

        ////Remove from this stupid other thing
        //GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.z)].entity = null;



        if (temp.TryGetComponent<Plant>(out Plant plant))
        {
            plant.Die();
        }
        else
        {
            temp.GetComponent<Entity>().Die();
        }
    }
}
