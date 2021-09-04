using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Bolt.Addons.Community;
using DG.Tweening.Core.Easing;

public class Animator_Attack : Animator_State
{
    private bool firstCheck=false;
    private bool chargeCheck = false;//Recovery�Ǿ��� ��, �������¶�� �ִϸ��̼��� �����.
    private bool pressedCheck_Normal = false;//���� chargeCheck �Ǻ� �� ���ȴ�.
    private bool releasedCheck_Normal = false;//���� chargeCheck �Ǻ� �� ���ȴ�.
    private bool pressedCheck_Strong = false;//���� chargeCheck �Ǻ� �� ���ȴ�.
    private bool releasedCheck_Strong = false;//���� chargeCheck �Ǻ� �� ���ȴ�.
    private bool nextCommand_Normal_Normal = false;
    private bool nextCommand_Normal_Delay = false;
    private bool nextCommand_Strong_Normal = false;
    private bool nextCommand_Strong_Delay = false;
    private bool nextCommand_Normal = false;
    private bool nextCommand_Strong = false;
    private bool normalCharging = false;//���� ���� ������� ���� ���� ����, isChargingȰ��ȭ.
    private bool strongCharging = false;//���� ���� ������� ���� ���� ����, isChargingȰ��ȭ.
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        firstCheck = false;
        animator.SetFloat("AttackSpeed",player.currentMotion.attackSpeed);

        #region ���� ����
        nextCommand_Normal_Normal = player.weaponData.Commands.Contains
            (player.currentCommand*10 + player.weaponData.Convert_Command(Motion.Command.Normal,false));
        nextCommand_Normal_Delay = player.weaponData.Commands.Contains
            (player.currentCommand*10 + player.weaponData.Convert_Command(Motion.Command.Delay,false));
        nextCommand_Strong_Normal = player.weaponData.Commands.Contains
            (player.currentCommand*10 + player.weaponData.Convert_Command(Motion.Command.Normal,true));
        nextCommand_Strong_Delay = player.weaponData.Commands.Contains
            (player.currentCommand*10 + player.weaponData.Convert_Command(Motion.Command.Delay,true));
        nextCommand_Normal = nextCommand_Normal_Delay || nextCommand_Normal_Normal;
        nextCommand_Strong = nextCommand_Strong_Delay || nextCommand_Strong_Normal;
        

        #endregion
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isFinished) return;
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        float normalizedTime = stateInfo.normalizedTime;
        #region ���� ��Ÿ��
        
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
            if (nextCommand_Normal && InputManager.PreInput_JS_Normal())
            {
                Debug.Log("NORMAL");
            }
            else if (nextCommand_Strong && InputManager.PreInput_JS_Strong())
            {
                Debug.Log("STRONG");
            }
        }
        else if (Recovery)
        {
            //�̵� �� ĵ��
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
