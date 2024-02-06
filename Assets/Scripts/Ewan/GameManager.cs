using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject ground;

    public GridTile[,] tileArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGridArray()
    {
        int x = (int)ground.transform.localScale.x * 10;
        int z = (int)ground.transform.localScale.z * 10;

        tileArray = new GridTile[x, z];
        for (int i = 0 ; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                tileArray[i, j].position = new Vector2Int(i - (x / 2), j - (z / 2));
                tileArray[i, j].traversable = true;
            }
        }



    }
}
