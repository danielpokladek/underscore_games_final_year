using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTwoBehaviour : StateMachineBehaviour
{
    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;
    
    private float timer;
    private float rand;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rand = Random.Range(minLength, maxLength);
        timer = rand;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0)
            SelectState(animator);
        else
            timer -= Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void SelectState(Animator animator)
    {
        int randState;

        randState = Random.Range(0, 2);
        //randState = 0;
        
//        if (randState == 0)
//            animator.SetTrigger("thumpUP");
//        
//        if (randState == 1)
//            animator.SetTrigger("shoot");
    }
}
