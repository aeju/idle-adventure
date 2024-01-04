using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// BatteryManager : 배터리 상태 변화 -> BatteryStatus, BatteryText : 구독  
public class BatteryManager : MonoBehaviour
{
    public static BatteryManager Instance { get; private set; }

    // 배터리 상태가 변경될 때, 호출될 이벤트
    public event Action<int, UnityEngine.BatteryStatus> OnBatteryStatusChanged;

    // 마지막 저장된 배터리 퍼센트, 상태 
    private int lastBatteryPercentage = -1;
    private UnityEngine.BatteryStatus lastBatteryStatus = UnityEngine.BatteryStatus.Unknown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckBatteryStatus();
    }

    // 배터리 상태 확인 
    private void CheckBatteryStatus()
    {
        // 배터리 %
        float batteryValue = SystemInfo.batteryLevel; // Return 0 ~ 1
        int batteryPercentage = (int)(batteryValue * 100);
        
        // 배터리 상태 
        UnityEngine.BatteryStatus batteryStatus = SystemInfo.batteryStatus;

        // 배터리 퍼센트, 상태에 변화가 있을 경우 이벤트를 발생 
        if (batteryPercentage != lastBatteryPercentage || batteryStatus != lastBatteryStatus)
        {
            lastBatteryPercentage = batteryPercentage;
            lastBatteryStatus = batteryStatus;

            OnBatteryStatusChanged?.Invoke(batteryPercentage, batteryStatus);
        }
    }
}