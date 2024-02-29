using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static void CreateGridArray()
    {
        //Get the size of the ground and how many grid squares there are based off of it
        //Does not account for (ceb) cell(s) size yet
        int x = (int)(GameManager.instance.ground.transform.localScale.x * 10)+1;
        int z = (int)(GameManager.instance.ground.transform.localScale.z * 10)+1;

        GameManager.instance.tileArray = new Dictionary<Vector2Int, GridTile>();
        for (int i = -7; i <= x-7; i++) //the grid is 13x13 so that the enemies can spawn on inaccessible terrain
        {
            for (int j = (-7); j <= z-7; j++)
            {
                var newTile = new GridTile();
                newTile.name = $"Tile {i}{j}";
                newTile.position = new Vector2Int(i, j);
                newTile.gridPosition = new Vector2Int(i, j);
                newTile.traversable = true;
                GameManager.instance.tileArray[new Vector2Int(i, j)] = newTile;
                
            }
        }
        for(int n = -7; n <=6; n++) //Makes all the edge tiles inaccessible so they cannot be targeted by attacks or moved into
        {
            GameManager.instance.tileArray[new Vector2Int(n, 6)].traversable = false;
            GameManager.instance.tileArray[new Vector2Int(n, -7)].traversable = false;
            GameManager.instance.tileArray[new Vector2Int(6, n)].traversable = false;
            GameManager.instance.tileArray[new Vector2Int(-7, n)].traversable = false;
        }
        
    }

    public static void UpdateGridForHouse()
    {
        foreach(GameObject placedObject in PlacementSystem.instance.placedGameObject)
        {
            if (placedObject.GetComponent<Entity>() != null)
            {
                Vector2Int placedObjectVector2 = new Vector2Int(Mathf.RoundToInt(placedObject.transform.position.x), Mathf.RoundToInt(placedObject.transform.position.z));

                Grid localGrid = GameManager.instance.GetGrid();
            }
        }
        Debug.Log(PlacementSystem.instance.placedGameObject[0].gameObject.name);
    }
}