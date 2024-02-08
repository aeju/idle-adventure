using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "Character Stat/Player Stat")]
public class PlayerStat : StatBase
{
    public string Class; // 직업 종류
    public int Critical_Hit_Rate;
    
    public float Skill_Multiplier; // 스킬 공격 퍼센트

    public int Attack_Distance;
}
