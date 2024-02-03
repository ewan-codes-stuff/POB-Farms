using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look : MonoBehaviour
{
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(cam.transform);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 225, gameObject.transform.eulerAngles.z);
    }
}
