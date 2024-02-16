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
    private Grid grid;

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

    private List<GameObject> placedGameObject = new();

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
        objectData = new();
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
        if(selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Debug.Log("Grid Pos of mouse" + gridPosition);

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
        

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

    }

    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            objectData;

        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }


    public void StartPlacement(int ID)
    {
        StopPlacement();
        CameraScript.instance.zoomOnPlayer = true;
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID dound {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);

        // Adds these functions to the OnClicked/OnExit Action
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

    }

    public void StopPlacement()
    {
        CameraScript.instance.zoomOnPlayer = false;
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if(placementValidity == false)
        {
            source.clip = error;
            source.Play();
            return;
        }

        source.clip = planted;
        source.Play(); 
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObject.Add(newObject);
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            objectData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObject.Count - 1);
    }

    private void PlaceInitialObject(int ID, Vector3 pos)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        Vector3Int gridPosition = grid.WorldToCell(pos);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }
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
