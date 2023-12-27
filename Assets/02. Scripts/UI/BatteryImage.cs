using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// // 배터리 상태 : ~10, ~25, ~50, ~75, ~100 / 충전 중 
public class BatteryImage : MonoBehaviour
{
    public Image batteryImage;
    public Sprite[] batterySprites;
    public Sprite chargingSprite;

    private UnityEngine.BatteryStatus lastBatteryStatus = UnityEngine.BatteryStatus.Unknown;

    void Update()
    {
        UpdateBatterySprite();
    }

    void UpdateBatterySprite()
    {
        UnityEngine.BatteryStatus batteryStatus = SystemInfo.batteryStatus;

        
        if (batteryStatus != lastBatteryStatus)
        {
            if (batteryStatus == UnityEngine.BatteryStatus.Charging)
            {
                SetBatterySprite(chargingSprite);
            }
            else
            {
                int spriteIndex = GetBatterySpriteIndex((int)(SystemInfo.batteryLevel * 100));
                if (spriteIndex >= 0 && spriteIndex < batterySprites.Length)
                {
                    SetBatterySprite(batterySprites[spriteIndex]);
                }
            }
            lastBatteryStatus = batteryStatus;
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
        if (batteryPercentage <= 10) return 0; // 0-10%
        else if (batteryPercentage <= 25)
            return 1; // 11-25%
        else if (batteryPercentage <= 50)
            return 2; // 26-50%
        else if (batteryPercentage <= 75)
            return 3; // 51-75%
        else
            return 4; // 76-100%
    }
}
