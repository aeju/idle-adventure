using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : EnforceObserver
{
    public static ResourceManager Instance { get; private set; }

    private PlayerEnforce playerEnforce;
    
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

        playerEnforce = (PlayerEnforce) FindObjectOfType(typeof(PlayerEnforce));
    }
    
    public void AddCoin(int coin)
    {
        current_Coin += coin;
        OnResourcesUpdated?.Invoke();
    }

    public override void Notify(EnforceSubject subject)
    {
        if (!playerEnforce)
            playerEnforce = subject.GetComponent<PlayerEnforce>();
        
    }
}
