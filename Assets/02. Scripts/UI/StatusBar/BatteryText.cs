using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 배터리 상태 변화에 따라 텍스트 업데이트
public class BatteryText : BatteryStatusTemplate
{
    [SerializeField] private TextMeshProUGUI batteryText;
    
    protected override void HandleBatteryChanged(int batteryPercentage, UnityEngine.BatteryStatus batteryStatus)
    {
        if (batteryStatus == UnityEngine.BatteryStatus.Unknown)
        {
            batteryText.text = "배터리 상태 알 수 없음";
        }
        else
        {
            batteryText.text = $"{batteryPercentage}%";
        }
    }
}