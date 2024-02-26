using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private int HP = 3;
    [SerializeField] private int cost = 3;
    [SerializeField] private bool Ally = true;
    #endregion

    #region Private variables
    //Stores my position on the grid
    private Vector2Int gridPosition;
    //Store my current HP
    private int currentHP;
    #endregion



    #region Initalisation
    private void Start()
    {
        Init();
    }
    public virtual void Init()
    {
        currentHP = HP;
    }
    #endregion

    #region Combat Functions
    public virtual void Heal(int healAmmount)
    {
        currentHP += healAmmount;

        if (currentHP > HP) { currentHP = HP; }
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
    public virtual void Die()
    {
        RemoveEntityFromGrids();
        GameObject.Destroy(gameObject);
    }
    #endregion

    #region Grid Management
    public virtual void AddEntityToGrids()
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Add(gameObject);
        GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(transform.position), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);

        //Add to GridTile entity variable
        GameManager.instance.tileArray[GetGridPosition()].entity = this;
    }
    public virtual void AddEntityToGrids(Vector3Int worldPos)
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Add(gameObject);
        GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(worldPos), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);

        //Add to GridTile entity variable
        GameManager.instance.tileArray[GetGridPosition()].entity = this;
    }

    public virtual void RemoveEntityFromGrids()
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Remove(gameObject);
        GameManager.instance.GetObjectData().RemoveObjectAt(GameManager.instance.GetGrid().WorldToCell(transform.position), new Vector2Int(1, 1));

        //Add to GridTile entity variable
        GameManager.instance.tileArray[GetGridPosition()].entity = null;
    }
    public virtual void RemoveEntityFromGrids(Vector3Int worldPos)
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Remove(gameObject);
        GameManager.instance.GetObjectData().RemoveObjectAt(worldPos, new Vector2Int(1, 1));

        //Add to GridTile entity variable
        GameManager.instance.tileArray[GetGridPosition()].entity = null;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    public void SetGridPosition(Vector2Int position)
    {
        gridPosition = position;
    }
    #endregion

    #region Getters / Setters
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
    #endregion
}