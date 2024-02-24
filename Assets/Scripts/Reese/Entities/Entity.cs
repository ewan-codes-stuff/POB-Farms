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

        if(currentHP <= 0) 
        {
            currentHP = 0;
            Die();
            
        }
    }
    public virtual void AddEntityToGrids()
    {
        if (!PlacementSystem.instance.placedGameObject.Contains(gameObject) && GameManager.instance.tileArray[GetGridPosition()].entity == null)
        {
            //Add to Unity Grid and placed objects list
            GameManager.instance.GetPlacedObjects().Add(gameObject);
            GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z))), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);

            //Add to GridTile entity variable
            GameManager.instance.tileArray[GetGridPosition()].entity = this;
        }
    }

    public virtual void RemoveEntityFromGrids(Vector3Int gridPosition)
    {
        if (PlacementSystem.instance.placedGameObject.Contains(gameObject))
        {
            //Add to Unity Grid and placed objects list
            GameManager.instance.GetPlacedObjects().Remove(gameObject);
            GameManager.instance.GetObjectData().RemoveObjectAt(gridPosition, new Vector2Int(1, 1));

            //Add to GridTile entity variable
            GameManager.instance.tileArray[GetGridPosition()].entity = null;
        }
    }
    public virtual void Die()
    {
        
        //Remove this gameobject from the placed objects list
        GameManager.instance.GetPlacedObjects().Remove(gameObject);

        //Remove the existing plant from the gridData
        PlacementSystem.instance.objectData.RemoveObjectAt(PlacementSystem.instance.grid.WorldToCell(transform.position), new Vector2Int(1, 1));
        
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