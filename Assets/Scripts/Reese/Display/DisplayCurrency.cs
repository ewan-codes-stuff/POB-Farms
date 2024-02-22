using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCurrency : MonoBehaviour
{
    Player player;

    public void Start()
    {
        player = Player.instance;
    }

    public void FixedUpdate()
    {
        if (this.gameObject.GetComponent<Text>() != null)
        {
            //Get the text component of the connected object and update it to equal the current turn
            this.gameObject.GetComponent<Text>().text = player.GetCurrency().ToString();
        }
        else if (this.gameObject.GetComponent<TextMeshProUGUI>() != null)
        {
            //Get the textMeshPro component of the connected object and update it to equal the current turn
            this.gameObject.GetComponent<TextMeshProUGUI>().text = player.GetCurrency().ToString();
        }
    }
}
