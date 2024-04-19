using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 배터리 상태 변화에 따라 이미지 업데이트
// 배터리 상태 : ~10, ~25, ~50, ~75, ~100 / charging
// 예외 : BatteryStatus.Unknown; -> 이미지 뜨지 x
// BatteryStatus : Unknown / 연결x : Discharging, Notcharging / 연결o: Charging, Full
public class BatteryStatus : BatteryStatusTemplate
{
    [SerializeField] private Image batteryImage;
    [SerializeField] private Sprite[] batterySprites; 
    [SerializeField] private Sprite chargingSprite; 
    
    void Start()
    {
        UpdateInitialImage();
    }
    
    private void UpdateInitialImage()
    {
        int currentPercentage = (int)(SystemInfo.batteryLevel * 100);
        UnityEngine.BatteryStatus currentStatus = SystemInfo.batteryStatus;
        HandleBatteryChanged(currentPercentage, currentStatus);
    }
    
    protected override void HandleBatteryChanged(int batteryPercentage, UnityEngine.BatteryStatus batteryStatus)
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
        else // 100 이하
            return 4;
    }
}