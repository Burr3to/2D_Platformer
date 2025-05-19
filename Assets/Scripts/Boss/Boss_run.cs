using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_run : StateMachineBehaviour
{
    Transform Player;
    Rigidbody2D rb;
    Boss boss;
    private PlayerDetection playerDetection;

    [Header ("Boss parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private float delayBetweenAtacks;

    private bool hasEnteredState = false;
    //bordel na tento script
    int RightorLeft;
    int rand;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rand = Random.Range(1, 4); 
        

        playerDetection = GameObject.FindGameObjectWithTag("PlayerDetection").GetComponent<PlayerDetection>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();

        if (boss.EnragedState && !hasEnteredState)
        {
            speed += 1f;
        }

        hasEnteredState = true;
    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!playerDetection.PlayerInArea)
        {
            animator.SetTrigger("Walk");
        }
        
        boss.LookAtPlayer();

        
        if (boss.isFlipped) //medzera medzi boss a mnou aby ma neposuval tak
        {
            RightorLeft = -1;
        }
        else RightorLeft = 1;

        Vector2 target = new Vector2(Player.position.x + RightorLeft, rb.position.y); //x suradnica hrac, Y je objekt nech nemeni y miesto
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);

        rb.MovePosition(newPos);



        delayBetweenAtacks -= Time.deltaTime;
        if ((Vector2.Distance(Player.position, rb.position) <= attackRange) && delayBetweenAtacks <= 0)
        {            
            if (rand == 1)
            {
                delayBetweenAtacks += 1f;
                animator.SetTrigger("Attack_3");            
            }
            else if (rand == 2)
            {
                delayBetweenAtacks += 2f;
                animator.SetTrigger("Attack_2");
            } 
            else if (rand == 3)
            {
                delayBetweenAtacks += 1.5f;
                animator.SetTrigger("Attack_1");
            }
        }

    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        animator.ResetTrigger("Attack_1");  //lebo sa moze attack viac triggerovat
        animator.ResetTrigger("Attack_2");
        animator.ResetTrigger("Attack_3");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
