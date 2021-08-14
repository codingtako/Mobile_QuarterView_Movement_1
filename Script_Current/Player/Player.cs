using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Bolt;
using Bolt.Addons.Community;
using Ludiq;
using UnityEngine;
using Sirenix.OdinInspector;
using Pathfinding;
public class Player : MonoBehaviour
{
    public Animator anim;
    public AIPath ai;
    public Player_Pointer pointer;
    public EquipManager equipManager;
    public enum MoveState{ RootMotion=0,RootPosition=1,RootRotation=2,None=3}

    private MoveState moveState;
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
    public bool transitionWaiting;
    #region Start
    public void SetComponent()
    {
        anim = GetComponent<Animator>();
        ai = GetComponentInParent<AIPath>();
        pointer = transform.parent.GetComponentInChildren<Player_Pointer>();
        equipManager = GetComponent<EquipManager>();
    }
    public void SetEvent()
    {
        InputManager.NB_Roll.onButtonDown.AddListener(On_NB_Roll_Down);
    }
    #endregion
    
    #region State관련 함수
        #region Locomotion(SuperState)
        
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
                              Check_AnimatorStateInfo("Move");
                #endregion 
                //print("CASE1: "+case1.ToString()+" , CASE2:"+case2.ToString());
                return case1 || case2;
            }

            public bool Stop_Idle()
            {
                //"Stop_Move보다 늦게 발생하기 때문에, 시간 지나면 입력에 상관없이 Idle로 보낸다."
                bool isStopFinished = (Check_AnimatorStateInfo("Stop_R")||Check_AnimatorStateInfo("Stop_L")) && animatorStateInfo.normalizedTime > 0.5;
                bool isIdle = Check_AnimatorStateInfo("Idle");
                return (isStopFinished || isIdle);
            }
            public bool Stop_Move()
            {
                //(입력 있는 경우) + (정지 애니메이션 일 경우, 일정 ratio 이상일 때)
                bool isStopFinished = (Check_AnimatorStateInfo("Stop_R")||Check_AnimatorStateInfo("Stop_L")) && animatorStateInfo.normalizedTime > 0.1f;
                bool isIdle = Check_AnimatorStateInfo("Idle")&& !anim.IsInTransition(0);
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
        #region Transition(SuperState)
        //Locomotion_Roll은 이벤트로 처리한다.
        public bool Roll_Locomotion()
        {
            if (!anim.IsInTransition(0)&&anim.GetFloat(str_NormalizedTime) > 0.65f
            && Check_AnimatorStateInfo("Roll"))
            {
                anim.SetTrigger(str_RollFinish);
                return true;
            }
            else return false;
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

    public void Set_EquipState(EquipManager.EquipState equipState)
    {
        //equipManager.Set_EquipState(equipState);
    }
    #endregion
    #region Get 함수
    #endregion
    #region Check 함수
    public bool Check_AnimatorStateInfo(AnimatorStateInfo info)
    {
        return animatorStateInfo.shortNameHash == info.shortNameHash;
    }
    public bool Check_AnimatorStateInfo(int shortNameHash)
    {
        return animatorStateInfo.shortNameHash == shortNameHash;
    }
    public bool Check_AnimatorStateInfo(string stateName)
    {
        return animatorStateInfo.IsName(stateName);
    }
    public bool Check_NormalizedTime(float normalizedTime)
    {
        return animatorStateInfo.normalizedTime < normalizedTime;
    }
    public bool Check_NormalizedTime(AnimatorStateInfo info)
    {
        return animatorStateInfo.normalizedTime < info.normalizedTime;
    }
    #endregion
    #region 기타 함수
    

    #endregion

    #region 이벤트 함수

    public void On_NB_Roll_Down()
    {
        if(!Check_AnimatorStateInfo("Roll"))DefinedEvent.Trigger(gameObject,GM.nb_Roll_Down);
    }

    #endregion



}
