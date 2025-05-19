using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Boss_Attack_1 : StateMachineBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float damage;

    Boss boss;
    public bool canDamagePlayer = true;

    private bool canIncreaseDmg = true;
    //references
    private Health playerHealth;

    // onstateenter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        canDamagePlayer = true;
        boss = animator.GetComponent<Boss>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        if (boss.EnragedState && canIncreaseDmg)
        {
            canIncreaseDmg = false;
            damage += 1;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (canDamagePlayer && boss.PlayerInSight() && boss.PlayerCanTakeDamage)
        {
            playerHealth.TakeDamage(damage);
            canDamagePlayer = false;
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canDamagePlayer = false;
        
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
