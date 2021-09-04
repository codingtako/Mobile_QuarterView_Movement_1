using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_State : StateMachineBehaviour
{
    public Player player;
    protected bool isFinished = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        base.OnStateEnter(animator, stateInfo, layerIndex);
        isFinished = false;
        player = animator.GetComponent<Player>();
        player.animatorStateInfo = stateInfo;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        animator.SetFloat("NormalizedTime",stateInfo.normalizedTime);
    }

    
}
