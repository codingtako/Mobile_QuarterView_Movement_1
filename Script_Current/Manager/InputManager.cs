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

    // Update is called once per frame
    void Update()
    {
        
    }
}
