using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager> 
{
    public event Action OnResourcesUpdated;
    
    public int current_Ruby = 0;
    public int current_Coin = 0;
    public int current_summon_Ticket = 0;
    
    public void AddCoin(int coin)
    {
        // 버프에 따른 코인 계산을 항상 적용 (기본 coinMultiplier = 1)
        float adjustedCoin = coin * BuffManager.Instance.CoinMultiplier;
        // 정수로 변환하기 전, 반올림
        int finalCoin = Mathf.FloorToInt(adjustedCoin);
        
        // 추가로 획득한 코인을 추가
        current_Coin += finalCoin;
        OnResourcesUpdated?.Invoke();
    }
    
    public void UseCoin(int coin)
    {
        if (current_Coin >= coin)
        {
            current_Coin -= coin; // 코인 감소
            OnResourcesUpdated?.Invoke(); // 코인 변화에 대한 이벤트 트리거
        }
    }
}
