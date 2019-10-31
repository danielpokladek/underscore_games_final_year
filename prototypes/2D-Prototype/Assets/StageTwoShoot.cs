using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTwoShoot : StateMachineBehaviour
{
    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;

    private float lengthTimer;
    private float randTime;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Choose random length of the attack state.
        randTime = Random.Range(minLength, maxLength);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (lengthTimer >= randTime)
            animator.SetTrigger("idle");
        else
            lengthTimer += Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("shootAttack");

        lengthTimer = 0.0f;
    }
}
