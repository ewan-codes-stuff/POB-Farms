using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpriteCorrector : MonoBehaviour
{
    public Camera cam;
    public Color nightColour;

    private Entity[] entities;
    private bool takingDamage;
    private float damageTimer;
    private Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        cam = CameraScript.instance.gameObject.GetComponent<Camera>();
        entities = gameObject.GetComponentsInParent<Entity>();
        scale = transform.localScale;
        if(entities != null ) 
        {
            foreach( Entity ent in entities )
            {
                //Subscribing to damage
                ent.TakingDamage += TakeDamage;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(cam.transform);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 225, gameObject.transform.eulerAngles.z);
        if (!takingDamage)
        {
            if (!TurnManager.instance.GetIsNight())
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = nightColour;
            }
            gameObject.transform.localScale = scale;
        }


        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
        else
        {
            takingDamage = false;
        }
    }

    private void TakeDamage()
    {
        gameObject.transform.localScale = 1.2f * scale;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        takingDamage = true;
        damageTimer = 0.2f;
    }
}