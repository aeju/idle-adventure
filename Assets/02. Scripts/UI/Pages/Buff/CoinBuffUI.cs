using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinBuffUI : Buff
{
    [SerializeField] private Image buffIcon;
    [SerializeField] private TextMeshProUGUI durationDisplayText;
    private float remainingTime;

    protected override void OnActivate()
    {
        //remainingTime = duration * 60; // 지속 시간을 초 단위로 변환
        remainingTime = Utilities.MinutesToSeconds(duration);
        UpdateDurationDisplay();
        StartCoroutine(UpdateDurationLoop());
    }

    protected override void Deactivate()
    {
        StopAllCoroutines();
        remainingTime = 0;
        UpdateDurationDisplay();
        // +) 비활성화 됐을 때의 UI 변경 로직 
    }

    private IEnumerator UpdateDurationLoop()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainingTime--;
            UpdateDurationDisplay();
            UpdateBuffIcon();
        }
    }

    private void UpdateDurationDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        durationDisplayText.text = $"남은 시간: {minutes}분 {seconds}초";
    }

    private void UpdateBuffIcon()
    {
        // 남은 시간에 따라 버프 아이콘의 UI를 업데이트. 예를 들어, 남은 시간 비율에 따라 아이콘의 채워짐을 조정
        //buffIcon.fillAmount = remainingTime / (duration * 60);
        buffIcon.fillAmount = remainingTime / Utilities.MinutesToSeconds(duration);
    }
}
