using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private int soyl = 10;

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
        return soyl;
    }

    public void DecreaseCurrency(int ammount)
    {
        soyl -= ammount;
        if(soyl < 0) soyl = 0;
    }

    public void AddCurrency(int ammount)
    {
        soyl += ammount;
    }
}