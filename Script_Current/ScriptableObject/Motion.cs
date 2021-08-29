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
    #region 포즈
    public enum Pose { LS_LF=0,LS_RF=1,RS_LF=2,RS_RF=3 }
    
    [BoxGroup("포즈")]
    [InfoBox("포즈 표기: (스윙방향_앞발)")]
    [LabelText("시작 포즈")]
    public Pose pose_Begin;
    [BoxGroup("포즈")]
    [LabelText("마무리 포즈")]
    public Pose pose_Finish;
    [BoxGroup("포즈")]
    [LabelText("동작 형태")]
    public ReadyMotionSet.MotionType poseMotionType;
    //공백
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _1;
    #endregion

    #region 조작,연계
    public enum Command { Normal=1, Delay =2, Charge =3}
    [Space]
    [LabelText("파생 가능 여부")]
    [BoxGroup("조작,연계")]
    public bool canDerive;
    [LabelText("커멘드")]
    [BoxGroup("조작,연계")]
    public Command command;
    [LabelText("Ready 트렌지션 시간")] [BoxGroup("조작,연계")]
    public float readyTransitionDuration = 0.1f;
    [LabelText("Attack 트렌지션 시간")] [BoxGroup("조작,연계")]
    public float attackTransitionDuration = 0.1f;
    [LabelText("Ready의 트렌지션 전 ratio")] [BoxGroup("조작,연계")]
    public float readyTransitionNoramlizedTime = 0.9f;
    [LabelText("Attack의 트렌지션 후 ratio")] [BoxGroup("조작,연계")]
    public float attackTransitionNoramlizedTime = 0.05f;
    //공백
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _3;
    #endregion

    #region 애니메이션 상태
    [Space]
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 anticipation;
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 contact;
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 delay;
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 recovery;
    [BoxGroup("애니메이션 상태")][Space]
    [MinMaxSlider(0,0.98f,true)]
    public Vector2 delayAttack;
    
    //공백
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _5;
    #endregion
}