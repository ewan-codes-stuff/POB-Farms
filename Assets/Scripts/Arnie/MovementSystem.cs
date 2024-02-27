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

    public SpriteRenderer playerSprite;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip shuffle;
    [SerializeField]
    private AudioClip error;

    public bool movePlayer = false;
    public Vector3 playerToMovePos;
    public Vector3 initialPlayerPos;
    float timeElapsed = 0;
    float moveDelayTimer = 0;
    public float lerpDuration = 3;

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
        gameObject.GetComponent<Entity>().AddEntityToGrids(gameObject);
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

        if (movePlayer)
        {
            // Lerps the player to move to the tile its targeting
            if (timeElapsed < lerpDuration)
            {
                cellIndicator.SetActive(false);
                gridVisualization.SetActive(false);
                gameObject.transform.position = Vector3.Lerp(initialPlayerPos, playerToMovePos, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                moveDelayTimer += Time.deltaTime;

                // Adds a little bit of a delay before resetting the movement
                if (moveDelayTimer > 0.25f)
                {
                    cellIndicator.SetActive(true);
                    gridVisualization.SetActive(true);
                    transform.position = playerToMovePos;
                    timeElapsed = 0;
                    moveDelayTimer = 0;
                    movePlayer = false;
                    TurnManager.instance.EndTurn();
                }
                
            }
        }
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
        if (!Player.instance.freezePlayer)
        {
            StopMovement();
            CameraScript.instance.zoomOnPlayer = true;
            gridVisualization.SetActive(true);
            cellIndicator.SetActive(true);


            inputManager.OnClicked += MovePlayer;
            inputManager.OnExit += StopMovement;
        }
        else
        {
            StopMovement();
        }
       
        
    }

    public void StopMovement()
    {
        CameraScript.instance.zoomOnPlayer = false;
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

        initialPlayerPos = gameObject.transform.position;
        playerToMovePos = grid.CellToWorld(gridPosition);

        // Adds player to the grid and removes its old position from the grids
        gameObject.GetComponent<Entity>().RemoveEntityFromGrids(gameObject);
        gameObject.GetComponent<Entity>().AddEntityToGrids(gridPosition);

        // Checks the Directly above and below positions isometrically and makes it so the player doesnt flip in those positions
        if ((playerToMovePos.x > initialPlayerPos.x && playerToMovePos.z > initialPlayerPos.z) || (playerToMovePos.x < initialPlayerPos.x && playerToMovePos.z < initialPlayerPos.z))
        {
            // No need to do anything
        }
        else
        {
            // Checks which way the player is moving and flips the sprite accordingly
            if (((playerToMovePos.x < initialPlayerPos.x) || (playerToMovePos.z > initialPlayerPos.z)))
            {
                playerSprite.flipX = false;
            }
            else
            {
                playerSprite.flipX = true;
            }
        }
        

        

        movePlayer = true;

    }

}
