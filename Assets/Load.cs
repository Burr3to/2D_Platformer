using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ScoreManager.instance.LoadData();
            ScoreManager.instance.ChangeScore(ScoreManager.instance.Skore);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ScoreManager.instance.LoadData();
            ScoreManager.instance.ChangeScore(ScoreManager.instance.Skore);
        }
    }
}
