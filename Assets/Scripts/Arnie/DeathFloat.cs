using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloat : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeSpeed = 0.5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
            enabled = false;
        }
    }

    void Update()
    {
        gameObject.transform.LookAt(CameraScript.instance.transform);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 225, gameObject.transform.eulerAngles.z);

        // Move the sprite up
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        // Fade out the sprite
        Color currentColor = spriteRenderer.color;
        float newAlpha = Mathf.Lerp(currentColor.a, 0f, fadeSpeed * Time.deltaTime);
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

        // Destroy the GameObject when it's fully faded out
        if (spriteRenderer.color.a <= 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
