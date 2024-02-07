using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    
    
    // 기본 공격 계산식 (공격력 * 스킬 퍼센트)
    // 일반 공격 / 치명타 공격 
    public int CalculateAttackDamage(int attack, float attack_Multiplier, float critical_Multiplier)
    {
        float multiplier;
        
        if (UnityEngine.Random.value < 0.3f) // 30% 확률로 치명타
        {
            multiplier = critical_Multiplier;
        }
        else
        {
            multiplier = attack_Multiplier;
        }
        
        int attackDamage = (int)(playerStats.attack * (multiplier / 100));
        return attackDamage;
    }
    
    // 스킬 공격 계산식 
    public int CalculateSkillDamage(int attack, float skill_Multiplier)
    {
        int skillDamage = (int)(playerStats.attack * (skill_Multiplier / 100));
        return skillDamage;
    }
}
