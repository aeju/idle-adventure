using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    // 전투력 계산 공식
    public int CalculateCombatPower(int maxHP, int attack, int defense)
    {
        CombatPower = maxHP + attack + defense;
        return CombatPower; 
    }
    
    // 기본 공격 계산식 
    public int CalculateAttackDamage(int attack, float attack_Multiplier)
    {
        int attackDamage = (int)(playerStats.Attack * (attack_Multiplier / 100));
        return attackDamage;
    }
    
    // 스킬 공격 계산식 
    public int CalculateSkillDamage(int attack, float skill_Multiplier)
    {
        int skillDamage = (int)(playerStats.Attack * (skill_Multiplier / 100));
        return skillDamage;
    }
}
