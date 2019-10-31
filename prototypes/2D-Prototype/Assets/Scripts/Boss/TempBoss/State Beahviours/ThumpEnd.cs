using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumpEnd : StateMachineBehaviour
{
    private int nextState;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        nextState = Random.Range(0, 2);
        
        if (nextState == 0)
            animator.SetTrigger("thumpAttack");
        
        if (nextState == 1)
            animator.SetTrigger("idle");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("thumpAttack");
    }
}
