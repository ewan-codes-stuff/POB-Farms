using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
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


    public GridData floorData, objectData;

    private Renderer[] previewRenderer;

    public List<GameObject> placedGameObject = new List<GameObject>();

    // Singleton instance
    public static PlacementSystem instance;

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
        StopPlacement();

        gridVisualization.SetActive(false);

        floorData = new GridData();
        objectData = new GridData();
        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();

        // Places the initial house
        PlaceInitialObject(10, new Vector3(-1, 0, -1));
        
        // Places bounds around the map so the player cant plant or move off map
        for (int i = -7; i < 7; i++)
        {
            PlaceInitialObject(100, new Vector3(-7, 0, i));
            PlaceInitialObject(100, new Vector3(6, 0, i));

            PlaceInitialObject(100, new Vector3(i, 0, -7));
            PlaceInitialObject(100, new Vector3(i, 0, 6));
        }
    }

    

    
    private void Update()
    {
        // Quick check to see if input is valid
        if(selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Checks if the placement position is valid
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        foreach(Renderer rend in previewRenderer)
        {
            if (placementValidity)
            {
                rend.material.color = Color.green;
            }
            else
            {
                rend.material.color = Color.red;
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
            floorData :
            objectData;

        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }


    public void StartPlacement(int ID)
    {
        if (!Player.instance.freezePlayer)
        {
            // Initially stops placement to make sure all variables are reset just incase
            StopPlacement();

            // Zooms in on player for a cool effect
            CameraScript.instance.StartLerp(3.5f, true);

            // Makes sure ID for the placed object is valid
            selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
            if (selectedObjectIndex < 0)
            {
                Debug.LogError($"No ID dound {ID}");
                // Return out as place is not valid
                return;
            }

            // Activates the grid for the player to see
            gridVisualization.SetActive(true);
            cellIndicator.SetActive(true);

            // Adds these functions to the OnClicked/OnExit Action
            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        else
        {
            StopPlacement();
        }


    }

    public void StopPlacement()
    {
        // Resets all initial values 
        CameraScript.instance.StartLerp(9.5f, true);
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);

        // Removes these functions to the OnClicked/OnExit Action
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void PlaceStructure()
    {
        // Gets the cost of the plant being attempted to be selected
        int plantCost = 0;
        if (database.objectsData[selectedObjectIndex].Prefab.GetComponent<Entity>())
        {
            plantCost = database.objectsData[selectedObjectIndex].Prefab.GetComponent<Entity>().GetCost();
        }
        
        // Checks if player has enough currency to place object
        if (plantCost <= Player.instance.GetCurrency()) 
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
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            if(placementValidity == false)
            {
                source.clip = error;
                source.Play();
                return;
            }

            // Placement begins so depreciate currency, play audio and instantate the releveant vegeteble
            Player.instance.DecreaseCurrency(plantCost);
            source.clip = planted;
            source.Play(); 
            GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
            //Save object's Grid pos to the entity
            if(newObject.GetComponent<Entity>() != null) { newObject.GetComponent<Entity>().SetGridPosition(new Vector2Int(gridPosition.x, gridPosition.z)); }
            newObject.transform.position = grid.CellToWorld(gridPosition);

            // Adding it to the dictionary of placed objects
            placedGameObject.Add(newObject);

            //Add To Ewan Grid Data
            GameManager.instance.tileArray[new Vector2Int(gridPosition.x, gridPosition.z)].entity = newObject.GetComponent<Entity>();

            GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
                floorData:
                objectData;
            selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObject.Count - 1);
        }
        else
        {
            // If not enough currency then play error sound
            source.clip = error;
            source.Play();
        }
    }

    private void PlaceInitialObject(int ID, Vector3 pos)
    {
        // Initially stops placement to make sure all variables are reset just incase
        StopPlacement();

        // Gets ID of the object being placed
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        // Gets the position on the grid from worldspace
        Vector3Int gridPosition = grid.WorldToCell(pos);

        // Checks the object can actually be placed
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }

        // Instantiates the object and adds it to the list of placed objects
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObject.Add(newObject);
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            objectData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObject.Count - 1);

        inputManager.OnExit += StopPlacement;
    }

}
