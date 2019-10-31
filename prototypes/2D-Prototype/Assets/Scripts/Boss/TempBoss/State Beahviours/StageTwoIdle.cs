using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTwoIdle : StateMachineBehaviour
{
    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;

    private BossController bossController;
    
    private float lengthTimer;
    private float randTime;
    private bool nextStageSet;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Choose random length of the idle state, between the min & max values.
        randTime = Random.Range(minLength, maxLength);

        bossController = animator.gameObject.GetComponent<BossController>();
        bossController.EnableDamage();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (lengthTimer >= randTime)
            SelectState(animator);
        else
            lengthTimer += Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset values on state exit.
        animator.ResetTrigger("idle");

        lengthTimer = 0.0f;
        nextStageSet = false;
    }

    private void SelectState(Animator animator)
    {
        if (nextStageSet)
            return;

        int nextState = 0;
        nextState = Random.Range(0, 2);
        
        if (nextState == 0)
            animator.SetTrigger("spawnEnemy");
        
        if (nextState == 1)
            animator.SetTrigger("shootAttack");
    }
}
