using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCorrector : MonoBehaviour
{
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        cam = CameraScript.instance.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(cam.transform);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 225, gameObject.transform.eulerAngles.z);
        if (!TurnManager.instance.GetIsNight())
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            Debug.Log("Day");
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            Debug.Log("Night");
        }
    }
}