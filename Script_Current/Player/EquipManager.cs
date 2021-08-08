using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    private Player player;
    public Weapon_Main testMain;
    public Weapon_Sub testSub;
    
    [ReadOnly]
    public Weapon_Main weaponMain;
    [ReadOnly]
    public Weapon_Sub weaponSub;
    public enum EquipState
    {
        None =0,Unsheath_Ready =1,Unsheath =2, Sheath_Ready=3,Sheath=4,Unarm =5
    }

    private EquipState equipState;

    public void Start()
    {
        player = GetComponent<Player>();
        Create_Weapon(testMain,testSub);
    }
    [Button]
    public void Create_Weapon(Weapon_Main main,Weapon_Sub sub)
    {
        weaponMain = Instantiate(main);
        weaponSub = Instantiate(sub);
    }
    [Button]
    public void Create_Weapon(Weapon_Main main)
    {
        weaponMain = Instantiate(main);
        weaponSub = null;
    }

    public void Remove_Weapon()
    {
        if(weaponMain!=null) Destroy(weaponMain);
        if(weaponSub!=null) Destroy(weaponSub);

        weaponMain = null;
        weaponSub = null;
    }
    public void Set_EquipState(EquipState state)
    {
        bool moveMain = true;
        bool moveSub = true;

        #region 각 무기들을 보고, 메인,서브 움직일지 확인

        

        #endregion
        equipState = state;
        switch (state)
        {
            case EquipState.None:
                break;
            case EquipState.Unsheath_Ready:
                break;
            case EquipState.Unsheath:
                if(moveMain)weaponMain.Move(weaponMain.unsheathTransform);
                if(moveSub)weaponSub.Move(weaponSub.unsheathTransform);
                break;
            case EquipState.Sheath_Ready:
                break;
            case EquipState.Sheath:
                if(moveMain)weaponMain.Move(weaponMain.sheathTransform);
                if(moveSub)weaponSub.Move(weaponSub.sheathTransform);
                break;
            case EquipState.Unarm:
                break;
        }
    }
}
