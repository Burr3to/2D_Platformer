using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject[] objects;
    [SerializeField] private float ActivateDelay;
    [SerializeField] private float DeactivateDelay;
    void Start()
    {
        StartCoroutine(ActivateDeactivate());
    }

    IEnumerator ActivateDeactivate()
    {
        int currentIndex = 0;
        while (true)
        {
            // Activate current object
            objects[currentIndex].SetActive(true);

            // Wait x seconds
            yield return new WaitForSeconds(ActivateDelay);

            // Deactivate current object
            objects[currentIndex].SetActive(false);

            // Wait x seconds
            yield return new WaitForSeconds(DeactivateDelay);

            // Move to the next object in the array
            currentIndex = (currentIndex + 1) % objects.Length;
        }
    }
}
