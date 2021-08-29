using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MiscUtil.Xml.Linq.Extensions;
using UnityEngine;

public class Animator_Ready : Animator_State
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetFloat("AttackReadySpeed",player.currentMotion.attackReadySpeed);
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isFinished) return;
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        //����� Ready�ִϸ��̼��� �÷��� ���� ��� > Attack���� Ʈ�������Ѵ�.
        if (stateInfo.normalizedTime % 1.0f > player.currentMotion.readyTransitionNoramlizedTime)
        {
            string stateName = "Attack " + (player.currentIndex-1);
            animator.CrossFadeInFixedTime(stateName,player.currentMotion.attackTransitionDuration
                ,0,player.currentMotion.attackTransitionNoramlizedTime*player.currentMotion.attack.length);
            //Debug.Log("BEFORE1: "+player.currentMotion.attackTransitionNoramlizedTime);
            //Debug.Log("BEFORE2: "+player.currentMotion.attack.length);
            isFinished = true;
            return;
        }
    }
}
