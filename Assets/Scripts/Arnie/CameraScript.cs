using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 InitialPos;
    public Transform PlayerPos;

    public float initialSize = 0;


    public bool zoomOnPlayer = false;


    float timeElapsed;
    public float lerpDuration = 3;

    public bool zoomOnPlayerMore = false;

    // Singleton instance
    public static CameraScript instance;

    public float orthosize = 5;

    Vector3 lerpPos = new Vector3();

    public Vector3 LerpFrom;
    public Vector3 LerpTo;

    public void Awake()
    {
        // Initialise Singleton
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitialPos = gameObject.transform.position;
        initialSize = gameObject.GetComponent<Camera>().orthographicSize;

        lerpPos = new Vector3(PlayerPos.position.x - 7, gameObject.transform.position.y, PlayerPos.transform.position.z - 7);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(initialSize, orthosize, timeElapsed / lerpDuration);
            transform.position = Vector3.Lerp(LerpFrom, LerpTo, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            if (zoomOnPlayer)
            {
                transform.position = new Vector3(PlayerPos.position.x - 7, gameObject.transform.position.y, PlayerPos.transform.position.z - 7);
            }
            else
            {
                transform.position = LerpTo;
            }
            
        }
    }

    public void StartLerp(float newOrthosize, bool lerpIn)
    {
        if (lerpIn)
        {
            zoomOnPlayer = true;
            timeElapsed = 0;
            LerpFrom = gameObject.transform.position;
            LerpTo = new Vector3(PlayerPos.position.x - 7, gameObject.transform.position.y, PlayerPos.transform.position.z - 7);
            orthosize = newOrthosize;
            initialSize = gameObject.GetComponent<Camera>().orthographicSize;
        }
        else
        {
            zoomOnPlayer = false;
            timeElapsed = 0;
            LerpFrom = gameObject.transform.position;
            LerpTo = InitialPos;
            orthosize = newOrthosize;
            initialSize = gameObject.GetComponent<Camera>().orthographicSize;
        }
    }
}
