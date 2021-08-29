using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.ProBuilder;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    
    public static Joystick JS_Move;

    public static Joystick JS_MainWaepon;

    public static Joystick JS_SubWeapon;

    public static Joystick JS_Skill;
    public static NormalButton NB_Roll;
    public Joystick _JS_Move;
    public Joystick _JS_MainWaepon;
    public Joystick _JS_SubWeapon;
    public Joystick _JS_Skill;
    public NormalButton _NB_Roll;
    // Start is called before the first frame update
    
    void Awake()
    {
        instance = this;
        JS_Move = _JS_Move;
        JS_MainWaepon = _JS_MainWaepon;
        JS_SubWeapon = _JS_SubWeapon;
        JS_Skill = _JS_Skill;
        NB_Roll = _NB_Roll;
    }

    public static bool IsPressingJS()
    {
        return JS_MainWaepon.isPressing || JS_Skill.isPressing;
    }

    public static bool Have_JS_PreInput()
    {
        bool normal_PreInput = (JS_MainWaepon.isCalled && Time.time - JS_MainWaepon.releasedTime < JS_MainWaepon.delay);
        bool strong_PreInput = (JS_Skill.isCalled && Time.time - JS_Skill.releasedTime < JS_Skill.delay);
        return normal_PreInput || strong_PreInput;
    }
    public static bool PreInput_JS_Normal()
    {
        bool normal_PreInput = (JS_MainWaepon.isCalled && Time.time - JS_MainWaepon.releasedTime < JS_MainWaepon.delay);
        bool strong_PreInput = (JS_Skill.isCalled && Time.time - JS_Skill.releasedTime < JS_Skill.delay);
        if (normal_PreInput)
        {
            if (strong_PreInput&& JS_MainWaepon.releasedTime>JS_Skill.releasedTime)return false;
            else return true;
        }
        else return false;
    }
    public static bool PreInput_JS_Strong()
    {
        bool normal_PreInput = (JS_MainWaepon.isCalled && Time.time - JS_MainWaepon.releasedTime < JS_MainWaepon.delay);
        bool strong_PreInput = (JS_Skill.isCalled && Time.time - JS_Skill.releasedTime < JS_Skill.delay);
        if (strong_PreInput)
        {
            if (normal_PreInput&& JS_MainWaepon.releasedTime<JS_Skill.releasedTime)return false;
            else return true;
        }
        else return false;
    }
}
