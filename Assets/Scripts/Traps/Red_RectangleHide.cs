using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_RectangleHide : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToHide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(false);
                StartCoroutine(ActivateAfterDelay(obj, 5f));
            }
        }
        
    }

    IEnumerator ActivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }
}
