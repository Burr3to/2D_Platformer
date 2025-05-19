using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameroOrtho : MonoBehaviour
{
    [SerializeField] private float customOrthographicSize = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Camera.main.orthographicSize = customOrthographicSize;
        }
    }
}
