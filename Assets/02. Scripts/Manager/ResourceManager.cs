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
        current_Coin += coin; // 코인 증가 
        Debug.Log($"Upgrade, Coin Updated: {current_Coin}"); 
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
