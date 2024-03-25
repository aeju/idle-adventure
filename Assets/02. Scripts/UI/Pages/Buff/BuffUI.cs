using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private TextMeshProUGUI buffEffectText;
    [SerializeField] private TextMeshProUGUI remainingTimeText;
    [SerializeField] private Image buffIconImage;
    [SerializeField] private Image blackImage;  // 남은 시간 표시위한, 검은색 이미지
    
    private float remainingTime; // 남은 시간 (초)
    private float totalDurationSeconds; // 총 지속 시간 (초)

    // 이벤트에 메서드 구독 및 구독 취소
    private void OnEnable()
    {
        BuffManager.Instance.OnBuffActivated += UpdateBuffUI; // 버프 활성화 시 UpdateBuffUI 호출           
        BuffManager.Instance.OnBuffDeactivated += ClearBuffUI; // 버프 비활성화 시 ClearBuffUI 호출
    }

    private void OnDisable()
    {
        BuffManager.Instance.OnBuffActivated -= UpdateBuffUI; // 버프 활성화 이벤트 구독 취소            
        BuffManager.Instance.OnBuffDeactivated -= ClearBuffUI; // 버프 비활성화 이벤트 구독 취소
    }
    
    // 버프 활성화 상태에 따른 UI 업데이트 로직
    private void UpdateBuffUI(Buff buff)
    {
        // 전체 지속 시간, 남은 시간 설정
        totalDurationSeconds = Utilities.MinutesToSeconds(buff.durationMinute); // 전체 지속 시간 설정
        remainingTime = totalDurationSeconds; // 남은 시간 초기화

        // 지속 시간을 UI에 반영 (이유 : 1초가 흐른 후에야 UI가 반영됨)
        UpdateDurationDisplay();

        // 남은 시간 감소 및 UI 업데이트 시작
        StartCoroutine(UpdateDurationLoop());

        // 버프 정보를 UI에 반영
        buffNameText.text = buff.buffName;
        buffEffectText.text = $"{buff.buffEffect} {buff.IncreasePercentage}% 증가";
        buffIconImage.sprite = buff.buffIconSprite;
    }
    
    // 버프 비활성화 상태에 따른 UI 클리어 로직
    private void ClearBuffUI(Buff buff)
    {
        StopAllCoroutines();
        remainingTime = 0;
        UpdateDurationDisplay(); // 버프 아이콘 업데이트
        blackImage.fillAmount = 1; // 검은 이미지 원래 상태로 복원
    }
    
    // 지속 시간이 끝날 때까지 매 초마다 UI를 업데이트하는 코루틴
    private IEnumerator UpdateDurationLoop()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1); // 1초 대기
            remainingTime--; // 남은 시간 감소
            UpdateDurationDisplay(); // 남은 시간 표시 업데이트
            UpdateBuffIcon(); // 버프 아이콘 업데이트
        }
    }

    // 남은 시간 표시
    private void UpdateDurationDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60); // 분
        int seconds = Mathf.FloorToInt(remainingTime % 60); // 초
        remainingTimeText.text = $"남은 시간: {minutes}분 {seconds}초";
    }

    // 남은 시간에 따라 버프 아이콘의 UI를 업데이트 (남은 시간에 따라 검은 이미지의 fillAmount 조정)
    // 기본 구현, 하위 클래스에서 필요에 따라 오버라이드 가능
    private void UpdateBuffIcon()
    {
        // 이미지가 위에서 아래로 내려오게 함 
        blackImage.fillAmount = 1 - (remainingTime / totalDurationSeconds);
    }
}
