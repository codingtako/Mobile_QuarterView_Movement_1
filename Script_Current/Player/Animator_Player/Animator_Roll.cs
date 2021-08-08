using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Roll : Animator_State
{
    public float turnSpeed=20;
    private Quaternion targetRot;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (InputManager.JS_Move.input != Vector3.zero)
        {
            targetRot = Quaternion.Euler(0, MainCamera.instance.transform.rotation.eulerAngles.y, 0)
                        * Quaternion.LookRotation(InputManager.JS_Move.input);
        }
        else
        {
            targetRot = Quaternion.LookRotation(player.ai.transform.rotation * Vector3.forward);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        player.ai.transform.rotation = Quaternion.Lerp(player.ai.transform.rotation,targetRot,turnSpeed*Time.deltaTime );
    }
}