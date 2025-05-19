using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingBall : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    float angle;
    int dir;

    void Start()
    {
        angle = 0;  // od 0 po 1
        dir = minAngle % 360 < maxAngle % 360 ? 1 : -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float acc = 2 * (0.51f - Mathf.Abs(0.5f - angle)); // ked je 0 je min  || 1 som na max
        // acc urcuje rychlost realtime
        //ked min mam minimalnu rychlost
        //zrychluje pokial nedosiahne stred drahy
        //hore vlavo/vpravo bude min rychlost a zrychluje smerom dole

        angle += speed * acc * dir * Time.deltaTime;   //sme na min prechadzame na max  //otocime sa naopak

        if (angle > 1) //otocenie smeru na max
        {
            angle = 1;
            dir *= -1;
        }
        else if (angle < 0) 
        {
            angle = 0;
            dir *= -1;
        }

        float delta = maxAngle - minAngle;  //urcujem rozsah kyvania

        transform.rotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.y,
            minAngle + angle * delta    
        );

    }
}