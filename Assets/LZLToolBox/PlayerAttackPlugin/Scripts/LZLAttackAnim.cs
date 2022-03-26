using System.Collections;
using System.Collections.Generic;
using LZLToolBox.PlayerController;
using Sirenix.OdinInspector;
using UnityEngine;

public class LZLAttackAnim : StateMachineBehaviour
{
    [Tooltip("是否需要设置攻击状态")]
    public bool needSetAtkState = false;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //处于攻击状态
        if (needSetAtkState && animator.gameObject.GetComponent<LZLBaseAttackCtrl>())
        {
            animator.gameObject.GetComponent<LZLBaseAttackCtrl>().SetAttackState_Anim(true);
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //
    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //退出攻击状态
        if (needSetAtkState && animator.gameObject.GetComponent<LZLBaseAttackCtrl>())
        {
            animator.gameObject.GetComponent<LZLBaseAttackCtrl>().SetAttackState_Anim(false);
        }
        
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
