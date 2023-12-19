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
        current_Coin += coin;
        OnResourcesUpdated?.Invoke();
    }
}
