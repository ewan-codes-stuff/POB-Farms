using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private string GameScene = "Reese's Scene";
    [SerializeField]
    private AudioSource audio;


    public void PlayGame()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        audio.volume = PlayerPrefs.GetFloat("masterVolume");
    }
}
