using UnityEngine;
using UnityEngine.Serialization;

public class PotionManager : ResourceBase<PotionManager>
{
    public int healAmount = 50;
    public float autoPotionPercent = 0.7f; // 체력 퍼센트
    public float autoPotionCheckInterval = 1f; // 체크 간격 
    
    public void AddPotion(int amount)
    {
        AddResource(amount);
    }
    
    public bool UsePotion(PlayerStats playerStats)
    {
        int currentPotions = GetCurrentPotions();
        if (GetCurrentResource() > 0)
        {
            // 최대 HP 넘지 않도록
            Debug.Log($"Using potion. Potions before use: {currentPotions}");
            int newHP = Mathf.Min(playerStats.CurrentHP + healAmount, playerStats.maxHP); // 현재 HP + 회복량 vs MaxHP 중 작은값 선택
            playerStats.CurrentHP = newHP;
            UseResource(1);
            
            Debug.Log($"Potion used. Potions remaining: {GetCurrentPotions()}");
            return true;
        }
        return false;
    }
    
    public int GetCurrentPotions()
    {
        return GetCurrentResource();
    }

    // 자동 포션 사용 조건 확인
    public bool CheckAutoPotionConditions(PlayerStats playerStats)
    {
        float currentHPPercentage = (float)playerStats.CurrentHP / playerStats.maxHP; // 현재 HP 비율 계산 
        // 1. HP 확인 : 현재 HP 비율이 설정된 퍼센트 이하인지 
        bool isHPLow = currentHPPercentage <= autoPotionPercent;
        // 2. 포션 개수 확인 : 사용 가능한 포션이 있는지 
        bool hasPotions = GetCurrentPotions() > 0;
        // 두 조건 만족하면, 포션 자동으로 사용
        bool shouldUsePotion = isHPLow && hasPotions;

        return shouldUsePotion;
    }
}
