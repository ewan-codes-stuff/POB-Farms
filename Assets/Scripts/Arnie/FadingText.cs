using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadingText : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI textField;
    public Color standardCol;

    private Color currentCol;
    private Vector3 startSize, endSize;
    private float timer = 0;
    private void Awake()
    {
        SetText("hey there delialah");
    }

    public void SetText(string itemText)
    {
        currentCol = standardCol;
        startSize = transform.localScale * 1.5f;
        endSize = transform.localScale * 0.5f;
        //textField.color = currentCol;
        textField.text = itemText;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector3.up * Time.deltaTime * 1.5f); // Make canvas rise
        transform.localScale = Vector3.Lerp(startSize, endSize, timer / 2);
        textField.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime / 1.5f); // Fade text out
        if (textField.color.a <= 0.0f)
        {
            Destroy(gameObject); // Destroy self when text fully gone
        }
    }
}
