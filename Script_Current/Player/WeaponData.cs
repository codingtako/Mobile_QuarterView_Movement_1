using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    
    #region State해쉬
    [ShowInInspector][HorizontalGroup("DIC")]
    public Dictionary<int, string> Commands_State = new Dictionary<int, string>();
    [ShowInInspector][HorizontalGroup("DIC")]
    public Dictionary<int, string> Commands_ReadyState = new Dictionary<int, string>();
    #endregion
    
    #region 컴포넌트

    private EquipManager equipManager;
    private Animator anim;

    #endregion
//----------------------------------------------------------------------------------------------------------------------
    #region 커멘드별 스킬,모션
    [HorizontalGroup("Commands")] [ShowInInspector] [ReadOnly]
    public List<int> Commands = new List<int>();
    //------------------------------------------------------------------------------------------------------------------
    [HorizontalGroup("Commands")] [ShowInInspector] [ReadOnly]
    public List<Skill> Skills = new List<Skill>();
    //------------------------------------------------------------------------------------------------------------------
    [HorizontalGroup("Commands")] [ShowInInspector] [ReadOnly]
    public List<Motion> Motions = new List<Motion>();
    //------------------------------------------------------------------------------------------------------------------
    [ReadOnly]
    public List<int> preCommands = new List<int>();
    #endregion
//----------------------------------------------------------------------------------------------------------------------
    public void Start()
    {
        equipManager = GetComponent<EquipManager>();
        anim = GetComponent<Animator>();
        ChangeAnimation();
    }

    #region 커멘드 추가,저장,로드 관련 함수
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
            _command = Convert_Command(motion.command, isSpecial);
            if(preCommands.Contains(preCommand)) preCommands.Remove(preCommand);
            
            preCommand *= 10;
            preCommand +=_command;
            Commands.Add(preCommand);
            Skills.Add(skill);
            Motions.Add(motion);
            
            preCommands.Add(preCommand);
            
            
        }
        #endregion
    }
    public int Convert_Command(Motion.Command command,bool isStrong)
    {
        switch (command)
        {
            case Motion.Command.Normal:
                if (isStrong)
                {
                    return 9;
                }
                else
                {
                    return 1;
                }
                break;
            case Motion.Command.Delay:
                if (isStrong)
                {
                    return 2;
                }
                else
                {
                    return 8;
                }
                break;
            case Motion.Command.Charge:
                if (isStrong)
                {
                    return 3;
                }
                else
                {
                    return 7;
                }
                break;
        }

        return 0;
    }
    
    //------------------------------------------------------------------------------------------------------------------
    [Button(ButtonSizes.Medium,Expanded = true)]
    public void ClearData()
    {
        Commands.Clear();
        Motions.Clear();
        Skills.Clear();
        preCommands.Clear();
    }
    public void SaveData()
    {
        
    }
    public void LoadData()
    {
        
    }
    #endregion

    #region 애니메이션 변경

    public void ChangeAnimation()
    {
        for (int i = 0; i < Commands.Count; i++)
        {
            Commands_State.Add(Commands[i],"Attack "+i.ToString());
            Commands_ReadyState.Add(Commands[i],"Attack_Ready "+i.ToString());
        }
        
        AnimatorOverrideController animatorOverride =equipManager.UpdatedAnimatorOverrideController();
        
        #region 공격 모션
        //animatorOverride[skill_Ready_State] = Motions[skill_Ready_Index].attack_Ready;
        //animatorOverride[normal_Ready_State] = Motions[skill_Ready_Index].attack_Ready;
        for (int i = 0; i < Motions.Count; i++)
        {
            //if(i== skill_Ready_Index|| i==normal_Ready_Index) continue;
            animatorOverride[Commands_State[Commands[i]]] = Motions[i].attack;
            if (i > 0)
            {
                animatorOverride[Commands_ReadyState[Commands[i]]] = Motions[i].readyMotionSet
                    .Get_ReadyClip(Motions[i-1].poseMotionType,Motions[i].poseMotionType);
            }
            else
            {
                animatorOverride[Commands_ReadyState[Commands[i]]] = Motions[i].readyMotionSet
                    .Get_FirstReadyClip(Motions[i].poseMotionType);
            }
            

        }
        #endregion

        anim.runtimeAnimatorController = animatorOverride;
    }

    public Skill Command_Skill(int command)
    {
        int index = Commands.IndexOf(command);
        return Skills[index];
    }

    public Motion Command_Motion(int command)
    {
        int index = Commands.IndexOf(command);
        return Motions[index];
    }
    public Skill Command_BeforeSkill(int command)
    {
        if (command < 10)
        {
            Debug.LogError("두번째 이상의 커멘드를 입력하세요!");
            return null;
        }
        int index = Commands.IndexOf((int)(command*0.1f));
        return Skills[index];
    }
    public Motion Command_BeforeMotion(int command)
    {
        if (command < 10)
        {
            Debug.LogError("두번째 이상의 커멘드를 입력하세요!");
            return null;
        }
        int index = Commands.IndexOf((int)(command*0.1f));
        return Motions[index];
    }

    #endregion
}
