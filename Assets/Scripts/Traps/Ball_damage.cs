using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_damage : MonoBehaviour
{
    [SerializeField] protected float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>()?.TakeDamage(damage);
        }

    }
}
/*
 * In C#, the ?. operator is called the null-conditional operator. It is used to check if the object 
 * or element is null before performing an action on it. */