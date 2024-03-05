using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour 
{
    public static ResourceManager Instance { get; private set; }

    public event Action OnResourcesUpdated;
    
    public int current_Ruby = 0;
    public int current_Coin = 0;
    public int current_summon_Ticket = 0;

    // 버프
    public float coinBonusMultiplier = 0f;
    
    // 코인 보너스 증가
    public void IncreaseCoinBonus(float percentage)
    {
        coinBonusMultiplier += percentage / 100f;
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddCoin(int coin)
    {
        int finalCoin = coin; // 최종 획득 코인

        // 버프가 적용된 경우
        if (BuffManager.Instance.CoinMultiplier > 1)
        {
            Debug.Log("buff o, BuffManager.Instance.CoinMultiplier : " + BuffManager.Instance.CoinMultiplier);
            // 버프에 따른 추가 코인 계산
            finalCoin = Mathf.FloorToInt(coin * BuffManager.Instance.CoinMultiplier);
        }

        else
        {
            Debug.Log("buff x, BuffManager.Instance.CoinMultiplier : " + BuffManager.Instance.CoinMultiplier);
        }
        
        // 코인 추가
        current_Coin += finalCoin;
        OnResourcesUpdated?.Invoke();
        
        Debug.Log($"획득 코인: {finalCoin}, 총 코인: {current_Coin}");
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
