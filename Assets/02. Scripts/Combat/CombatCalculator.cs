using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatCalculator 
{
    // 기본 공격 계산 (Player, Monster 공유)
    // 30% 확률로 치명타 
    public static int CalculateAttackDamage(int attack, int defense, float attack_Multiplier, float critical_Multiplier)
    {
        // 치명타 여부 결정
        bool isCriticalHit = UnityEngine.Random.value < 0.3f;
        float multiplier = isCriticalHit ? critical_Multiplier : attack_Multiplier;

        // 원래 공격 데미지 계산
        float rawDamage = attack * (multiplier / 100);
        
        // 데미지 감소 계산 (방어력 아무리 높아져도, 데미지 감소율 100% X)
        float damageReductionMultiplier = 1 - (defense / (defense + 50.0f)); 
        float finalDamage = rawDamage * damageReductionMultiplier;

        // 최종 데미지가 0보다 작지 않도록 보장 (최종 데미지 -> 음수여도, 최종 데미지 0이도록 = 음수 데미지 방지)
        if (finalDamage < 0) 
            finalDamage = 0;

        return (int)finalDamage;
    }

    // 플레이어만 사용 
    public static int CalculateSkillDamage(int attack, int defense, float skillMultiplier)
    {
        // 기존 스킬 데미지 계산
        float rawSkillDamage = attack * (skillMultiplier / 100);

        // 방어력 반영 데미지 감소 계산 
        float damageReductionMultiplier = 1 - (defense / (defense + 50.0f)); 
        int finalSkillDamage = (int)(rawSkillDamage * damageReductionMultiplier);

        // 최종 스킬 데미지가 0보다 작지 않도록 보장
        if (finalSkillDamage < 0) 
            finalSkillDamage = 0;

        return (int)finalSkillDamage;
    }
    
    // 전투력 계산 공식
    public static int CalculateCombatPower(int attack, int maxHP, int defense)
    {
        int combatPower = 500 + (int)((attack * 10) + (maxHP * 0.5) +  (defense * 2));
        return combatPower; 
    }
}
