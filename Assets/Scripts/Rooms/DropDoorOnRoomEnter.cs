using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDoorOnRoomEnter : MonoBehaviour
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
        if (collision.tag == "Player")
        {
            if (roomAllEnemies.AllDead)
            {
                roomAllEnemies.StartTimer = false;
                roomAllEnemies.Congratulations.SetActive(true);
            }
            else
            {
                roomAllEnemies.StartTimer = true;
                roomAllEnemies.Congratulations.SetActive(false);
            }
            roomAllEnemies.ShowText = true;
            DroppedDoorLeft.SetActive(true);
            DroppedDoorRight.SetActive(true);

        }
        
    }
    
}
