using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDesc : MonoBehaviour
{
    public GameObject Desc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            Desc.SetActive(true);
            Debug.Log("ASSSS");
        }
        else
        {
            Desc.SetActive(false);
            Debug.Log("Its NOT over UI elements");
        }*/

    }

    public void OnMouseEnter()
    {
        Desc.SetActive(true);
        Debug.Log("ASSSS");
    }

    public void OnMouseExit()
    {
        Desc.SetActive(false);
        Debug.Log("Its NOT over UI elements");
    }
}
