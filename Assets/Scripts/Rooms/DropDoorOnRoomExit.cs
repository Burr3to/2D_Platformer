using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDoorOnRoomExit : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    [SerializeField] private GameObject DroppedDoorLeft, DroppedDoorRight;
    private RoomAllEnemies roomAllEnemies;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>(); //for later use
        roomAllEnemies = GameObject.FindGameObjectWithTag("DroppingDoor").GetComponent<RoomAllEnemies>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (roomAllEnemies.AllDead)
        {
            roomAllEnemies.StartTimer = false;
            roomAllEnemies.Congratulations.SetActive(false);
        }
       

        /*if (collision.tag == "Player" && roomAllEnemies.enemies.Count == 0) 
        {
            DroppedDoorLeft.SetActive(false);
            DroppedDoorRight.SetActive(false);
        }   */

    }
}
