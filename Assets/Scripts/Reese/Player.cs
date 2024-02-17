using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private int currency = 10;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void Pay(int ammount)
    {
        currency -= ammount;
        if(currency < 0) currency = 0;
    }

    public void AddCurrency(int ammount)
    {
        currency += ammount;
    }
}