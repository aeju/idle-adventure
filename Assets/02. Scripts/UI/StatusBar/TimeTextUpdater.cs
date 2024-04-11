using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class TimeTextUpdater : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI timeText;
    private float timeSinceLastUpdate = 0f; // 마지막 업데이트 이후 경과 시간

    // 현재 시간, 문자열로 변환하는 추상 메서드
    protected abstract string GetTimeString(); 

    // 1초마다 시간을 업데이트
    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= 1f && timeText != null)
        {
            timeText.text = GetTimeString();
            timeSinceLastUpdate = 0f; // 마지막 업데이트 시간을 재설정
        }
    }
}
