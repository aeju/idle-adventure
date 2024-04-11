using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// HH:mm (상단바) 
public class TimeTextHourMin : TimeTextUpdater
{
    protected override string GetTimeString()
    {
        return Utilities.GetCurrentTimeKST().ToString("HH:mm");
    }
    
    /*
    [SerializeField] private TextMeshProUGUI timeTextHourMin;
    private float timeSinceLastUpdate = 0f; // 마지막 업데이트 이후 경과 시간
    
    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        
        // 1초마다 시간을 업데이트
        if (timeSinceLastUpdate >= 1f)
        {
            if (timeTextHourMin != null)
            {
                timeTextHourMin.text = Utilities.GetCurrentTimeKST().ToString("HH:mm");
            }
            timeSinceLastUpdate = 0f;  // 마지막 업데이트 시간을 재설정
        }
    }
    */
}
