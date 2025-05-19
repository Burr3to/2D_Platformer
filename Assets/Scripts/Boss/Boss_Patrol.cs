using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Patrol : MonoBehaviour
{
    [Header("Player Transform")]
    [SerializeField] private Transform playerTransform;

    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Boss")]
    [SerializeField] private Transform BossTransform;
    [SerializeField] private Boss boss;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 enemyScale;
    public bool movingLeft { get; private set; }

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    [Header("Player Detection script")]
    [SerializeField] private PlayerDetection playerDetection;


    private void Awake()
    {
        enemyScale = BossTransform.localScale;
    }
    private void Start()
    {
        playerDetection = GameObject.FindGameObjectWithTag("PlayerDetection").GetComponent<PlayerDetection>();
    }
    private void Update()
    {

        if (boss.IntroFinished == true && !playerDetection.PlayerInArea)
        {

            if (movingLeft)
            {
                if (BossTransform.position.x >= leftEdge.position.x)
                    MoveInDirection(-1);
                else
                    DirectionChange();
            }
            else
            {
                if (BossTransform.position.x <= rightEdge.position.x)
                    MoveInDirection(1);
                else
                    DirectionChange();
            }
        }
        else if (boss.IntroFinished == true && playerDetection.PlayerInArea)
        {
            anim.SetTrigger("Run");
            
    
        }
    }

    private void DirectionChange()
    {
      
        anim.SetTrigger("Idle");
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
     
        anim.SetTrigger("Walk");

        //Make BossTransform face direction
        BossTransform.localScale = new Vector3(Mathf.Abs(enemyScale.x) * _direction,
            enemyScale.y, enemyScale.z);

        if (boss.EnragedState)
        {
            BossTransform.position = new Vector3(BossTransform.position.x + Time.deltaTime * _direction * (speed * 1.5f),
            BossTransform.position.y, BossTransform.position.z);
        }
        else
        {
            BossTransform.position = new Vector3(BossTransform.position.x + Time.deltaTime * _direction * speed,
                        BossTransform.position.y, BossTransform.position.z);
        }
        
    }
}
