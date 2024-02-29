using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDesc : MonoBehaviour
{
    public GameObject Desc;

    public void OnMouseEnter()
    {
        Desc.SetActive(true);
    }

    public void OnMouseExit()
    {
        Desc.SetActive(false);
    }
}
