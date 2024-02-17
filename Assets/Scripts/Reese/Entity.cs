using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int HP = 3;

    private Vector2Int gridPosition;
    private int currentHP;
}