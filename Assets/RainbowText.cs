using System.Collections;
using UnityEngine;
using TMPro;

public class RainbowText : MonoBehaviour
{
    // The speed at which the colors change
    public float colorSpeed = 1f;

    // The speed at which the text rotates
    public float rotationSpeed = 10f;

    // Reference to the TextMeshProUGUI component
    private TextMeshProUGUI text;

    private void Start()
    {
        // Get the reference to the TextMeshProUGUI component
        text = GetComponent<TextMeshProUGUI>();

        // Start the color change coroutine
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        // A variable to keep track of the current color position
        float colorPos = 0;

        // A infinite loop to continuously change the color and rotation
        while (true)
        {
            // Increase the color position over time
            colorPos += Time.deltaTime * colorSpeed;

            // Use the color position to determine the current color using HSVToRGB
            text.color = Color.HSVToRGB(colorPos % 1, 1, 1);

            // Rotate the text around its center
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * rotationSpeed);

            // Wait until the next frame before continuing
            yield return null;
        }
    }
}
