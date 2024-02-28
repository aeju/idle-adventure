using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatCalculator 
{
    // 기본 공격 계산 (Player, Monster 공유)
    // 30% 확률로 치명타 
    public static int CalculateAttackDamage(int attack, float attack_Multiplier, float critical_Multiplier)
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
        int attackDamage = (int)(attack * (multiplier / 100));
        return attackDamage;
    }

    // 플레이어만 사용 
    public static int CalculateSkillDamage(int attack, float skillMultiplier)
    {
        int skillDamage = (int)(attack * (skillMultiplier / 100));
        return skillDamage;
    }
    
    // 전투력 계산 공식
    public static int CalculateCombatPower(int attack, int maxHP, int defense)
    {
        //combatPower = maxHP + attack + defense;
        int combatPower = (attack * 10) + maxHP +  defense;
        return combatPower; 
    }
}
