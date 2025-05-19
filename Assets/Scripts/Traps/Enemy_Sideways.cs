using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    private bool movingLeft;


    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge.position.x)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge.position.x)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}