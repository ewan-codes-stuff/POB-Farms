using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayTurn : MonoBehaviour
{
    TurnManager turnManager;

    public void Start()
    {
        turnManager = TurnManager.instance;
        turnManager.EndTurnEvent += UpdateTurnDisplay;
        UpdateTurnDisplay();
    }

    private void UpdateTurnDisplay()
    {
        if (this.gameObject.GetComponent<Text>() != null)
        {
            //Get the text component of the connected object and update it to equal the current turn
            this.gameObject.GetComponent<Text>().text = (turnManager.GetCurrentTurn()).ToString();
        }
        else if (this.gameObject.GetComponent<TextMeshProUGUI>() != null)
        {
            //Get the textMeshPro component of the connected object and update it to equal the current turn
            this.gameObject.GetComponent<TextMeshProUGUI>().text = (turnManager.GetCurrentTurn()).ToString();
        }
    }
}