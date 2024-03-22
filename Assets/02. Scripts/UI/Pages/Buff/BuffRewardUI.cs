using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffRewardUI : MonoBehaviour
{
    [SerializeField] private CoinBuff coinBuff; // CoinBuff에 대한 참조
    [SerializeField] private RewardAdManager rewardAdManager;
    
    // 버프 활성화 이벤트
    public static event Action OnBuffActivated;

    void Start()
    {
        if (!coinBuff)
        {
            Debug.LogError("rewardAdManager is null");
            coinBuff = FindObjectOfType<CoinBuff>();
        }

        if (!rewardAdManager)
        {
            Debug.LogError("rewardAdManager is null");
            rewardAdManager = FindObjectOfType<RewardAdManager>();
        }
    }

    void Update()
    {
        // 터치 : 창 닫김 + 버프 적용 
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            rewardAdManager.isAdWatched = false;
        }
    }

    /*
    void BuffOn()
    {
        if (coinBuff != null)
        {
            coinBuff.Activate();
            
            // 버프 활성화 이벤트 발생
            OnBuffActivated?.Invoke();
        }
    }
    */
}
