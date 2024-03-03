using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayTurn : MonoBehaviour
{
    TurnManager turnManager;

    [SerializeField] bool displayRoundInstead = false;

    public void Start()
    {
        turnManager = TurnManager.instance;
        turnManager.EndTurnEvent += UpdateTurnRoundDisplay;
        UpdateTurnRoundDisplay();
    }

    private void UpdateTurnRoundDisplay()
    {
        if(!displayRoundInstead)
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
        else
        {
            if (this.gameObject.GetComponent<Text>() != null)
            {
                //Get the text component of the connected object and update it to equal the current turn
                this.gameObject.GetComponent<Text>().text = (turnManager.GetCurrentRound()).ToString();
            }
            else if (this.gameObject.GetComponent<TextMeshProUGUI>() != null)
            {
                //Get the textMeshPro component of the connected object and update it to equal the current turn
                this.gameObject.GetComponent<TextMeshProUGUI>().text = (turnManager.GetCurrentRound()).ToString();
            }
        }
    }
}