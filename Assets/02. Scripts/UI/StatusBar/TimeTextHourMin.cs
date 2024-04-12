using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// HH:mm (상단바) 
public class TimeTextHourMin : TimeTextUpdater
{
    // 게임 시작 시 바로 현재 시간으로 초기화
    void Start()
    {
        if (timeText != null)
        {
            timeText.text = GetTimeString();
        }
    }
    
    protected override string GetTimeString()
    {
        return Utilities.GetCurrentTimeKST().ToString("HH:mm");
    }
}
