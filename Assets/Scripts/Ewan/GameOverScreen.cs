using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Fade in
        //New Menu with Restart or Quit on it
    }

    public void OnDie()
    {


    }

    void Fade()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene("Reese's Scene");
    }
}
