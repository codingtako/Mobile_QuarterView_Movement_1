using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    #region State�ؽ�
    [ShowInInspector][HorizontalGroup("DIC")]
    public Dictionary<int, string> Commands_State = new Dictionary<int, string>();
    [ShowInInspector][HorizontalGroup("DIC")]
    public Dictionary<int, string> Commands_ReadyState = new Dictionary<int, string>();
    #endregion
    
    #region ������Ʈ

    private EquipManager equipManager;
    private Animator anim;

    #endregion
//----------------------------------------------------------------------------------------------------------------------
    #region Ŀ��庰 ��ų,���
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

    #region Ŀ��� �߰�,����,�ε� ���� �Լ�
    [Button(ButtonSizes.Medium,Expanded = true)]
    public void AddSkill(int preCommand,bool isSpecial,Skill skill)
    {
        
        bool canAdd=true;
        Motion firstMotion = skill.GetMotions()[0];
        #region ���� preCommand�� ���� ���θ� üũ�Ѵ�
        //preCommand�� ���� �ʴ� ��� ����
        if (preCommand!=0&&!preCommands.Contains(preCommand))
        {
            canAdd = false;
            Debug.LogWarning("PreCommand�� �����ϴ� ���� �־��ּ���!");
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
                if(!canAdd)Debug.LogWarning("�̹� �ش� ���� Ŀ��尡 �ֽ��ϴ�!");
            }
        }
        #endregion


        #region �� ���� Ŀ��带 ����Ѵ�.
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
            Commands.Add(preCommand);
            Skills.Add(skill);
            Motions.Add(motion);
            
            preCommands.Add(preCommand);
            
            
        }
        #endregion
    }
    [Button(ButtonSizes.Medium,Expanded = true)]
    //------------------------------------------------------------------------------------------------------------------
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

    #region �ִϸ��̼� ����

    public void ChangeAnimation()
    {
        for (int i = 0; i < Commands.Count; i++)
        {
            Commands_State.Add(Commands[i],"Attack "+i.ToString());
            Commands_ReadyState.Add(Commands[i],"Attack_Ready "+i.ToString());
            /*
            if (Commands[i] < 10)
            {
                if (Commands[i] >= 5)
                {
                    skill_Ready_State = "Attack_Ready " + i.ToString();
                }
                else
                {
                    normal_Ready_State = "Attack_Ready " + i.ToString();
                }
            }
            */
        }
        
        AnimatorOverrideController animatorOverride =equipManager.UpdatedAnimatorOverrideController();
        
        #region ���� ���
        //animatorOverride[skill_Ready_State] = Motions[skill_Ready_Index].attack_Ready;
        //animatorOverride[normal_Ready_State] = Motions[skill_Ready_Index].attack_Ready;
        for (int i = 0; i < Motions.Count; i++)
        {
            //if(i== skill_Ready_Index|| i==normal_Ready_Index) continue;
            animatorOverride[Commands_State[Commands[i]]] = Motions[i].attack;
            animatorOverride[Commands_ReadyState[Commands[i]]] = Motions[i].attack_Ready;

        }
        #endregion

        anim.runtimeAnimatorController = animatorOverride;
    }
    

    #endregion
}
