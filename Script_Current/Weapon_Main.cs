using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Weapon_Main : Weapon
{
    public enum EquipType
    {
        oneHand_Sword=0,twoHand_Sword=1
    }

    public override void StartSetting(EquipManager e)
    {
        base.StartSetting(e);
        switch (equipType)
        {
            case EquipType.oneHand_Sword:
                unsheathReadyTransform.parent = player.back;
                unsheathTransform.parent = player.rightHand;
                sheathReadyTransform.parent = player.rightHand;
                sheathTransform.parent = player.back;
                break;
            case EquipType.twoHand_Sword:
                unsheathReadyTransform.parent = player.back;
                unsheathTransform.parent = player.rightHand;
                sheathReadyTransform.parent = player.rightHand;
                sheathTransform.parent = player.back;
                break;
        }
    }
    public EquipType equipType;
    public override void Move(TargetTransform tt,float delay=0.0f)
    {
        base.Move(tt,delay);
    }
}
