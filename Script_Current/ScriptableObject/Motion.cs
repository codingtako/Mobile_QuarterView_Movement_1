using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Motion", menuName = "Motion")]

public class Motion : ScriptableObject

{
    
   
    public AnimationClip unsheath_Ready;
    public AnimationClip attack_Ready;
    public AnimationClip attack;
    
    #region ����
    public enum Pose { LS_LF=0,LS_RF=1,RS_LF=2,RS_RF=3 }
    
    [BoxGroup("����")]
    [InfoBox("���� ǥ��: (��������_�չ�)")]
    [LabelText("���� ����")]
    public Pose pose_Begin;
    [BoxGroup("����")]
    [LabelText("������ ����")]
    public Pose pose_Finish;
    //����
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _1;
    #endregion

    #region ����,����
    public enum Command { Normal=1, Delay =2, Charge =3}
    [Space]
    [LabelText("�Ļ� ���� ����")]
    [BoxGroup("����,����")]
    public bool canDerive;
    [LabelText("Ŀ���")]
    [BoxGroup("����,����")]
    public Command command;
    [LabelText("���� Ŀ��� ���")]
    [BoxGroup("����,����")]
    public List<Command> nextCommands = new List<Command>();

    //����
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _3;
    #endregion

    #region �ִϸ��̼� ����
    [Space]
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,1,true)]
    public Vector2 anticipation;
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,1,true)]
    public Vector2 contact;
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,1,true)]
    public Vector2 delay;
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,1,true)]
    public Vector2 recovery;
    
    //����
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _5;
    #endregion

    #region Ư�� Ŀ���
    [BoxGroup("Ư�� Ŀ���")]
    [ShowIf("Check_Command_DelayAttack")]
    [Space]
    [MinMaxSlider(0,1,true)]
    public Vector2 delayAttack;
    [BoxGroup("Ư�� Ŀ���")]
    [ShowIf("Check_Command_ChargeAttack")]
    [MinMaxSlider(0,1,true)]
    public Vector2 chargeAttack;
    #endregion
    
    
    #region Odin_Bool
    public bool Check_Command_DelayAttack()
    {
            return nextCommands.Contains(Command.Delay);
        
    }
    public bool Check_Command_ChargeAttack()
    {
        return nextCommands.Contains(Command.Charge);
        
    }
    #endregion
}