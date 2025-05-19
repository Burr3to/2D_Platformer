using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Scrolling_background : MonoBehaviour
{
    public float scrollSpeed = 10f;
    public float startY = 0f;
    public float endY = 100f;

    void Update()
    {
        // target position for the game object
        float targetY = transform.position.y + scrollSpeed * Time.deltaTime;

        // Reverse the direction of movement if the game object has reached start or end
        if (targetY > endY || targetY < startY)
        {
            scrollSpeed = -scrollSpeed;
        }

        // Use the Lerp method to smoothly interpolate between the current position and the target position
        float t = Time.deltaTime * Mathf.Abs(scrollSpeed);

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), t);

    }

}
