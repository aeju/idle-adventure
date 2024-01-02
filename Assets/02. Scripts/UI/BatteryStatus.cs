using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 배터리 상태 : ~10, ~25, ~50, ~75, ~100 / charging
public class BatteryStatus : MonoBehaviour
{
    public Image batteryImage;
    public Sprite[] batterySprites;
    public Sprite chargingSprite;

    void Update()
    {
        CheckBatteryStatus();
    }

    void CheckBatteryStatus()
    {
        float batteryValue = SystemInfo.batteryLevel; // Return 0 ~ 1
        int batteryPercentage = (int)(batteryValue * 100);
        
        UnityEngine.BatteryStatus batteryStatus = SystemInfo.batteryStatus;
        
        if (batteryStatus == UnityEngine.BatteryStatus.Charging)
        {
            SetBatterySprite(chargingSprite);
        }
        else
        {
            int spriteIndex = GetBatterySpriteIndex(batteryPercentage);
            if (spriteIndex >= 0 && spriteIndex < batterySprites.Length)
            {
                SetBatterySprite(batterySprites[spriteIndex]);
            }
        }
    }
    
    void SetBatterySprite(Sprite sprite)
    {
        if (batteryImage != null)
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