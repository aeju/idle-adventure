using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "Character Stat/Player Stat")]
public class PlayerStat : StatBase
{
    public int Critical_Hit_Rate; // 치명타 확률
    public int Accuracy; // 명중 
    public int HP_Recovery; // 생명력 회복
    public string Class; // 직업 종류
    public string Character_Name; // 캐릭터 이름

    public float Attack_Multiplier; // 기본 공격 퍼센트
    public float Critical_Multiplier; // 치명타 퍼센트
    public float Skill_Multiplier; // 스킬 공격 퍼센트

    public int Attack_Distance;
}
