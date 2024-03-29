using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffRewardUI : MonoBehaviour
{
    [SerializeField] private Buff buff; 
    [SerializeField] private RewardAdManager rewardAdManager;
    [SerializeField] private Image buffIconImage;
    [SerializeField] private TextMeshProUGUI buffEffectText;
    
    void Start()
    {
        UpdateUI(buff);
        
        if (!buff)
        {
            Debug.LogError("buff is null");
            buff = FindObjectOfType<Buff>();
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

            BuffOn();
        }
    }
    
    void BuffOn()
    {
        if (buff != null)
        {
            buff.Activate();
        }
        
        else
        {
            Debug.LogError("buff is null");
        }
    }

    void UpdateUI(Buff buff)
    {
        buffEffectText.text = $"버프 효과: {buff.buffEffect} {buff.IncreasePercentage}% 증가";
        buffIconImage.sprite = buff.buffIconSprite;
    }
}
