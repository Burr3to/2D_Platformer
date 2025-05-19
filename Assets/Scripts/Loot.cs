using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public static Loot instance;
    [SerializeField] LootTable LootTable;
    Transform transform;
    Health health;

    private void Start()
    {
        if (instance == null)  // Check if singleton instance is null
        {
            instance = this; // If so, set it to this script's instance
        }

    }
    private void Update()
    {
        transform = GetComponent<Transform>();
        health = transform.GetComponent<Health>();

        if (health.ReadytoSpawnLoot == true)
        {
            Instantiate(LootTable.GetDrop(), transform.position, Quaternion.identity); // Spawn a randomly selected item from the LootTable
            //quaternion represents no rotation value
            health.ReadytoSpawnLoot = false; //after spawn loot disable
        }
    }
}
