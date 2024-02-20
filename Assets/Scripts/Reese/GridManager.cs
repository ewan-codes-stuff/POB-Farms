using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static void CreateGridArray()
    {
        //Get the size of the ground and how many grid squares there are based off of it
        //Does not account for (ceb) cell(s) size yet
        int x = (int)(GameManager.instance.ground.transform.localScale.x * 10);
        int z = (int)(GameManager.instance.ground.transform.localScale.z * 10);

        GameManager.instance.tileArray = new Dictionary<Vector2Int, GridTile>();
        for (int i = (-x/2)+1; i < x; i++)
        {
            for (int j = (-z/2)+1; j < z; j++)
            {
                var newTile = new GridTile();
                newTile.name = $"Tile {i}{j}";
                newTile.position = new Vector2Int(i, j);
                newTile.gridPosition = new Vector2Int(i, j);
                newTile.traversable = true;
                GameManager.instance.tileArray[new Vector2Int(i, j)] = newTile;
            }
        }
    }

    void UpdateGridForPlacedObjects()
    {
        foreach(GameObject placedObject in PlacementSystem.instance.placedGameObject)
        {
            Vector2Int placedObjectVector2 = new Vector2Int(Mathf.RoundToInt(placedObject.transform.position.x), Mathf.RoundToInt(placedObject.transform.position.z));

            Grid localGrid = GameManager.instance.GetGrid();
            localGrid.WorldToCell(placedObject.transform.position);
        }
    }
}