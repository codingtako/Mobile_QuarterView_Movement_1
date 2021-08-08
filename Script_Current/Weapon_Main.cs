using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Main : Weapon
{
    public enum EquipType
    {
        oneHand_Sword=0,twoHand_Sword=1
    }

    public EquipType equipType;
    public override void Move(TargetTransform tt)
    {
        base.Move(tt);
    }
}
