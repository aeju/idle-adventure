using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 업데이트 : 배터리값 변화가 있을 때만 
// 마지막 배터리 %, 현재 배터리 % 비교 필요 
public class BatteryText : MonoBehaviour
{
    public TextMeshProUGUI batteryText;

    // BatteryManager 이벤트에 메서드 등록
    private void OnEnable()
    {
        BatteryManager.Instance.OnBatteryStatusChanged += UpdateBatteryText;
    }

    private void OnDisable()
    {
        BatteryManager.Instance.OnBatteryStatusChanged -= UpdateBatteryText;
    }

    private void UpdateBatteryText(int batteryPercentage, UnityEngine.BatteryStatus batteryStatus)
    {
        if (batteryStatus == UnityEngine.BatteryStatus.Unknown)
        {
            batteryText.text = "배터리 상태 알 수 없음";
        }
        else
        {
            batteryText.text = batteryPercentage + "%";
        }
    }
}