using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ground;

    public GameObject house;

    public Dictionary<Vector2Int, GridTile> tileArray;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        instance = this;
    }

    private void Start()
    {
        GridManager.CreateGridArray();
    }

    public GridData GetObjectData()
    {
        return PlacementSystem.instance.objectData;
    }

    public Grid GetGrid()
    {
        return PlacementSystem.instance.grid;
    }

    public List<GameObject> GetPlacedObjects()
    {
        return PlacementSystem.instance.placedGameObject;
    }
}