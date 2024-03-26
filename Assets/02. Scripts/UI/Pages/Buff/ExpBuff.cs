using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class ExpBuff : Buff
{
    // 지속시간 1분, 획득량 증가율 20%
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private TextMeshProUGUI buffEffectText;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private Image buffIconImage;
    
    [SerializeField] private Button buffbtn; // 테스트용 버프 활성화 버튼 
    
    void Start()
    {
        if (BuffManager.Instance != null)
        {
            UpdateUI();
            
            /*
            buffbtn.OnClickAsObservable().Subscribe(_ =>
            {
                Activate();
            }).AddTo(this);
            */
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
        BuffManager.Instance.BuffIconOn();
        //BuffManager.Instance.CoinMultiplier += IncreasePercentage / 100.0f;
    }

    // 버프 비활성화 
    protected override void Deactivate()
    {
        BuffManager.Instance.BuffIconOff();
        //BuffManager.Instance.CoinMultiplier -= IncreasePercentage / 100.0f;
    }
}
