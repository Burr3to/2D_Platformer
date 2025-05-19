using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed;

    private Vector3 currentTarget;

    void Start()
    {
        currentTarget = endPoint.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        if (transform.position == endPoint.position)
        {
            currentTarget = startPoint.position;
        }
        else if (transform.position == startPoint.position)
        {
            currentTarget = endPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)  //movement hraca s plosinou 
    {
        if (collision.transform.position.y > transform.position.y && (collision.gameObject.CompareTag("Player")))
        {
            collision.transform.SetParent(transform);
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
