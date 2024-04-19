using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 배터리 상태 변화에 따라 텍스트 업데이트
public class BatteryText : BatteryStatusTemplate
{
    [SerializeField] private TextMeshProUGUI batteryText;

    void Start()
    {
        UpdateInitialText();
    }
    
    private void UpdateInitialText()
    {
        int currentPercentage = (int)(SystemInfo.batteryLevel * 100);
        UnityEngine.BatteryStatus currentStatus = SystemInfo.batteryStatus;
        HandleBatteryChanged(currentPercentage, currentStatus);
    }
    
    protected override void HandleBatteryChanged(int batteryPercentage, UnityEngine.BatteryStatus batteryStatus)
    {
        if (batteryStatus == UnityEngine.BatteryStatus.Unknown)
        {
            batteryText.text = "?%";
        }
        else
        {
            batteryText.text = $"{batteryPercentage}%";
        }
    }
}