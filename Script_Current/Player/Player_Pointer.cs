using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.DemiLib;
using UnityEngine;
using DG.Tweening;
using RootMotion.FinalIK;
using Shapes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Player_Pointer : MonoBehaviour
{
    private Animator anim;
    private Player player;
    private LookAtIK lookAtIk;
    [Range(0,180)]
    public float angleRange=160;
    public float lookSpeed = 10;
    public float arrowSpeed = 10;
    public Transform lookAt;
    public Transform arrow;
    private float lookAngle = 0;
    private float arrowAngle=0;
    public void Awake()
    {
        anim = GetComponent<Animator>();
        lookAtIk = GetComponent<LookAtIK>();
        player = transform.parent.GetComponentInChildren<Player>();
    }
    public enum PointerState
    {
        Point_and_LookAt = 0,
        Point = 1,
        LookAt =2,
        Deactivate = 3
    }
    private PointerState pointerState;

    #region 트렌지션 여부 체크(StateMachine) 함수
    public bool CanTransition(PointerState targetState)
    {
        return pointerState == targetState;
    }
    public bool CanActivate()
    {
        return InputManager.JS_MainWaepon.input != Vector3.zero||InputManager.JS_Skill.input != Vector3.zero;
    }
    #endregion

    #region 기타 함수
    private void Update_State(bool use_point, bool use_lookAt)
    {
        #region 입력 받아서 twist 최초 각도 확인
        Vector3 targetVec;
        float angleDist;
        if ( InputManager.JS_MainWaepon.input == Vector3.zero&&InputManager.JS_Skill.input == Vector3.zero)
        {
            targetVec= transform.parent.transform.rotation*Vector3.forward;
            angleDist = 0;
        }
        else
        {
            targetVec = Quaternion.Euler(0, MainCamera.instance.transform.rotation.eulerAngles.y, 0)
                        *  (InputManager.JS_MainWaepon.input+InputManager.JS_Skill.input);
            angleDist = STARTUP.Angle(targetVec, transform.parent.rotation * Vector3.forward);
        }
        #endregion
            
        if (use_point)
        {
            
            //arrowAngle 설정
            arrowAngle = Mathf.LerpAngle(arrowAngle, -angleDist, arrowSpeed * Time.deltaTime);
            //Position 설정
            Vector3 arrowPos = Quaternion.LookRotation(Quaternion.Euler(0, arrowAngle, 0) * new Vector3(0,0.85f,0.85f))*Vector3.forward;
            arrow.localPosition = arrowPos;
            //Rotation 설정
            arrowPos.y = 0;
            arrow.localRotation = Quaternion.LookRotation(arrowPos);
        }
        else
        {
            arrowAngle = Mathf.LerpAngle(arrowAngle, 0, arrowSpeed * Time.deltaTime);
            //Position 설정
            Vector3 arrowPos = Quaternion.LookRotation(Quaternion.Euler(0, arrowAngle, 0) * new Vector3(0,0.85f,0.85f))*Vector3.forward;
            arrow.localPosition = arrowPos;
            //Rotation 설정
            arrowPos.y = 0;
            arrow.localRotation = Quaternion.LookRotation(arrowPos);
        }

        
        if (use_lookAt)
        {
            #region 각도 180,-180에서 휙휙 바뀌는거 방지
            if ((-angleRange>=angleDist || angleDist>=angleRange))
            {
                if (Mathf.Abs(lookAngle)<5)
                {
                    if (angleDist < 0) angleDist = -angleRange;
                    else angleDist = angleRange;
                }
                else
                {
                    if (lookAngle < 0) angleDist = angleRange;
                    else angleDist = -angleRange;
                }
            }
            
            #endregion
            lookAngle = Mathf.Lerp(lookAngle, -angleDist, lookSpeed * Time.deltaTime);
            //Twist값 설정, LookAtIK의 target 이동
            player.anim.SetFloat("Twist", lookAngle * 0.5f);
            lookAt.localPosition = Quaternion.Euler(0, lookAngle, 0) * new Vector3(0, 2, 10);
        }
        else
        {
            lookAngle = Mathf.Lerp(lookAngle, 0, lookSpeed * Time.deltaTime);
            //Twist값 설정, LookAtIK의 target 이동
            player.anim.SetFloat("Twist",lookAngle*0.5f);
            lookAt.localPosition = Quaternion.Euler(0, lookAngle, 0) * new Vector3(0, 2, 10);
        }
        
    }
    public void Set_State(PointerState state)
    {
        pointerState = state;
    }
    #endregion
    
    #region PointerState(SuperState)
        #region Point_And_LookAt(FlowState)
        public void Point_And_LookAt_Enter()
        {
            pointerState = PointerState.Point_and_LookAt;
            anim.SetBool("Activate",true);
            
        }
        public void Point_And_LookAt()
        {
            Update_State(true,true);
        }
        #endregion
        #region Point(FlowState)
        public void Point_Enter()
        {
            pointerState = PointerState.Point;
            anim.SetBool("Activate",true);
        }
        public void Point()
        {
            Update_State(true,false);
        }
        #endregion
        #region LookAt(FlowState)
        public void LookAt_Enter()
        {
            pointerState = PointerState.LookAt;
            anim.SetBool("Activate",false);
        }
        public void LookAt()
        {
            Update_State(false,true);
        }
        #endregion
        #region Deactivate(FlowState)
        public void Deactivate_Enter()
        {
            pointerState = PointerState.Deactivate;
            anim.SetBool("Activate",false);
        }
        public void Deactivate()
        {
            Update_State(false,false);
        }
        #endregion
    #endregion
}
