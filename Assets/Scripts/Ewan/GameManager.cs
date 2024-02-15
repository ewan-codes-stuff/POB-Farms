using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ground;
    public GameObject test;

    public GridTile[,] tileArray;
    // Start is called before the first frame update
    void Start()
    {
        CreateGridArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGridArray()
    {
        //Get the size of the ground and how many grid squares there are based off of it
        //Does not account for (ceb) cell(s) size yet
        int x = (int)(ground.transform.localScale.x * 10);
        int z = (int)(ground.transform.localScale.z * 10);

        tileArray = new GridTile[x, z];
        for (int i = 0 ; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                tileArray[i, j].position = new Vector2Int(i - (x / 2), j - (z / 2));
                tileArray[i, j].traversable = true;
            }
        }
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                Instantiate(test, new Vector3(tileArray[i,j].position.x, ground.transform.position.y + 1, tileArray[i,j].position.y),Quaternion.identity);
            } 
        }

    }
}
