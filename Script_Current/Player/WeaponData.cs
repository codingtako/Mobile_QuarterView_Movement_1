using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public Weapon_Main mainWeapon;

    public Weapon_Sub subWeapon;
//----------------------------------------------------------------------------------------------------------------------
    #region 커멘드별 스킬,모션 
    [HorizontalGroup("Commands")]
    [ShowInInspector]
    [ReadOnly]
    public Dictionary<int, Skill> Commands_Skill = new Dictionary<int, Skill>();
    //------------------------------------------------------------------------------------------------------------------
    [HorizontalGroup("Commands")]
    [ShowInInspector]
    [ReadOnly]
    public Dictionary<int, Motion> Commands_Motion = new Dictionary<int, Motion>();
    //------------------------------------------------------------------------------------------------------------------
    [ReadOnly]
    public List<int> preCommands = new List<int>();
    #endregion
//----------------------------------------------------------------------------------------------------------------------
    

    public int ConvertCommand(Motion.Command command)
    {
        switch (command)
        {
            case Motion.Command.Normal:
                return 1;
                break;
            case Motion.Command.Delay:
                return 2;
                break;
            case Motion.Command.Charge:
                return 3;
                break;
        }

        return -1;
    }
    [Button(ButtonSizes.Medium,Expanded = true)]
    public void AddSkill(int preCommand,bool isSpecial,Skill skill)
    {
        
        bool canAdd=true;
        Motion firstMotion = skill.GetMotions()[0];
        #region 먼저 preCommand의 맞음 여부를 체크한다
        //preCommand가 맞지 않는 경우 빠꾸
        if (preCommand!=0&&!preCommands.Contains(preCommand))
        {
            canAdd = false;
            Debug.LogWarning("PreCommand에 존재하는 값을 넣어주세요!");
        }else if (preCommand == 0)
        {
            foreach (var command  in preCommands)
            {
                if (STARTUP.FirstDigit(command) == 1 && firstMotion.command == Motion.Command.Normal && !isSpecial ) canAdd = false;
                if (STARTUP.FirstDigit(command) == 9 && firstMotion.command == Motion.Command.Normal && isSpecial ) canAdd = false;
                if (STARTUP.FirstDigit(command) == 2 && firstMotion.command == Motion.Command.Delay&& !isSpecial) canAdd = false;
                if (STARTUP.FirstDigit(command) == 8 && firstMotion.command == Motion.Command.Delay&& isSpecial ) canAdd = false;
                if (STARTUP.FirstDigit(command) == 3 && firstMotion.command == Motion.Command.Charge&& !isSpecial) canAdd = false;
                if (STARTUP.FirstDigit(command) == 7 && firstMotion.command == Motion.Command.Charge&& isSpecial ) canAdd = false;
                if(!canAdd)Debug.LogWarning("이미 해당 시작 커멘드가 있습니다!");
            }
        }
        #endregion


        #region 그 다음 커멘드를 등록한다.
        if (!canAdd) return;
        
        foreach (var motion in skill.GetMotions())
        {
            int _command=0;
            switch (motion.command)
            {
                case Motion.Command.Normal:
                    if (!isSpecial) _command = 1;
                    else _command = 9;
                    break;
                case Motion.Command.Delay:
                    if (!isSpecial) _command = 2;
                    else _command = 8;
                    break;
                case Motion.Command.Charge:
                    if (!isSpecial) _command = 3;
                    else _command = 7;
                    break;

            }
            if(preCommands.Contains(preCommand)) preCommands.Remove(preCommand);
            
            preCommand *= 10;
            preCommand +=_command;
            Commands_Skill.Add(preCommand,skill);
            Commands_Motion.Add(preCommand,motion);
            
            preCommands.Add(preCommand);
            
            
        }
        #endregion
    }
    public void SaveData()
    {
        
    }
    public void LoadData()
    {
        
    }
    public void ApplyAnimation()
    {
        
    }
    [Button(ButtonSizes.Medium,Expanded = true)]
    public void ClearData()
    {
        Commands_Motion.Clear();
        Commands_Skill.Clear();
        preCommands.Clear();
    }
}
