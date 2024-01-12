using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 배터리 상태 : ~10, ~25, ~50, ~75, ~100 / charging
// 예외 : BatteryStatus.Unknown; -> 이미지 뜨지 x
// BatteryStatus : Unknown / 연결x : Discharging, Notcharging / 연결o: Charging, Full
public class BatteryStatus : MonoBehaviour
{
    public Image batteryImage;
    public Sprite[] batterySprites; // 충전 중x, 스프라이트 배열 (battery level_0, battery level_1, battery level_2, battery level_3, battery level_4)
    public Sprite chargingSprite; // 충전 중 (battery level_9)

    // BatteryManager 이벤트에 메서드 등록
    private void OnEnable()
    {
        BatteryManager.Instance.OnBatteryStatusChanged += UpdateBatteryImage;
    }

    private void OnDisable()
    {
        BatteryManager.Instance.OnBatteryStatusChanged -= UpdateBatteryImage;
    }

    private void UpdateBatteryImage(int batteryPercentage, UnityEngine.BatteryStatus batteryStatus)
    {
        // 배터리 상태 Unknown : 이미지 비활성화 
        if (batteryStatus == UnityEngine.BatteryStatus.Unknown) 
        {
            batteryImage.enabled = false;
            return;
        }
        else // 그 외 : 이미지 활성화 (Discharging, Notcharging / Charging, Full)
        {
            batteryImage.enabled = true;

            if (batteryStatus == UnityEngine.BatteryStatus.Charging || batteryStatus == UnityEngine.BatteryStatus.Full)
            {
                batteryImage.sprite = chargingSprite;
            }
            else
            {
                batteryImage.sprite = batterySprites[GetBatterySpriteIndex(batteryPercentage)];
            }
        }
    }

    // 배터리 퍼센트에 따라 스프라이트 인덱스 반환 
    private int GetBatterySpriteIndex(int batteryPercentage)
    {
        if (batteryPercentage <= 10)
            return 0;
        else if (batteryPercentage <= 25)
            return 1;
        else if (batteryPercentage <= 50)
            return 2;
        else if (batteryPercentage <= 75)
            return 3;
        else
            return 4;
    }
}