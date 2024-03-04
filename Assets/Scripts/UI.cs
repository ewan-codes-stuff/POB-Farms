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
    [SerializeField] GameObject pauseMenu; //The pause menu in it's entirety simply sets to active or not active
    [SerializeField] public GameObject DeathScreen; //The pause menu in it's entirety simply sets to active or not active
                     bool       isGamePaused = false;

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

    private void Update()
    {
        if (Input.GetButtonDown("Pause")){ PauseGame(); }
    }

    public void SetHealth(int health)
    {
        healthText.text = health.ToString();
    }

    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }

    public void SetTurn(int remainingTurns, int maxTurns, bool night = false)
    {
        turnSlider.value = (float)(remainingTurns / maxTurns);
        if (night) turnSlider.value = 1f - turnSlider.value;
        turnText.text = remainingTurns.ToString();
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        Player.instance.FreezeInputs(isGamePaused);
        pauseMenu.GetComponent<PauseMenu>().GetRidOfSubMenuOnResume();
        pauseMenu.SetActive(isGamePaused);
    }
}
