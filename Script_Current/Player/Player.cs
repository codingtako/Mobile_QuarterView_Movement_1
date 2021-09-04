using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Bolt;
using Bolt.Addons.Community;
using Bolt.Addons.Community.ReturnEvents;
using JetBrains.Annotations;
using Ludiq;
using UnityEngine;
using Sirenix.OdinInspector;
using Pathfinding;
using Unity.Collections;

public class Player : MonoBehaviour
{
    public float timeScale = 0.5f;

    #region Conponent
    [HideInInspector]public Animator anim;
    [HideInInspector]public AIPath ai;
    [HideInInspector]public Player_Pointer pointer;
    [HideInInspector]public EquipManager equipManager;
    [HideInInspector]public WeaponData weaponData;
    #endregion
    #region 각종 State
    //루트모션의 이동,회전 상태
    public enum MoveState{ RootMotion=0,RootPosition=1,RootRotation=2,None=3}
    private MoveState moveState;
    //플레이어의 상태
    public enum PlayerState { Locomotion = 0, Attack=1,Roll=2  }
    [Sirenix.OdinInspector.ReadOnly]
    public PlayerState playerState;
    //공격 상태
    public enum AttackState { Anticipation=0, Contact =1, Delay =2, Recovery =3 }
    [Sirenix.OdinInspector.ReadOnly]
    public AttackState attackState;
    #endregion

    #region Attack관련 변수

    public enum Input_Type { Normal=0,Strong=1}
    public Input_Type input_Type;
    
    public int currentCommand = 0;//그냥 하나
    public int currentIndex = 0;
    public Motion currentMotion;
    #endregion
    private float moveSpeed = 1;

    #region EquipTransform
    [FoldoutGroup("Equip Transform")]
    public Transform leftHand, rightHand, shield, back;
    #endregion
    #region String Premake

    private string str_Roll = "Roll";
    private string str_RollFinish = "RollFinish";
    private string str_NormalizedTime = "NormalizedTime";
    private string str_LeftStickMagnitude = "LeftStickMagnitude";
    private string str_DegreeDifference = "DegreeDifference";
    #endregion
    public void OnAnimatorMove()
    {
        Vector3 moveVec;
        moveVec = (anim.rootPosition - transform.position);
        moveVec.y = 0;
        
        anim.SetBool("IsInTransition", anim.IsInTransition(0));
        switch (moveState)
        {
            case MoveState.RootMotion:
                ai.FinalizeMovement(transform.position + moveVec*moveSpeed, anim.rootRotation);
                break;
            case MoveState.RootPosition:
                ai.FinalizeMovement(transform.position + moveVec*moveSpeed, ai.transform.rotation);
                break;
            case MoveState.RootRotation:
                ai.FinalizeMovement(ai.transform.position, anim.rootRotation);
                break;
            case MoveState.None:
                ai.FinalizeMovement(ai.transform.position, ai.transform.rotation);
                break;

        }
    }
    public AnimatorStateInfo animatorStateInfo;
    #region Start
    public void SetComponent()
    {
        anim = GetComponent<Animator>();
        ai = GetComponentInParent<AIPath>();
        pointer = transform.parent.GetComponentInChildren<Player_Pointer>();
        equipManager = GetComponent<EquipManager>();
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = timeScale * 0.02f;
        weaponData = GetComponent<WeaponData>();
    }
    public void SetEvent()
    {
        InputManager.NB_Roll.onButtonDown.AddListener(On_NB_Roll_Down);
    }
    #endregion
    
    #region State관련 함수
        #region Locomotion(SuperState)
            public void Locomotion()
            {
                //ㅣ공격 선입력
                if (equipManager.IsAttackReady())
                {
                    if (equipManager.js_Type == EquipManager.JS_Type.Normal)
                    {
                        
                        currentCommand = weaponData.Convert_Command(Motion.Command.Normal, false);
                    }
                    else if (equipManager.js_Type == EquipManager.JS_Type.Strong)
                    {
                        
                        currentCommand = weaponData.Convert_Command(Motion.Command.Normal, true);
                    }
                    else
                    {
                        print("?");
                    }
                    
                    //--------------------------------------------------------------------------------------------------
                    currentMotion = weaponData.Command_Motion(currentCommand);
                    DefinedEvent.Trigger(gameObject,GM.bolt_Attack);
                    string stateName = "Attack_Ready " + weaponData.Commands.IndexOf(currentCommand);
                    anim.CrossFadeInFixedTime(stateName,currentMotion.readyTransitionDuration,0);
                    currentIndex += 1;
                    InputManager.JS_MainWaepon.isCalled = false;
                    InputManager.JS_Skill.isCalled = false;
                    return;
                }
                //회피 선입력
                if (IsRollReady())
                {
                    InputManager.NB_Roll.isCalled = false;
                    DefinedEvent.Trigger(gameObject,GM.nb_Roll_Down);
                }
            }
            #region Move(FlowState)

            
            public void Move()
            {
                if (InputManager.JS_Move.input == Vector3.zero) return;
                //기본적인 각도float인 "DegreeDifference" 설정을 한다.
                Vector3 applyInput = MainCamera.instance.transform.rotation *  InputManager.JS_Move.input;
                float applyAngle = 90 - Mathf.Atan2(applyInput.z, applyInput.x) * Mathf.Rad2Deg -
                                   transform.rotation.eulerAngles.y;
                applyAngle = MyLibrary.BetweenDegree(applyAngle);
                //회전 관련 처리를 한다.
                anim.SetFloat(str_LeftStickMagnitude, InputManager.JS_Move.input.magnitude);
                anim.SetFloat(str_DegreeDifference,
                    Mathf.Lerp(anim.GetFloat(str_DegreeDifference), applyAngle, 15 * Time.deltaTime));

            }
            
            #endregion
            #region Stop(FlowState)

            public void Stop()
            {
                anim.SetFloat(str_LeftStickMagnitude,0);
            }
            #endregion
            #region Idle(FlowState)
            public void Idle()
            {
                anim.SetFloat(str_DegreeDifference,0);
                anim.SetFloat(str_LeftStickMagnitude,0);
            }
            #endregion
            #region Transition

            public bool Idle_Move()
            {
                return InputManager.JS_Move.input != Vector3.zero;
            }
            public bool Move_Stop()
            {
                //"일단 밑의 2 경우이면, 무조건 트렌지션"
                
                #region case1:입력을 멈춘 경우
                bool case1 = InputManager.JS_Move.input == Vector3.zero;
                #endregion
                #region case2:(뒤 입력 할 경우) + (MoveStart말고 Move상태일 때)
                Vector3 applyInput = MainCamera.instance.transform.rotation *  InputManager.JS_Move.input;
                float applyAngle = 90 - Mathf.Atan2(applyInput.z, applyInput.x) * Mathf.Rad2Deg -
                                   transform.rotation.eulerAngles.y;
                applyAngle = MyLibrary.BetweenDegree(applyAngle);
                bool case2 = Mathf.Abs(applyAngle) > 150 && InputManager.JS_Move.input != Vector3.zero &&
                              IsName_AnimatorState("Move");
                #endregion 
                //print("CASE1: "+case1.ToString()+" , CASE2:"+case2.ToString());
                return case1 || case2;
            }

            public bool Stop_Idle()
            {
                //"Stop_Move보다 늦게 발생하기 때문에, 시간 지나면 입력에 상관없이 Idle로 보낸다."
                bool isStopFinished = (IsName_AnimatorState("Stop_R")||IsName_AnimatorState("Stop_L")) && animatorStateInfo.normalizedTime > 0.5;
                bool isIdle = IsName_AnimatorState("Idle");
                return (isStopFinished || isIdle);
            }
            public bool Stop_Move()
            {
                //(입력 있는 경우) + (정지 애니메이션 일 경우, 일정 ratio 이상일 때)
                bool isStopFinished = (IsName_AnimatorState("Stop_R")||IsName_AnimatorState("Stop_L")) && animatorStateInfo.normalizedTime > 0.1f;
                bool isIdle = IsName_AnimatorState("Idle")&& !anim.IsInTransition(0);
                //print("isStopFinished: "+isStopFinished.ToString()+" , isIdle:"+isIdle.ToString());
                return (isStopFinished || isIdle)&& InputManager.JS_Move.input!=Vector3.zero;
            }
            #endregion
        #endregion
        #region Roll(SuperState)
            #region OnEnterState

            public void RollEnter()
            {
                anim.SetTrigger(str_Roll);
                anim.SetFloat(str_DegreeDifference,0);
                anim.SetFloat(str_LeftStickMagnitude,0);
            }

            

            #endregion
            #region Transition(아직 없음)
            #endregion
        #endregion

        #region Attack
            public void Attack()
            {
                
            }
        #endregion
        #region Transition(SuperState)
        //Locomotion_Roll은 이벤트로 처리한다.
        public bool Roll_Locomotion()
        {
            if (!anim.IsInTransition(0)&&anim.GetFloat(str_NormalizedTime) > 0.65f
            && IsName_AnimatorState("Roll"))
            {
                anim.SetTrigger(str_RollFinish);
                return true;
            }
            else return false;
        }

        public bool Attack_Locomotion()
        {
            return false;
        }
        public bool Attack_Roll()
        {
            return false;
        }
        #endregion
    #endregion
    #region Set 함수
    public void Set_AnimatorStateInfo(AnimatorStateInfo info)
    {
        animatorStateInfo = info;
    }
    public void Set_BeginDegree()
    {
        Vector3 targetVec;
        float angleDist;
        if (InputManager.JS_Move.input == Vector3.zero)
        {
            targetVec= transform.parent.transform.rotation*Vector3.forward;
            angleDist = 0;
        }
        else
        {
            targetVec = Quaternion.Euler(0, MainCamera.instance.transform.rotation.eulerAngles.y, 0)
                        * InputManager.JS_Move.input;
            angleDist = STARTUP.Angle(targetVec, transform.parent.rotation * Vector3.forward);
        }
        
        anim.SetFloat("BeginDegree", -angleDist);
    }

    public void Set_MoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    public void Set_AnimSpeed(float speed)
    {
        anim.speed = speed;
    }

    public void Set_PointerState(Player_Pointer.PointerState pointerState)
    {
        pointer.Set_State(pointerState);
    }

    public void Set_MoveState(MoveState state)
    {
        moveState = state;
    }


    public void Set_PlayerState(PlayerState state)
    {
        playerState = state;
    }
    #endregion
    #region Get 함수
    #endregion
    #region Is 함수
    public bool IsName_AnimatorState(string stateName)
    {
        return animatorStateInfo.IsName(stateName);
    }

    public bool IsRollReady()
    {
        bool havePreInput = InputManager.NB_Roll.isCalled;
        bool canPreInput = Time.time - InputManager.NB_Roll.pressedTime < InputManager.NB_Roll.delay;
        
        if (havePreInput&&canPreInput) return true;
        else return false;

    }
    #endregion
    #region 기타 함수

    

    #endregion

    #region 이벤트 함수
    public void On_NB_Roll_Down()
    {
        //if(!IsName_AnimatorState("Roll"))DefinedEvent.Trigger(gameObject,GM.nb_Roll_Down);
    }

    
    #endregion

    #region 상태 검출


    public bool Can_Roll()
    {
        return playerState== PlayerState.Locomotion;
    }

    #endregion



}
