using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeBossStageTwoIdle : StateMachineBehaviour
{
    [SerializeField] private float stateMinLength;
    [SerializeField] private float stateMaxLength;

    private float _stateTime;
    private float _stateTimer;
    private int _nextState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _nextState = Random.Range(0, 3);
        _stateTime = Random.Range(stateMinLength, stateMaxLength);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stateTimer < _stateTime)
            _stateTimer += Time.deltaTime;
        else
            SetNextState(animator);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _stateTimer = 0;
        _nextState = 0;

        animator.ResetTrigger("");
        animator.ResetTrigger("");
        animator.ResetTrigger("");
    }

    private void SetNextState(Animator animator)
    {
        if (_nextState == 0)
            animator.SetTrigger("rotatingSpiral");
        else if (_nextState == 1)
            animator.SetTrigger("expandingBarriers");
        else if (_nextState == 2)
            animator.SetTrigger("attackStomp");
    }
}
