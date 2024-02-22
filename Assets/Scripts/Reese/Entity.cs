using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] private int HP = 3;
    [SerializeField] private int Cost = 3;

    [SerializeField]private bool Ally = true;

    //Stores my position on the grid
    private Vector2Int gridPosition;

    //Store my current HP
    private int currentHP;

    private void Start()
    {
        currentHP = HP;
    }
    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        Debug.Log(this.gameObject.name + " Taken damage");

        if(currentHP < 0) 
        {
            currentHP = 0;
            Debug.Log(this.gameObject.name + " Died");
            Die();
            
        }
    }

    void Die()
    {
        GameObject.DestroyImmediate(this.gameObject);
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