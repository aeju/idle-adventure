using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BuffUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI buffNameText;
    [SerializeField] protected TextMeshProUGUI buffEffectText;
    [SerializeField] protected TextMeshProUGUI remainingTimeText;
    [SerializeField] protected Image buffIconImage;
    [SerializeField] protected Image blackImage;  // 남은 시간 표시위한, 검은색 이미지
    
    protected float remainingTime; // 남은 시간 (초)
    protected float totalDurationSeconds; // 총 지속 시간 (초)
    
    // 버프 활성화/비활성화될 때 호출
    protected abstract void UpdateBuffUI(Buff buff); 
    protected abstract void ClearBuffUI(Buff buff);
    
    // 이벤트에 메서드 구독 및 구독 취소
    protected void OnEnable()
    {
        Buff.OnBuffActivated += UpdateBuffUI; // 버프 활성화 시 UpdateBuffUI 호출           
        Buff.OnBuffDeactivated += ClearBuffUI; // 버프 비활성화 시 ClearBuffUI 호출
    }

    protected void OnDisable()
    {
        Buff.OnBuffActivated -= UpdateBuffUI; // 버프 활성화 이벤트 구독 취소            
        Buff.OnBuffDeactivated -= ClearBuffUI; // 버프 비활성화 이벤트 구독 취소
    }
    
    // 지속 시간이 끝날 때까지 매 초마다 UI를 업데이트하는 코루틴
    protected IEnumerator UpdateDurationLoop()
    {
        while (remainingTime > 0)
        {
            UpdateDurationDisplay(); // 현재 남은 시간으로 UI 업데이트
            UpdateBuffIcon(); // 버프 아이콘 상태 업데이트

            yield return new WaitForSeconds(1); // 1초 기다림
            remainingTime--; // 남은 시간 1초 감소
        }
        // 루프 종료 후, 남은 시간이 0 이하인 경우 한 번 더 UI 업데이트를 확실히 수행
        // (목적 : 남은 시간 음수 버그 방지)
        remainingTime = 0;
        UpdateDurationDisplay();
        UpdateBuffIcon();
    }

    // 남은 시간 표시
    protected void UpdateDurationDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60); // 분
        int seconds = Mathf.FloorToInt(remainingTime % 60); // 초
        remainingTimeText.text = $"남은 시간: {minutes}분 {seconds}초";
    }

    // 남은 시간에 따라 버프 아이콘의 UI를 업데이트 (남은 시간에 따라 검은 이미지의 fillAmount 조정)
    // 기본 구현, 하위 클래스에서 필요에 따라 오버라이드 가능
    protected virtual void UpdateBuffIcon()
    {
        // 이미지가 위에서 아래로 내려오게 함 
        blackImage.fillAmount = 1 - (remainingTime / totalDurationSeconds);
    }
}
