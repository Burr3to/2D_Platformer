using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_skill : MonoBehaviour
{
    [SerializeField] private float damageNormalEnemies;
    [SerializeField] private float damageBoss;
    public float DamageBoss { get { return damageBoss; } set { DamageBoss = value; } }

    CircleCollider2D circleCollider;

    [Header("SFX")]
    [SerializeField] private AudioClip explosion;

    private void Start()
    {
        circleCollider= GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>()?.TakeDamage(damageNormalEnemies);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<Boss>().TakeDamage(damageBoss);
        }
    }
    public void PlayExplosionAudio()
    {
        SoundManager.instance.PlaySound(explosion);
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void DisableCOllider()
    {
        circleCollider.enabled = false;
    }

    public void EnableCollider()
    {
        circleCollider.enabled = true;
    }
}
