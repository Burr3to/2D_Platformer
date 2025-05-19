using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Value")]
    [SerializeField] public int coinValue;
    [SerializeField] private AudioClip coinPickUpSound;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            ScoreManager.instance.ChangeCash(coinValue);
            SoundManager.instance.PlaySound(coinPickUpSound);
            Destroy(gameObject);
        }
    }
}
