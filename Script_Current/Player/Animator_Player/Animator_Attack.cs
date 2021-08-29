using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt.Addons.Community;
public class Animator_Attack : Animator_State
{
    private bool firstCheck=false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        firstCheck = false;
        animator.SetFloat("AttackSpeed",player.currentMotion.attackSpeed);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isFinished) return;
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        float normalizedTime = stateInfo.normalizedTime;

        #region »óÅÂ ³ªÅ¸³¿
        
        bool Anticipation = stateInfo.normalizedTime % 1.0f < player.currentMotion.anticipation.y;
        bool Contact = stateInfo.normalizedTime % 1.0f < player.currentMotion.contact.y;
        bool Delay = stateInfo.normalizedTime % 1.0f < player.currentMotion.delay.y;
        bool Recovery = stateInfo.normalizedTime % 1.0f < player.currentMotion.recovery.y;
        #endregion
        if (Anticipation)
        {
            
        }
        else if (Contact)
        {
            ActivateTrail(true);
        }
        else if (Delay)
        {
            ActivateTrail(false);
        }
        else if (Recovery)
        {
            if (InputManager.JS_Move.input != Vector3.zero)
            {
                FinishAttack();
                return;
            }
        }
        else
        {
            FinishAttack();
            return;
        }

        void FinishAttack()
        {
            DefinedEvent.Trigger(player.gameObject,GM.bolt_AttackFinish);
            player.currentCommand = 0;
            player.currentIndex = 0;
            player.currentMotion = null;
            animator.CrossFadeInFixedTime("Idle",0.2f);
            isFinished = true;
        }

        void ActivateTrail(bool activate)
        {
            if (player.equipManager.weaponMain != null && player.equipManager.weaponMain.trail.active==!activate)
            {
                player.equipManager.weaponMain.trail.active = activate;
            }
            if (player.equipManager.weaponSub != null && player.equipManager.weaponSub.trail.active==!activate)
            {
                player.equipManager.weaponSub.trail.active = activate;
            }
        }
        
    }
}
