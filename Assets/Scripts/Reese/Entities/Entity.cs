using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private int HP = 3;
    [SerializeField] private int damage = 1;
    [SerializeField] private int cost = 3;
    [SerializeField] private bool Ally = true;
    #endregion

    #region Private variables
    //Stores my position on the grid
    private Vector2Int gridPosition;
    //Store my current HP
    private int currentHP;
    //Timer for taking damage
    private float damageTimer = 0.2f;
    #endregion

    #region Public Actions
    public event Action TakingDamage;
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
    public virtual IEnumerator TakeDamage(int damageAmount)
    {
        TakingDamage?.Invoke();

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            yield return null;
        }
        currentHP -= damageAmount;

        if(currentHP <= 0) 
        {
            currentHP = 0;
            Die();
        }

        damageTimer = 0.2f;
    }

    public virtual void Die()
    {
        RemoveEntityFromGrids();
        GameObject.Destroy(gameObject);
    }
    #endregion

    #region Grid Management
    //Adding to the grid
    public virtual void AddEntityToGrids()
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Add(gameObject);
        GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(transform.position), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);

        //Add to GridTile entity variable
        GameManager.instance.tileArray[new Vector2Int((int)transform.position.x, (int)transform.position.z)].entity = this;
    }
    public virtual void AddEntityToGrids(GameObject obj)
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Add(obj);
        GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(obj.transform.position), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);

        if (GameManager.instance.tileArray == null) return;

        //Add to GridTile entity variable
        if (obj.GetComponent<Entity>() != null) GameManager.instance.tileArray[new Vector2Int((int)obj.transform.position.x, (int)obj.transform.position.z)].entity = obj.GetComponent<Entity>();
    }
    public virtual void AddEntityToGrids(Vector3Int worldPos)
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Add(gameObject);
        GameManager.instance.GetObjectData().AddObjectAt(GameManager.instance.GetGrid().WorldToCell(worldPos), new Vector2Int(1, 1), 100, GameManager.instance.GetPlacedObjects().Count - 1);

        //Add to GridTile entity variable
        GameManager.instance.tileArray[new Vector2Int(worldPos.x, worldPos.z)].entity = this;
    }

    //Removing from the grid
    public virtual void RemoveEntityFromGrids()
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Remove(gameObject);
        GameManager.instance.GetObjectData().RemoveObjectAt(GameManager.instance.GetGrid().WorldToCell(transform.position), new Vector2Int(1, 1));

        //Add to GridTile entity variable
        GameManager.instance.tileArray[new Vector2Int((int)transform.position.x, (int)transform.position.z)].entity = null;
    }
    public virtual void RemoveEntityFromGrids(GameObject obj)
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Remove(obj);
        GameManager.instance.GetObjectData().RemoveObjectAt(GameManager.instance.GetGrid().WorldToCell(obj.transform.position), new Vector2Int(1, 1));

        //Add to GridTile entity variable
        if(obj.GetComponent<Entity>() != null) GameManager.instance.tileArray[new Vector2Int((int)obj.transform.position.x, (int)obj.transform.position.z)].entity = null;
    }
    public virtual void RemoveEntityFromGrids(Vector3Int worldPos)
    {
        //Add to Unity Grid and placed objects list
        GameManager.instance.GetPlacedObjects().Remove(gameObject);
        GameManager.instance.GetObjectData().RemoveObjectAt(worldPos, new Vector2Int(1, 1));

        //Add to GridTile entity variable
        GameManager.instance.tileArray[new Vector2Int(worldPos.x, worldPos.z)].entity = null;
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

    public int GetDamage()
    {
        return damage;
    }

    public int GetHealth()
    {
        return currentHP;
    }
    #endregion
}