using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private float WinScore; //skore na win && load next level
    [SerializeField] private string NextLevelname;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.MyInstance.Finish(WinScore,NextLevelname);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.MyInstance.HideVictoryCondition();
        }

    }

}
