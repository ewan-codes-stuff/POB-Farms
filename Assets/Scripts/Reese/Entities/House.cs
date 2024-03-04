using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class House : Entity
{
    public override void Die()
    {
        base.Die();
        Player.instance.Die();
    }
}