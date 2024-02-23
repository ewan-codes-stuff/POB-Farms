using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KillScript : MonoBehaviour
{
    public float lifetime = 0.5f;
    private void Start()
    {
        Invoke("DestroySelf", lifetime);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
