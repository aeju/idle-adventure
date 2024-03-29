using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

[RequireComponent(typeof(RewardAdManager))]
public class CoinBuff : Buff
{
    // 지속시간 1분, 획득량 증가율 20%
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private TextMeshProUGUI buffEffectText;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private Image buffIconImage;
    
    private void Start()
    {
        if (BuffManager.Instance != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.LogError("buff manager X");
        }
    }

    protected override void UpdateUI()
    {
        buffIconImage.sprite = buffIconSprite;
        buffNameText.text = buffName;
        buffEffectText.text = $"{buffEffect} {IncreasePercentage}% 증가";
        durationText.text = $"지속시간 : {durationMinute}분";
    }

    // 버프 활성화 (코인 획득량 증가)
    protected override void OnActivate()
    {
        Debug.Log("1. buff: BuffManager.Instance.CoinMultiplier" + BuffManager.Instance.CoinMultiplier);
        BuffManager.Instance.CoinMultiplier += IncreasePercentage / 100.0f;
        Debug.Log("2. buff: BuffManager.Instance.CoinMultiplier" + BuffManager.Instance.CoinMultiplier);
    }

    // 버프 비활성화 
    protected override void Deactivate()
    {
        BuffManager.Instance.CoinMultiplier -= IncreasePercentage / 100.0f;
    }
}
