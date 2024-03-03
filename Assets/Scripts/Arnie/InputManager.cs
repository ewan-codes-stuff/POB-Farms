using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;

    public event Action OnClicked, OnExit;

    public InputState inputSwitch;
    public enum InputState
    {
        PLACE,
        MOVE,
        HARVEST
    }

    public float delay = 0;

    private void Update()
    {
        delay += Time.deltaTime;
        if (!Player.instance.IsPlayerFrozen())
        {
            if(delay > 0.25f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnClicked?.Invoke();
                    delay = 0;
                }
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    OnExit?.Invoke();
                    delay = 0;
                }
            }
            
        }
        
            
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }



    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        else
        {
            ResetSelectedMapPosition();
        }

        return lastPosition;
    }

    public void ResetSelectedMapPosition()
    {
        lastPosition = Player.instance.transform.position;
    }
}
