using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]

public class Skill : ScriptableObject

{
    public bool canLink;

    #region ��� �߰�,����
    [Space]
    [ShowInInspector]
    [ReadOnly]
    [ListDrawerSettings(Expanded = true)]
    [LabelText("�� ��� ����Ʈ(readOnly)")]
    public List<Motion> motions= new List<Motion>();
    
    
    
    [LabelText("��� �߰�")]
    [Button(ButtonSizes.Medium,ButtonStyle.Box,Expanded = true)]
    public void AddMotion(Motion motion)
    {
        bool checkList = (motion != null);
        
        if(checkList)motions.Add(motion);
        else Debug.LogWarning("����� �߰��� �� �����ϴ�!");
    }
    
    public List<Motion> GetMotions()
    {
        return motions;
    }
    
    [Button]
    public void ClearAll()
    {
        motions.Clear();
    }
    #endregion
    
}