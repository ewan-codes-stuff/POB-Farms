using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarComponent : MonoBehaviour
{
    #region Meow
    //
    /*
            /\_____/\
           /  o   o  \
          ( ==  ^  == )
           )         (
          (           )
         ( (  )   (  ) )
        (__(__)___(__)__)
        meow
    */
    #endregion

    #region Public Variables

    public int maxHealth;
    public Vector3 healthbarOffset = new Vector3(0, 0.35f, 0);
    public Transform healthbarParent;

    #endregion
    #region Private Variables

    HealthbarController healthbar;

    #endregion
    
    #region Public Methods

    public void RefreshHealthbar(int _currHealth)
    {
        healthbar.RefreshHealthbar(_currHealth);
    }

    #endregion
    #region Private Variables

    private void Start()
    {
        GenerateUI();
        healthbar.ToggleDisplay(false);
    }
    void GenerateUI()
    {
        if (healthbarParent == null) { healthbarParent = transform; }
        healthbar = Instantiate(Resources.Load("UI/Healthbar") as GameObject, healthbarParent).GetComponent<HealthbarController>();
        healthbar.transform.localPosition = healthbarOffset;
        healthbar.Initialise(maxHealth);
    }

    #endregion

}
