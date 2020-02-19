using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAttackLoop : StateMachineBehaviour
{
    [SerializeField] private float minStateLength;
    [SerializeField] private float maxStateLength;

    private float _stateTime;
    private float _stateTimer;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stateTime = Random.Range(minStateLength, maxStateLength);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stateTimer < _stateTime)
            _stateTimer += Time.deltaTime;
        else
            animator.SetTrigger("idle");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stateTimer = 0;
        _stateTime = 0;

        animator.ResetTrigger("idle");
    }
}
