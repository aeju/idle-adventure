using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 업데이트 : 배터리값 변화가 있을 때만 
// 마지막 배터리 %, 현재 배터리 % 비교 필요 
public class BatteryText : MonoBehaviour
{
    public TextMeshProUGUI batteryText;

    private int lastBatteryPercentage = -1;

    void Update()
    {
        UpdateBatteryPercentage();
    }

    void UpdateBatteryPercentage()
    {
        float batteryValue = SystemInfo.batteryLevel; // Return 0 ~ 1
        int batteryPercentage = (int)(batteryValue * 100);

        if (batteryPercentage != lastBatteryPercentage) // %값 변화가 있을 때만 텍스트 업데이트 
        {
            if (batteryText != null)
            {
                batteryText.text = batteryPercentage + "%"; // 업데이트 
            }
            lastBatteryPercentage = batteryPercentage; // 현재 퍼센트 저장 
        }
    }
}
