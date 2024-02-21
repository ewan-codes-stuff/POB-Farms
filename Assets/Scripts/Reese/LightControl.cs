using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    private float startIntensity, currentIntensity, targetIntensity;

    private float timeToLerp = 1.0f;

    private float timer;


    private void Start()
    {
        startIntensity = 0;
        currentIntensity = 1;
        targetIntensity = 1;
        timer = timeToLerp;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < timeToLerp)
        {
            currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, timer / timeToLerp);
            this.GetComponent<Light>().intensity = currentIntensity;
            timer += Time.deltaTime;
        }
    }

    public void FlipIntensity()
    {
        targetIntensity = startIntensity;
        startIntensity = currentIntensity;
        timer = 0;
    }
}
