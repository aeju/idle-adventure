using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinBuffUI : BuffUI
{
    // 버프 활성화 상태에 따른 UI 업데이트 로직
    protected override void UpdateBuffUI(Buff buff)
    {
        // buff 객체가 coinBuff의 인스턴스인 경우
        // buff 타입을 CoinBuff 타입으로 캐스팅하여 coinBuff 변수에 할당
        var coinBuff = buff as CoinBuff;
        // 캐스팅 성공 시, CoinBuff 관련 정보로 UI 업데이트
        if (coinBuff != null)
        {
            // 전체 지속 시간, 남은 시간 설정
            totalDurationSeconds = Utilities.MinutesToSeconds(coinBuff.durationMinute); // 전체 지속 시간 설정
            remainingTime = totalDurationSeconds; // 남은 시간 초기화
            
            // 지속 시간을 UI에 반영 (이유 : 1초가 흐른 후에야 UI가 반영됨)
            UpdateDurationDisplay();
            
            // 남은 시간 감소 및 UI 업데이트 시작
            StartCoroutine(UpdateDurationLoop());

            buffNameText.text = coinBuff.buffName;
            buffEffectText.text = $"{coinBuff.buffEffect} {coinBuff.IncreasePercentage}% 증가";
            buffIconImage.sprite = coinBuff.buffIconSprite;
        }
    }
    
    // 버프 비활성화 상태에 따른 UI 클리어 로직
    protected override void ClearBuffUI(Buff buff)
    {
        StopAllCoroutines();
        remainingTime = 0;
        UpdateDurationDisplay(); // 버프 아이콘 업데이트
        blackImage.fillAmount = 1; // 검은 이미지 원래 상태로 복원
    }
}

