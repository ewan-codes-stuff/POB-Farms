using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementSystem : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    [SerializeField]
    private InputManager inputManager;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;

    public event Action OnClicked, OnExit;

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private GameObject gridVisualization;

    private Renderer[] previewRenderer;


    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip shuffle;
    [SerializeField]
    private AudioClip error;

    // Singleton instance
    public static MovementSystem instance;

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



    public void Start()
    {
        StopMovement();
        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = PlacementSystem.instance.CheckPlacementValidity(gridPosition, 1);
        foreach (Renderer rend in previewRenderer)
        {
            if (placementValidity)
            {
                rend.material.color = Color.yellow;
            }
            else
            {
                rend.material.color = Color.red;
            }
        }

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }


    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            Debug.Log(hit.collider.GetComponent<GameObject>().name);
            lastPosition = hit.point;
        }

        return lastPosition;
    }

    public void StartMovement()
    {

        StopMovement();
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);


        inputManager.OnClicked += MovePlayer;
        inputManager.OnExit += StopMovement;
        
    }

    public void StopMovement()
    {
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);

        inputManager.OnClicked -= MovePlayer;
        inputManager.OnExit -= StopMovement;
    }

    private void MovePlayer()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = PlacementSystem.instance.CheckPlacementValidity(gridPosition, 1);
        if (placementValidity == false)
        {
            source.clip = error;
            source.Play();
            return;
        }
        source.clip = shuffle;
        source.Play();

        gameObject.transform.position = grid.CellToWorld(gridPosition);

    }

}
