using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GM : MonoBehaviour
{
    
    public static GM gm;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    #region 이벤트

    #region LS
    public static UnityEvent JS_Move_Drag_Begin;
    public static  UnityEvent JS_Move_Drag;
    public static  UnityEvent JS_Move_Pressed;
    public static  UnityEvent JS_Move_Released;
    public static  UnityEvent JS_Move_Canceled;
    #endregion
    #region RS
    public static  UnityEvent JS_MainWeapon_Drag_Begin;
    public static  UnityEvent JS_MainWeapon_Drag;
    public static  UnityEvent JS_MainWeapon_Pressed;
    public static  UnityEvent JS_MainWeapon_Released;
    public static  UnityEvent JS_MainWeapon_Canceled;
    #endregion
    #region RS2
    public static  UnityEvent JS_Skill_Drag_Begin;
    public static  UnityEvent JS_Skill_Drag;
    public static  UnityEvent JS_Skill_Pressed;
    public static  UnityEvent JS_Skill_Released;
    public static  UnityEvent JS_Skill_Canceled;
    #endregion
    #region RB
    public static  UnityEvent NB_Roll_Down;
    public static NB_Roll_Down nb_Roll_Down = new NB_Roll_Down();
    public static  UnityEvent NB_Roll_Up;
    public static NB_Roll_Up nb_Roll_Up = new NB_Roll_Up();
    #endregion

    public static Bolt_Attack bolt_Attack = new Bolt_Attack();
    public static Bolt_AttackFinish bolt_AttackFinish = new Bolt_AttackFinish();
    public static Bolt_LeftHandAttack bolt_LeftHandAttack = new Bolt_LeftHandAttack();
    public static Bolt_RightHandAttack bolt_RightHandAttack = new Bolt_RightHandAttack();
    

    public UnityEvent EUpdate;
    public UnityEvent ELateUpeate;
    public UnityEvent EFixedUPdate;

    
    #endregion

    public void Awake()
    {
        gm = this;
        JS_Move_Canceled = new UnityEvent();
        JS_Move_Drag = new UnityEvent();
        JS_Move_Pressed = new UnityEvent();
        JS_Move_Released = new UnityEvent();
        JS_Move_Drag_Begin = new UnityEvent();

        JS_MainWeapon_Canceled = new UnityEvent();
        JS_MainWeapon_Drag = new UnityEvent();
        JS_MainWeapon_Pressed = new UnityEvent();
        JS_MainWeapon_Released = new UnityEvent();
        JS_MainWeapon_Drag_Begin = new UnityEvent();

        JS_Skill_Canceled = new UnityEvent();
        JS_Skill_Drag = new UnityEvent();
        JS_Skill_Pressed = new UnityEvent();
        JS_Skill_Released = new UnityEvent();
        JS_Skill_Drag_Begin = new UnityEvent();

        EUpdate = new UnityEvent();
        ELateUpeate = new UnityEvent();
        EFixedUPdate = new UnityEvent();

        NB_Roll_Down = new UnityEvent();
        NB_Roll_Up = new UnityEvent();
    }

    public static void AddListener(UnityEvent _event,UnityAction _action)
    {
        _event.AddListener(_action);
    }

    public static void RemoveListener(UnityEvent _event, UnityAction _action)
    {
        _event.RemoveListener(_action);
    }
}
