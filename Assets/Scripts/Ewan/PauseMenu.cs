using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    [SerializeField] private Slider VolumeSlider;

    private void Start()
    {
        VolumeSlider.onValueChanged.AddListener(delegate { OnChangeVolume(); });
    }
    public void Resume()
    {
        optionsMenu.SetActive(false);
        UI.instance.PauseGame();
    }

    public void OptionsButton()
    {
        optionsMenu.SetActive(true);
    }

    public void QuitGame()
    {

    }

    public void OnChangeVolume()
    {
        TurnManager.instance.SetVolume(VolumeSlider.value);
    }
}
