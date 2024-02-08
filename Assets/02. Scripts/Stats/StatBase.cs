using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBase : ScriptableObject
{
    // 공통 스탯
    public int MaxHP; // 생명력 
    public int Attack; // 공격력 
    public int Defense; // 방어력
    public int Movement_Speed; // 이동속도
    public string Name;
    
    public float Attack_Multiplier; // 기본 공격 퍼센트
    public float Critical_Multiplier; // 치명타 퍼센트
}




