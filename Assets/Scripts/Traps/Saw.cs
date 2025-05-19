using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed;

    void Update()
    {
        float lerp = Mathf.PingPong(Time.time * (speed / 10), 1); // /10 lebo to bolo vela
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerp);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
        }
        
    }
}
/*
The movement of the game object is achieved using the Mathf.PingPong and Vector3.Lerp functions. The Mathf.PingPong function returns a value that 
oscillates between 0 and 1 as time increases, and the Vector3.Lerp function interpolates between two vectors based on 
a given parameter (in this case, the value returned by Mathf.PingPong). The resulting value is then used to update the position of the game object.
 
 
 
 */