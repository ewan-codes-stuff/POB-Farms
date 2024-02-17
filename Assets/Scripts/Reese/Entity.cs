using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] private int HP = 3;

    private bool Ally = true;

    //Stores my position on the grid
    private Vector2Int gridPosition;

    //Store my current HP
    private int currentHP;

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

        if(currentHP < 0) { currentHP = 0; }
    }

    public void Heal(int healAmmount) 
    {
        currentHP += healAmmount;

        if(currentHP > HP) { currentHP = HP; }
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public void SetGridPosition( Vector2Int position)
    {
        gridPosition = position;
    }

    public void SetAlly(bool team)
    {
        Ally = team;
    }

    public bool IsAlly()
    {
        return Ally;
    }
}