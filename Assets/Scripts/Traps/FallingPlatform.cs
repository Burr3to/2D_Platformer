using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay;
    [SerializeField] private float destroyDelay;
    [SerializeField] private Rigidbody2D rigidBody2d;

    bool FallActive = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (!FallActive && collision.transform.position.y > transform.position.y + 1.45f)
            {
                //Debug.Log("falling");
                FallActive = true;
                StartCoroutine(Fall());
            }
        }
    }

    private IEnumerator Fall()
    {
        
        yield return new WaitForSeconds(fallDelay);
        rigidBody2d.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);

    }
}
