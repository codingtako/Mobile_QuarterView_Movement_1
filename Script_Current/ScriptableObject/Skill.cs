using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]

public class Skill : ScriptableObject

{
    public bool canLink;

    #region 모션 추가,삭제
    [Space]
    [ShowInInspector]
    [ReadOnly]
    [ListDrawerSettings(Expanded = true)]
    [LabelText("현 모션 리스트(readOnly)")]
    public List<Motion> motions= new List<Motion>();
    
    
    
    [LabelText("모션 추가")]
    [Button(ButtonSizes.Medium,ButtonStyle.Box,Expanded = true)]
    public void AddMotion(Motion motion)
    {
        bool checkList = (motion != null);
        
        if(checkList)motions.Add(motion);
        else Debug.LogWarning("모션을 추가할 수 없습니다!");
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