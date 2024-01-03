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
    public Sprite[] batterySprites;
    public Sprite chargingSprite;
    
    // 상태가 변경될 때만 업데이트 
    private int lastBatteryPercentage = -1;
    private UnityEngine.BatteryStatus lastBatteryStatus = UnityEngine.BatteryStatus.Unknown;

    void Update()
    {
        CheckBatteryStatus();
    }

    void CheckBatteryStatus()
    {
        float batteryValue = SystemInfo.batteryLevel; // Return 0 ~ 1
        int batteryPercentage = (int)(batteryValue * 100);
        UnityEngine.BatteryStatus batteryStatus = SystemInfo.batteryStatus;
        
        // 상태가 변경되었는지 확인하고 Unknown 상태를 처리
        if (batteryStatus == UnityEngine.BatteryStatus.Unknown)
        {
            batteryImage.enabled = false; // 배터리 이미지 비활성화
        }
        else
        {
            batteryImage.enabled = true; // 배터리 이미지 활성화

            // 상태가 변경되었는지 확인
            if (batteryPercentage != lastBatteryPercentage || batteryStatus != lastBatteryStatus)
            {
                lastBatteryPercentage = batteryPercentage;
                lastBatteryStatus = batteryStatus;

                // Charging(연결 o, 충전 o)이거나 Full(연결 o, 가득차서 충전 x)
                if (batteryStatus == UnityEngine.BatteryStatus.Charging || batteryStatus == UnityEngine.BatteryStatus.Full)
                {
                    SetBatterySprite(chargingSprite);
                }
                else
                {
                    SetBatterySprite(batterySprites[GetBatterySpriteIndex(batteryPercentage)]);
                }
            }
        }
    }
    
    void SetBatterySprite(Sprite sprite)
    {
        if (batteryImage != null  && batteryImage.sprite != sprite)
        {
            batteryImage.sprite = sprite;
        }
    }

    int GetBatterySpriteIndex(int batteryPercentage)
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