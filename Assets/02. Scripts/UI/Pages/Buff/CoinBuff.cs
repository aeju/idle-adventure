using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class CoinBuff : Buff
{
    // 지속시간 1분, 획득량 증가율 20%
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI percentText;
    
    [SerializeField] private int coinIncreasePercentage = 20; // 코인 획득량 증가율

    [SerializeField] private Button buffbtn;
    
    private void Start()
    {
        if (BuffManager.Instance != null)
        {
            duration = 1;
            UpdateUI();
            
            buffbtn.OnClickAsObservable().Subscribe(_ =>
            {
                Debug.Log("buff: click1");
                Activate();
            }).AddTo(this);
        }
        else
        {
            Debug.Log("buff manager X");
        }
    }

    private void UpdateUI()
    {
        durationText.text = $"지속시간 : {duration}분";
        percentText.text = $"금화 획득량 {coinIncreasePercentage}% 증가 ";
    }

    // 버프 활성화 (코인 획득량 증가 로직을 구현
    protected override void OnActivate()
    {
        Debug.Log("1. buff: BuffManager.Instance.CoinMultiplier" + BuffManager.Instance.CoinMultiplier);
        BuffManager.Instance.BuffIconOn();
        BuffManager.Instance.CoinMultiplier += coinIncreasePercentage / 100.0f;
        Debug.Log("2. buff: BuffManager.Instance.CoinMultiplier" + BuffManager.Instance.CoinMultiplier);
    }

    // Deactivate : 버프가 끝났을 때의 로직을 구현
    protected override void Deactivate()
    {
        BuffManager.Instance.BuffIconOff();
        BuffManager.Instance.CoinMultiplier -= coinIncreasePercentage / 100;
    }
}
