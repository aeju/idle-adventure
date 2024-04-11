using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// HH:mm:ss (절전 모드)
public class TimeTextHourMinSec : TimeTextUpdater
{
    protected override string GetTimeString()
    {
        return Utilities.GetCurrentTimeKST().ToString("HH:mm:ss");
    }
    
    /*
    [SerializeField] private TextMeshProUGUI timeTextHourMinSec;
    private float timeSinceLastUpdate = 0f; // 마지막 업데이트 이후 경과 시간
    
    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        
        // 1초마다 시간을 업데이트
        if (timeSinceLastUpdate >= 1f)
        {
            if (timeTextHourMinSec != null)
            {
                timeTextHourMinSec.text = Utilities.GetCurrentTimeKST().ToString("HH:mm:ss");
            }
            timeSinceLastUpdate = 0f;  // 마지막 업데이트 시간을 재설정
        }
    }
    */
}
