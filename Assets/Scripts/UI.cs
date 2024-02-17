using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText; // text object for health
    [SerializeField] TextMeshProUGUI moneyText; // text object for money
    [SerializeField] Slider turnSlider; // bar to show turn progress
    [SerializeField] TextMeshProUGUI turnText; // text that shows current turn number

    public static UI instance;
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        instance = this;
    }

    public void SetHealth(int health)
    {
        healthText.text = health.ToString();
    }

    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }

    public void SetTurn(int remainingTurns, int maxTurns)
    {
        turnSlider.value = (float)(remainingTurns / maxTurns);
        turnText.text = remainingTurns.ToString();
    }
}
