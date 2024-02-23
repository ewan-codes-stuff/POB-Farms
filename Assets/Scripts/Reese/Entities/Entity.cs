using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] private int HP = 3;
    [SerializeField] private int cost = 3;

    [SerializeField] private bool Ally = true;

    //Stores my position on the grid
    private Vector2Int gridPosition;

    //Store my current HP
    private int currentHP;

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        currentHP = HP;
    }

    public virtual void TakeDamage(int damageAmount)
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

    public virtual void Die()
    {
        if (gameObject.GetComponent<AI>() != null)
        {
            GameManager.instance.aiManager.RemoveAIFromList(gameObject);
        }
        else
        {
            //Remove this gameobject from the placed objects list
            GameManager.instance.GetPlacedObjects().Remove(gameObject);

            //Remove the existing plant from the gridData
            PlacementSystem.instance.objectData.RemoveObjectAt(PlacementSystem.instance.grid.WorldToCell(transform.position), new Vector2Int(1, 1));
        }
        

        //Remove from this stupid other thing
        GameManager.instance.tileArray[GetGridPosition()].entity = null;
        GameObject.Destroy(gameObject);
    }

    public virtual void Heal(int healAmmount) 
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

    public int GetCost()
    {
        return cost;
    }
}