using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;

    private int wollars = 10;

    private void Awake()
    {
        //Setup the Player instance
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public int GetCurrency()
    {   
        //Getter for currency on player
        return wollars;
    }

    public void DecreaseCurrency(int ammount)
    {
        //Remove Currency from Player
        wollars -= ammount;

        //Ensure currency remains at 0 minimum
        if(wollars < 0) wollars = 0;
    }

    public void AddCurrency(int ammount)
    {
        //Add Currency to Player
        wollars += ammount;
    }
}