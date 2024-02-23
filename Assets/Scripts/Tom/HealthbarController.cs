using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class HealthbarController : MonoBehaviour
{
    #region Variables
    GameObject[] hitPoints;
    int maxHealth;
    int currHealth;
    float damageHex = 800000;
    float diameter = 0.225f;
    bool isVisualised = false;
    #endregion

    #region Public Methods
    public void Initialise(int _maxHealth)
    {
        maxHealth = _maxHealth;
        currHealth = maxHealth;
        hitPoints = new GameObject[_maxHealth];
        // 1 - 0
        // 2 - -0.5x
        // 3 - -1x
        // 4 - -1.5x
        float xCorner = (_maxHealth - 1) * -(diameter * 0.5f);

        //auto scale
        Transform parent = transform.parent;
        transform.parent = null;
        transform.localScale = Vector3.one;
        transform.parent = parent;

        //Generate healthbar
        for (int i = 0; i < _maxHealth; i++)
        {
            hitPoints[i] = GameObject.Instantiate(Resources.Load("UI/HitpointIcon") as GameObject, transform);
            hitPoints[i].GetComponent<SpriteRenderer>().color = Color.green;
            hitPoints[i].transform.localPosition = Vector3.left * (xCorner + i * diameter);
        }
    }

    public void ToggleDisplay(bool _isDisplayed)
    {
        isVisualised = _isDisplayed;
        foreach (GameObject obj in hitPoints)
        {
            obj.SetActive(_isDisplayed);
        }
    }
    public void RefreshHealthbar(int _currHealth, bool _spawnDamageFX = false)
    {
        currHealth = _currHealth;
        for (int i = 0; i < hitPoints.Length; i++)
        {
            Color col = _currHealth > i ? Color.green : new Color(0.5f, 0, 0);
            hitPoints[i].GetComponent<SpriteRenderer>().color = col;
        }
        if (!_spawnDamageFX) { return; }
        SpawnDamageVFX();
    }

    #endregion
    #region Private Methods
    private void Update()
    {
        //Check if mouse over
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.parent.position);
        Vector3 mousePos = Input.mousePosition;
        screenPos.z = mousePos.z;
        ToggleDisplay(Vector3.Distance(screenPos, mousePos) < 25);
        //

        //Test damage
        if (Input.GetKeyDown(KeyCode.Z) && isVisualised)
        {
            RefreshHealthbar(currHealth - 1, true);
        }
    }

    void SpawnDamageVFX()
    {
        //Spawn vfx at parent pos
        GameObject fx = Instantiate(Resources.Load("FX/DamageFX") as GameObject);
        fx.transform.parent = transform.parent;
        fx.transform.localPosition = Vector3.zero;
        fx.transform.localRotation = Quaternion.identity;
    }
    #endregion

}
