using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MiscUtil.Xml.Linq.Extensions;
using UnityEngine;

public class Animator_Ready : Animator_State
{
    private Quaternion targetRot;
    private Quaternion startRot;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetFloat("AttackReadySpeed",player.currentMotion.attackReadySpeed);
        Vector3 targetVec = InputManager.JS_Move.lastInput;
        if (player.equipManager.js_Type == EquipManager.JS_Type.Normal && InputManager.JS_MainWaepon.dragged)
        {
            targetVec = Quaternion.Euler(0,MainCamera.instance.transform.rotation.eulerAngles.y,0)*InputManager.JS_MainWaepon.lastInput;
        }
        else if (player.equipManager.js_Type == EquipManager.JS_Type.Strong && InputManager.JS_Skill.dragged)
        {
            targetVec = Quaternion.Euler(0,MainCamera.instance.transform.rotation.eulerAngles.y,0)*InputManager.JS_Skill.lastInput;
        }
        else
        {
            targetVec = player.transform.rotation * Vector3.forward;
        }
        targetRot = Quaternion.LookRotation(targetVec);
        startRot = player.ai.transform.rotation;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isFinished) return;
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        //충분히 Ready애니메이션을 플레이 했을 경우 > Attack으로 트렌지션한다.
        if (stateInfo.normalizedTime % 1.0f > player.currentMotion.readyTransitionNoramlizedTime)
        {
            string stateName = "Attack " + (player.currentIndex-1);
            animator.CrossFadeInFixedTime(stateName,player.currentMotion.attackTransitionDuration
                ,0,player.currentMotion.attackTransitionNoramlizedTime*player.currentMotion.attack.length);
            player.ai.transform.rotation = targetRot;
            isFinished = true;
            return;
        }
        else
        {
            float ratio = stateInfo.normalizedTime%1.0f/player.currentMotion.readyTransitionNoramlizedTime;
            player.ai.transform.rotation = Quaternion.Lerp(startRot,targetRot,ratio);
        }
    }
}
