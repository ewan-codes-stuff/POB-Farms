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



    // Singleton instance
    public static CameraScript instance;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (zoomOnPlayer)
        {
            Vector3 TargetPos = new Vector3(PlayerPos.position.x - 7, gameObject.transform.position.y, PlayerPos.transform.position.z - 7);
            if (timeElapsed < lerpDuration)
            {
                gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(initialSize, 5, timeElapsed / lerpDuration);
                transform.position = Vector3.Lerp(InitialPos, TargetPos, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3(PlayerPos.position.x - 7, gameObject.transform.position.y, PlayerPos.transform.position.z - 7);
            }

        }
        else
        {
            timeElapsed = 0;
            gameObject.transform.position = new Vector3(InitialPos.x, gameObject.transform.position.y, InitialPos.z);
            gameObject.GetComponent<Camera>().orthographicSize = initialSize;
        }
    }
}
