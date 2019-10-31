using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneShoot : StateMachineBehaviour
{
    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;

    private float lengthTimer;
    private float randTime;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Choose random length of the idle state, between the min & max values.
        randTime = Random.Range(minLength, maxLength);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (lengthTimer >= randTime)
            SelectNextState(animator);
        else
            lengthTimer += Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lengthTimer = 0.0f;
        
        animator.ResetTrigger("shootAttack");
    }

    private void SelectNextState(Animator animator)
    {
        animator.SetTrigger("idle");
    }
}
