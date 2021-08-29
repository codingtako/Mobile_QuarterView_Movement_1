using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Motion", menuName = "Motion")]

public class Motion : ScriptableObject

{
    public ReadyMotionSet readyMotionSet;
    public float attackReadySpeed=1;
    public AnimationClip attack;
    public float attackSpeed=1;
    #region ����
    public enum Pose { LS_LF=0,LS_RF=1,RS_LF=2,RS_RF=3 }
    
    [BoxGroup("����")]
    [InfoBox("���� ǥ��: (��������_�չ�)")]
    [LabelText("���� ����")]
    public Pose pose_Begin;
    [BoxGroup("����")]
    [LabelText("������ ����")]
    public Pose pose_Finish;
    [BoxGroup("����")]
    [LabelText("���� ����")]
    public ReadyMotionSet.MotionType poseMotionType;
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
    [LabelText("Ready Ʈ������ �ð�")] [BoxGroup("����,����")]
    public float readyTransitionDuration = 0.1f;
    [LabelText("Attack Ʈ������ �ð�")] [BoxGroup("����,����")]
    public float attackTransitionDuration = 0.1f;
    [LabelText("Ready�� Ʈ������ �� ratio")] [BoxGroup("����,����")]
    public float readyTransitionNoramlizedTime = 0.9f;
    [LabelText("Attack�� Ʈ������ �� ratio")] [BoxGroup("����,����")]
    public float attackTransitionNoramlizedTime = 0.05f;
    //����
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _3;
    #endregion

    #region �ִϸ��̼� ����
    [Space]
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 anticipation;
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 contact;
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 delay;
    [BoxGroup("�ִϸ��̼� ����")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 recovery;
    [BoxGroup("�ִϸ��̼� ����")][Space]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 delayAttack;
    
    //����
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _5;
    #endregion
}