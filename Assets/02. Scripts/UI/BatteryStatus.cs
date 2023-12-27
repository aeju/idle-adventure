using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 배터리 퍼센트
// 배터리 상태 : ~10, ~25, ~50, ~75, ~100 / charging
public class BatteryStatus : MonoBehaviour
{
    public TextMeshProUGUI batteryLevelText;

    public Image batteryImage;
    public Sprite[] batterySprites;
    public Sprite chargingSprite;

    void Update()
    {
        CheckBatteryStatus();
    }

    void CheckBatteryStatus()
    {
        // 퍼센트 
        float batteryValue = SystemInfo.batteryLevel; // Return 0 ~ 1
        int batteryPercentage = (int)(batteryValue * 100);
        
        if (batteryLevelText != null)
            batteryLevelText.text = "Battery Level: " + batteryPercentage + "%";
        
        // 이미지 
        UnityEngine.BatteryStatus batteryStatus = SystemInfo.batteryStatus;
        
        // 충전 중인지 확인 
        if (batteryStatus == UnityEngine.BatteryStatus.Charging)
        {
            SetBatterySprite(chargingSprite);
        }
        else // 충전 중이 아니면, 배터리 상태 이미지 표시 
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
            return 0; // 0-10%
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
