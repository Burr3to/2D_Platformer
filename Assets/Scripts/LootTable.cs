using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Attribute to create a menu item for this scriptable object
public class LootTable : ScriptableObject
{

    [Serializable]
   public class Drop
    {
        public GameObject drop;
        public int probability;
    }

    public List<Drop> table;

    public GameObject GetDrop()
    {
        int roll = UnityEngine.Random.Range(0, 101);

        for (int i = 0; i < table.Count; i++) // Iterate through all the drops in the table
        {
            roll -= table[i].probability; // Subtract the probability of the current drop from the roll

            if (roll < 0) // If the roll is less than 0, the current drop should be returned
            {
                return table[i].drop; // Return the GameObject associated with the drop

            }
        }
      
        return table[0].drop;

    }
}
/*
       // if we roll 40 cycle wil calculate 40 minus probability of the item 0
          which will be -10 cuz our probability is 50 > will return item number 0


      */