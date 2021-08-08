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
    
    #region 포즈
    public enum Pose { LS_LF=0,LS_RF=1,RS_LF=2,RS_RF=3 }
    
    [BoxGroup("포즈")]
    [InfoBox("포즈 표기: (스윙방향_앞발)")]
    [LabelText("시작 포즈")]
    public Pose pose_Begin;
    [BoxGroup("포즈")]
    [LabelText("마무리 포즈")]
    public Pose pose_Finish;
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
    [LabelText("다음 커멘드 목록")]
    [BoxGroup("조작,연계")]
    public List<Command> nextCommands = new List<Command>();

    //공백
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _3;
    #endregion

    #region 애니메이션 상태
    [Space]
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,1,true)]
    public Vector2 anticipation;
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,1,true)]
    public Vector2 contact;
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,1,true)]
    public Vector2 delay;
    [BoxGroup("애니메이션 상태")]
    [MinMaxSlider(0,1,true)]
    public Vector2 recovery;
    
    //공백
    [ShowInInspector]
    [ReadOnly]
    [HideLabel]
    private bool _5;
    #endregion

    #region 특수 커멘드
    [BoxGroup("특수 커멘드")]
    [ShowIf("Check_Command_DelayAttack")]
    [Space]
    [MinMaxSlider(0,1,true)]
    public Vector2 delayAttack;
    [BoxGroup("특수 커멘드")]
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