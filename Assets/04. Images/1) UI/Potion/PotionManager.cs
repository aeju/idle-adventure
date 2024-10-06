using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : Singleton<PotionManager>
{
    public int current_Potion = 0;
    
    public event Action OnPotionUpdated;
    
    public void AddPotion(int potion)
    {
        // 추가로 획득한 코인을 추가
        current_Potion += potion;
        OnPotionUpdated?.Invoke();
    }
    
    public void UsePotion(int potion)
    {
        if (current_Potion >= potion)
        {
            current_Potion -= potion; // 포션 감소
            OnPotionUpdated?.Invoke(); // 포션 변화에 대한 이벤트 트리거
        }
    }
}
