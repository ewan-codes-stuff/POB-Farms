using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    [SerializeField] private Slider VolumeSlider;
    bool optionsOpen = false;

    private void Start()
    {
        VolumeSlider.onValueChanged.AddListener(delegate { OnChangeVolume(); });
        VolumeSlider.value = GameManager.instance.GetVolume();
    }
    public void Resume()
    {
        optionsMenu.SetActive(false);
        UI.instance.PauseGame();
    }

    public void GetRidOfSubMenuOnResume()
    {
        optionsMenu.SetActive(false);
    }

    public void OptionsButton()
    {
        optionsMenu.SetActive(!optionsOpen);
        optionsOpen = !optionsOpen;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnChangeVolume()
    {
        GameManager.instance.SetVolume(VolumeSlider.value);
        PlayerPrefs.SetFloat("masterVolume", VolumeSlider.value);
    }
}