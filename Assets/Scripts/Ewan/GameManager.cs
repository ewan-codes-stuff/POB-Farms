using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ground;

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
        CreateGridArray();
    }
          
    void CreateGridArray()
    {
        //Get the size of the ground and how many grid squares there are based off of it
        //Does not account for (ceb) cell(s) size yet
        int x = (int)(ground.transform.localScale.x * 10);
        int z = (int)(ground.transform.localScale.z * 10);

        tileArray = new Dictionary<Vector2Int, GridTile>();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                var newTile = new GridTile();
                newTile.name = $"Tile {i}{j}";
                newTile.position = new Vector2Int(i - (x / 2), j - (z / 2));
                newTile.gridPosition = new Vector2Int(i, j);
                newTile.traversable = true;
                tileArray[new Vector2Int(i, j)] = newTile;
            }
        }
    }
}