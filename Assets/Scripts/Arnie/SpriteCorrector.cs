using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class SpriteCorrector : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Color nightColour;
    [SerializeField]
    private float timeToLerp = 1.0f;

    private bool isNight;

    private Entity[] entities;
    private Color currentColour;
    private bool takingDamage;
    private float lerpTimer;
    private float damageTimer;
    private Vector3 scale;

    private Vector3 startColour;
    private Vector3 targetColour;
    private Vector3 newColour;

    // Start is called before the first frame update
    void Start()
    {
        cam = CameraScript.instance.gameObject.GetComponent<Camera>();
        entities = gameObject.GetComponentsInParent<Entity>();
        if(entities != null ) 
        {
            foreach( Entity ent in entities )
            {
                //Subscribing to damage
                ent.TakingDamage += TakeDamage;
            }
        }
        scale = transform.localScale;
        lerpTimer = timeToLerp + 1;
        isNight = TurnManager.instance.GetIsNight();
        if( isNight ) currentColour = nightColour;
        else currentColour = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(cam.transform);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 225, gameObject.transform.eulerAngles.z);

        //If the sprite is not taking damage
        if (!takingDamage)
        {
            //Reset the sprite's size
            gameObject.transform.localScale = scale;
            gameObject.GetComponent<SpriteRenderer>().color = currentColour;

            //If it is not night
            if (!TurnManager.instance.GetIsNight() && isNight)
            {
                Debug.Log("Wakey, wakey Asshole");
                //Set is Night bool
                isNight = false;
                //Setup the target colours for lerping to day
                startColour = new Vector3(currentColour.r, currentColour.g, currentColour.b);
                targetColour = new Vector3(Color.white.r, Color.white.g, Color.white.b);
                //Reset Lerp Timer
                lerpTimer = 0;
            }
            //If it is night
            if(TurnManager.instance.GetIsNight() && !isNight)
            {
                //Set is Night bool
                isNight = true;
                //Setup the target colours for lerping to night
                startColour = new Vector3(currentColour.r, currentColour.g, currentColour.b);
                targetColour = new Vector3(nightColour.r, nightColour.g, nightColour.b);

                //Reset Lerp Timer
                lerpTimer = 0;
            }

            if(lerpTimer <= timeToLerp)
            {
                lerpTimer += Time.deltaTime;
                newColour = Vector3.Lerp(startColour, targetColour, lerpTimer / timeToLerp);
                currentColour = new Color(newColour.x, newColour.y, newColour.z, 1);
            }
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
        //Increase sprite scale when taking damage
        gameObject.transform.localScale = 1.2f * scale;
        //Change render colour to red to show taking damage
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        //Set the taking Damage bool to true
        takingDamage = true;
        //Increase the damage
        damageTimer = 0.2f;
    }
}