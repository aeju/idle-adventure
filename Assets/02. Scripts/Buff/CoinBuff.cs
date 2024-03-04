using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

// 시간 초 재서 
public class CoinBuff : Buff
{
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI percentText;
    
    [SerializeField] private int coinIncreasePercentage = 20; // 코인 획득량 증가율
    //[SerializeField] private int duration = 1; // 분

    [SerializeField] private Button buffbtn;
    
    private void Start()
    {
        UpdateUI();
        
        buffbtn.OnClickAsObservable().Subscribe(_ =>
        {
            duration = 1;
            OnActivate();
        }).AddTo(this);
    }

    private void UpdateUI()
    {
        durationText.text = $"지속시간 : {duration}분";
        percentText.text = $"금화 획득량 {coinIncreasePercentage}% 증가 ";
    }

    // 코인 획득량 증가 로직을 구현
    protected override void OnActivate()
    {
        BuffManager.Instance.BuffIconOn();
        BuffManager.Instance.CoinMultiplier += coinIncreasePercentage / 100;
    }

    // Deactivate에서는 버프가 끝났을 때의 로직을 구현
    protected override void Deactivate()
    {
        BuffManager.Instance.BuffIconOff();
        BuffManager.Instance.CoinMultiplier -= coinIncreasePercentage / 100;
    }
}
