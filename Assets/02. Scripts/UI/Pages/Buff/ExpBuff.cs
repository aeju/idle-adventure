using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

[RequireComponent(typeof(RewardAdManager))]
public class ExpBuff : Buff
{
    // 지속시간 2분, 획득량 증가율 30%
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private TextMeshProUGUI buffEffectText;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private Image buffIconImage;
    
    void Start()
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
    
    // 버프 활성화 (경험치 획득량 증가)
    protected override void OnActivate()
    {
        BuffManager.Instance.ExpMultiplier += IncreasePercentage / 100.0f;
    }
}
