using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sub : Weapon
{
    public enum EquipType
    {
        shield=0,bow=1,dagger=2
    }

    public bool optional = true;
    public EquipType equipType;

    public override void Start()
    {
        base.Start();
        switch (equipType)
        {
            case EquipType.shield:
                unsheathTransform.parent = player.shield;
                sheathTransform.parent = player.back;
                break;
            case EquipType.bow:
                unsheathTransform.parent = player.leftHand;
                sheathTransform.parent = player.back;
                break;
            case EquipType.dagger:
                unsheathTransform.parent = player.leftHand;
                sheathTransform.parent = player.back;
                break;
        }
    }
    public override void Move(TargetTransform tt)
    {
        base.Move(tt);
    }
}
