using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider primaryHealthBar; //The slider for enemies health
    public Slider secondaryHealthBar;// a secondary slider that follows the first with a delay, purely visual

    public GameObject uiElements; //All ui elements

    public Entity entity;

    private float delayTimer = 0;
    private static float decayDelay = 0.75f; //delay before the secondary healthbar catches up to the primary
    private static float decaySpeed = .5f; //speed at which the secondary bar moves


    // Start is called before the first frame update
    void Start()
    {
        primaryHealthBar.value = 1;
        secondaryHealthBar.value = 1;

        uiElements.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        primaryHealthBar.value = (float)entity.GetHealth() / (float)entity.GetMaxHealth();

        delayTimer += Time.deltaTime;
        if (delayTimer < decayDelay)
        {
            return;
        }
        UpdateHealthBar(primaryHealthBar, secondaryHealthBar); //update healthbars
    }

    private void UpdateHealthBar(Slider _primaryBar, Slider _secondaryBar) //update bar slider values
    {
        if (_secondaryBar.value <= _primaryBar.value)
        {
            _secondaryBar.value = _primaryBar.value;
            uiElements.SetActive(false);
            return;
        }
        _secondaryBar.value -= Time.deltaTime * decaySpeed;
    }

    public void ApplyDamage() //Allows attached agent to refresh this script and show damage
    {
        uiElements.SetActive(true);
        delayTimer = 0;
    }
}
